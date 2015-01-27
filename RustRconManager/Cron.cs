using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RustRcon;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace RustRconManager
{
    class Cron
    {
        private Dictionary<string, DateTime> crons;
        private Rcon rcon;

        public Cron()
        {
            this.crons = new Dictionary<string, DateTime>()
            {
                {"cron_announce", DateTime.Now},
                //{"cron_airdrop", DateTime.Now},
                {"cron_count", DateTime.Now},
                {"cron_userstats", DateTime.Now},
                {"cron_hourly", DateTime.Now},
                {"cron_ig_hourly", DateTime.Now},
                {"cron_irc", DateTime.Now},
                {"cron_actions", DateTime.Now}
            };

            Dictionary<string, DateTime> overwrite = Data.Instance.GetCrons();
            foreach (string cron in overwrite.Keys)
            {
                if (this.crons.ContainsKey(cron))
                    this.crons.Remove(cron);
                this.crons.Add(cron, overwrite[cron]);
            }
        }

        public void Run(Rcon rcon)
        {
            this.rcon = rcon;

            new Task(() => { cron_announce(); }).Start();
            new Task(() => { cron_airdrop(); }).Start();
            new Task(() => { cron_count(); }).Start();
            new Task(() => { cron_userstats(); }).Start();
            new Task(() => { cron_hourly(); }).Start();
            new Task(() => { cron_ig_hourly(); }).Start();
            new Task(() => { cron_irc(); }).Start();
            new Task(() => { cron_actions(); }).Start();
        }

        private void cron_count()
        {
            TimeSpan wait = (this.crons["cron_count"] - DateTime.Now);
            if (wait.TotalSeconds > 1)
                Thread.Sleep(wait);

            while (true)
            {
                Package count = new Package("status");
                count.RegisterCallback((Package package) =>
                {
                    Match match = Regex.Match(package.Response, Program.validations["playercount"]);
                    int amount = int.Parse(match.Groups[1].Value);

                    Data.Instance.WriteKey(amount);

                    if (amount == 0)
                        return;
                    if (amount == 1)
                    {
                        rcon.Send(Language.Say("alone", Language.GERMAN));
                        rcon.Send(Language.Say("alone", Language.ENGLISH));
                        return;
                    }
                    rcon.Send(Language.Say("count", Language.GERMAN, match.Groups[1].Value));
                    rcon.Send(Language.Say("count", Language.ENGLISH, match.Groups[1].Value));
                });
                rcon.SendPackage(count);
                Data.Instance.SaveCron("cron_count", DateTime.Now.AddMilliseconds(1000 * 86));
                Thread.Sleep(1000 * 86);
            }
        }

        private void cron_announce()
        {
            TimeSpan wait = (this.crons["cron_announce"] - DateTime.Now);
            if (wait.TotalSeconds > 1)
                Thread.Sleep(wait);

            int announce = 1;
            while (true)
            {
                if (Language.Say(String.Format("announce-{0}", announce)) == null)
                    announce = 1;

                rcon.Send(Language.Say(String.Format("announce-{0}", announce), Language.GERMAN));
                rcon.Send(Language.Say(String.Format("announce-{0}", announce++), Language.ENGLISH));

                Data.Instance.SaveCron("cron_announce", DateTime.Now.AddMilliseconds(1000 * 60));
                Thread.Sleep(1000 * 150);
            }
        }

        private void cron_airdrop()
        {
            TimeSpan wait = (this.crons["cron_airdrop"] - DateTime.Now);
            if (wait.TotalSeconds > 1)
                Thread.Sleep(wait);

            bool swap = false;
            while (true)
            {
                Package count = new Package("status");
                count.RegisterCallback((Package response) =>
                    {
                        Match match = Regex.Match(response.Response, Program.validations["playercount"]);
                        int playercount = int.Parse(match.Groups[1].Value);
                        int drop = 0;
                        if (playercount > 50)
                            drop = 2;
                        else if (playercount > 20)
                            drop = 1;
                        else if (playercount > 10)
                        {
                            swap = !swap;
                            if (swap)
                                drop = 1;
                        }

                        if (drop > 0)
                        {
                            for (int i = 0; i < drop; i++)
                                rcon.Send("airdrop.drop");
                            rcon.Send(Language.Say("airdrop-incoming"));
                        }
                    });
                rcon.SendPackage(count);
                int waiting = 1000 * 60 * 30 + new Random().Next(0, 1000 * 60 * 15);
                Data.Instance.SaveCron("cron_airdrop", DateTime.Now.AddMilliseconds(waiting));
                Thread.Sleep(waiting);
            }
        }

        private Dictionary<string, DateTime> user_timeout = new Dictionary<string, DateTime>();
        private void cron_userstats()
        {
            TimeSpan wait = (this.crons["cron_userstats"] - DateTime.Now);
            if (wait.TotalSeconds > 1)
                Thread.Sleep(wait);

            List<User> floatings = new List<User>();

            while (true)
            {
                Package status = new Package("status");
                status.RegisterCallback((Package result) =>
                    {
                        MatchCollection matches = Regex.Matches(result.Response, Program.validations["user-lines"]);
                        List<User> new_floatings = new List<User>();

                        foreach (Match submatch in matches)
                        {
                            User floating = new User(submatch.Groups[1].Value, submatch.Groups[2].Value, submatch.Groups[4].Value, submatch.Groups[5].Value);
                            User bound = Data.Instance.GetUserById(floating.ID);
                            new_floatings.Add(floating);

                            // Check user online state
                            if (bound == null || !bound.Online)
                            {
                                rcon.Inject(new Package(0, 4, String.Format("User Connected: {0} ({1})", floating.SafeName, floating.ID)));
                                continue;
                            }

                            User pre_floating = floatings.Where(match => match.ID == floating.ID).FirstOrDefault();
                            if (pre_floating != null)
                            {
                                Auth previous = bound.Auth;
                                if (pre_floating.Time < floating.Time)
                                {
                                    // Default add (10s -> 20s, add 10s)
                                    bound.Time += floating.Time - pre_floating.Time;
                                    Data.Instance.UserAddTime(bound, floating.Time - pre_floating.Time);
                                }
                                else if (pre_floating.Time > floating.Time)
                                {
                                    // Disconnect at 10 seconds, reconnect with 5 seconds, add 5 seconds
                                    bound.Time += floating.Time;
                                    Data.Instance.UserAddTime(bound, floating.Time);
                                }
                                else
                                {
                                    // Unlikely, don't do anything.
                                    // Only case is when user connects, stays 3 seconds, disconnects, then connects and is scanned after 3 seconds
                                    // They can only lose up to 10 seconds
                                }

                                if (bound.Auth != previous) // Auth change
                                {
                                    string text = Language.Say(String.Format("vip-{0}-grats", bound.Auth.ToString()), bound, bound.Name, bound.Auth);
                                    if (text != null)
                                        rcon.Send(text);
                                }
                                bound.Save();
                            }
                        }
                        foreach (User user in Data.Instance.GetOnlineUsers())
                        {
                            if (new_floatings.Count(match => match.ID == user.ID) == 0)
                            {
                                if (!user_timeout.ContainsKey(user.ID))
                                {
                                    user_timeout.Add(user.ID, DateTime.Now);
                                    rcon.Send(Language.Say("tabout", user, user.Colour, user.Name));
                                }
                                if (user_timeout[user.ID].AddMinutes(5) < DateTime.Now)
                                {
                                    user_timeout.Remove(user.ID);
                                    rcon.Send(Language.Say("timeout", user, user.Colour, user.Name));
                                    rcon.Inject(new Package(0, 4, String.Format("User Disconnected: {0}", user.SafeName)));
                                    continue;
                                }
                            }
                            else if (user_timeout.ContainsKey(user.ID))
                            {
                                user_timeout.Remove(user.ID);
                                rcon.Send(Language.Say("tabin", user, user.Colour, user.Name));
                            }
                        }
                        floatings = new_floatings;
                    });
                rcon.SendPackage(status);

                Data.Instance.SaveCron("cron_userstats", DateTime.Now.AddMilliseconds(1000 * 10));
                Thread.Sleep(1000 * 10);
            }
        }

        private void cron_hourly()
        {
            DateTime target = DateTime.Now;
            target = target.AddMinutes(-1 * target.Minute).AddSeconds(-1 * target.Second).AddHours(1);
            if ((target - DateTime.Now).TotalSeconds > 1)
                Thread.Sleep(target - DateTime.Now);

            while (true)
            {
                rcon.Send(Language.Say("hourly", Language.GERMAN, DateTime.Now.ToString("HH:mm")));
                rcon.Send(Language.Say("hourly", Language.ENGLISH, DateTime.Now.ToString("HH:mm")));

                target = DateTime.Now;
                target = target.AddMinutes(-1 * target.Minute).AddSeconds(-1 * target.Second).AddHours(1);
                Data.Instance.SaveCron("cron_hourly", target);
                Thread.Sleep(target - DateTime.Now);
            }
        }

        private void cron_ig_hourly()
        {
            TimeSpan wait = (this.crons["cron_ig_hourly"] - DateTime.Now);
            if (wait.TotalSeconds > 1)
                Thread.Sleep(wait);

            int previous = -1;
            while (true)
            {
                //rcon.Send("env.time 12");
                Package time = new Package("env.time");
                time.RegisterCallback((Package result) =>
                {
                    Match match = Regex.Match(result.Response, Program.validations["time"]);

                    double current = double.Parse(match.Groups[1].Value);
                    int full = (int)Math.Floor(current);
                    if (previous == -1)
                        previous = full;
                    if ((full == 0 || full == 6 || full == 12 || full == 18) && full != previous)
                    {
                        rcon.Send(Language.Say("ighourly", Language.GERMAN, new DateTime(1991, 1, 1).AddHours(double.Parse(match.Groups[1].Value)).ToString("HH:mm")));
                        rcon.Send(Language.Say("ighourly", Language.ENGLISH, new DateTime(1991, 1, 1).AddHours(double.Parse(match.Groups[1].Value)).ToString("HH:mm")));
                        previous = full;
                    }
                });
                rcon.SendPackage(time);

                Data.Instance.SaveCron("cron_hourly", DateTime.Now.AddSeconds(2));
                Thread.Sleep(1000 * 2);
            }
        }

        private void cron_irc()
        {
            TimeSpan wait = (this.crons["cron_irc"] - DateTime.Now);
            if (wait.TotalSeconds > 1)
                Thread.Sleep(wait);

            while (true)
            {
                List<ChatLog> irc = Data.Instance.ReadFromIRC();
                if (irc != null && irc.Count > 0)
                    foreach (ChatLog log in irc)
                        rcon.Send(Language.Say("irc", Language.GERMAN, log.User.Name, log.Text));

                Data.Instance.SaveCron("cron_irc", DateTime.Now.AddSeconds(1));
                Thread.Sleep(1000);
            }
        }

        private void cron_actions()
        {
            TimeSpan wait = (this.crons["cron_actions"] - DateTime.Now);
            if (wait.TotalSeconds > 1)
                Thread.Sleep(wait);

            while (true)
            {
                Dictionary<int, string> actions = Data.Instance.GetActions();
                foreach (int key in actions.Keys)
                {
                    string action = actions[key];
                    if (action.StartsWith("--warn--"))
                    {
                        Commands.warn(Data.Instance.GetUserById(action.Substring("--warn--".Length)), rcon);
                    }
                    else
                        rcon.Send(action);
                    Data.Instance.RemoveAction(key);
                }

                Data.Instance.SaveCron("cron_actions", DateTime.Now.AddSeconds(2));
                Thread.Sleep(2000);
            }
        }
    }
}
