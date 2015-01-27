using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RustRcon;
using System.Threading;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading.Tasks;

namespace RustRconManager
{
    class Commands
    {
        private Rcon rcon;

        public Commands(Rcon rcon)
        {
            this.rcon = rcon;
        }

        private Dictionary<string, string> commands = new Dictionary<string, string>()
        {
            // Hilfe
            {"help", "cmd_info_help"},
            {"commands", "cmd_info_help"},
            {"command", "cmd_info_help"},
            {"hilfe", "cmd_info_help"},
            {"befehl", "cmd_info_help"},
            {"befehle", "cmd_info_help"},

            // Airdrop
            //{"airdrop", "cmd_info_airdrop"},
            //{"drop", "cmd_info_airdrop"},

            // Suicide
            {"suicide", "cmd_info_suicide"},
            {"kill", "cmd_info_suicide"},

            // Remove
            {"remove", "cmd_info_remove"},
            {"löschen", "cmd_info_remove"},

            // Services
            //{"pve", "cmd_info_pve"},
            {"service", "cmd_info_services"},
            {"services", "cmd_info_services"},
            {"ts", "cmd_info_services"},
            {"teamspeak", "cmd_info_services"},
            {"mumble", "cmd_info_services"},
            {"site", "cmd_info_services"},
            {"page", "cmd_info_services"},
            {"website", "cmd_info_services"},
            {"web", "cmd_info_services"},
            {"webpage", "cmd_info_services"},
            {"hp", "cmd_info_services"},
            {"homepage", "cmd_info_services"},
            {"voice", "cmd_info_services"},

            // Admin
            {"auth", "cmd_auth"},
            {"tp", "cmd_teleport"},
            {"watch", "cmd_watch"},
            {"unwatch", "cmd_unwatch"},
            {"warn", "cmd_warn"},
            {"unwarn", "cmd_unwarn"},

            // Lost
            {"lost", "cmd_lost"},
            {"verlaufen", "cmd_lost"},

            // Quit
            {"quit", "cmd_quit"},

            // Whois
            {"who", "cmd_who"},
            {"whois", "cmd_who"},
            {"player", "cmd_who"},
            {"vip", "cmd_who"},
            {"seen", "cmd_who"},
            {"lookup", "cmd_who"},
            {"last", "cmd_who"},
            {"wer", "cmd_who"},

            {"count", "cmd_count"},
            {"spieler", "cmd_count"},
            {"list", "cmd_count"},
            {"liste", "cmd_count"},
            {"players", "cmd_count"},
            {"status", "cmd_count"},

            {"time", "cmd_time"},
            {"zeit", "cmd_time"},

            {"kit", "cmd_kit"},

            {"tpr", "cmd_teleport_request"},
            {"tpa", "cmd_teleport_accept"},

            {"log", "cmd_log"},
            {"chat", "cmd_log"},
            {"history", "cmd_log"},
            {"verlauf", "cmd_log"},

            {"admin", "cmd_authed"},
            {"admins", "cmd_authed"},

            //{"pvp", "cmd_towns"},
            {"town", "cmd_towns"},
            {"towns", "cmd_towns"},
            {"state", "cmd_towns"},
            {"states", "cmd_towns"},
            {"stadt", "cmd_towns"},
            {"städte", "cmd_towns"},
            {"staat", "cmd_towns"},
            {"ort", "cmd_towns"},
            {"orte", "cmd_towns"},

            {"ping", "cmd_ping"},
/*
            {"location", "cmd_location"},
            {"gps", "cmd_location"},
            {"koordinaten", "cmd_location"},
            {"coords", "cmd_location"},
            {"coordinates", "cmd_location"},
*/
            {"report", "cmd_report"},

            /*
            {"karma", "cmd_karma"},
            {"good", "cmd_karma_good"},
            {"bad", "cmd_karma_bad"},
            {"gut", "cmd_karma_good"},
            {"böse", "cmd_karma_bad"},
            {"happy", "cmd_karma_good"},
            {"made", "cmd_karma_bad"},
            */

            // Groups
            {"group", "cmd_group"},
            {"groups", "cmd_group"},
            {"gruppe", "cmd_group"},
            {"gruppen", "cmd_group"},

            {"gcreate", "cmd_group_create"},
            {"ginvite", "cmd_group_invite"},
            {"gaccept", "cmd_group_accept"},
            {"gkick", "cmd_group_kick"},
            {"gquit", "cmd_group_quit"},

            {"alliance", "cmd_alliance"},
            {"ally", "cmd_alliance"},
            {"allianz", "cmd_alliance"},

            {"acreate", "cmd_alliance_create"},
            {"ainvite", "cmd_alliance_invite"},
            {"aaccept", "cmd_alliance_accept"},
            {"akick", "cmd_alliance_kick"},
            {"aquit", "cmd_alliance_quit"}

            /*
            {"chute", "cmd_parachute"},
            {"para", "cmd_parachute"},
            {"fallschirm", "cmd_parachute"},
            {"parachute", "cmd_parachute"},
            {"schirm", "cmd_parachute"},

            {"test_ping", "cmd_test_ping"},
            {"test_bans", "cmd_test_bans"}
             * */
        };

        public void Run(User user, string command, string parameter, string[] parameters)
        {
            if (commands.ContainsKey(command))
                this.GetType().GetMethod(commands[command]).Invoke(this, new object[] { user, command, parameter, parameters });
        }

        public TimeSpan CommandTime(string ident)
        {
            return CommandTime(ident, DateTime.Now, false);
        }

        public TimeSpan CommandTime(string ident, DateTime time, bool set)
        {
            DateTime remaining = Data.Instance.GetCommandTime(ident);

            if (set)
                Data.Instance.SetCommandTime(ident, time);

            if (DateTime.Now < remaining) // Not yet over threshold
                return remaining - DateTime.Now;

            return new TimeSpan(0);
        }

        public void cmd_info_help(User user, string command, string parameter, string[] parameters)
        {
            if (CommandTime("help-global", DateTime.Now.AddSeconds(10), true).TotalSeconds > 0)
                return;

            rcon.Send(Language.Say("help1", user));
            rcon.Send(Language.Say("help2", user));
            rcon.Send(Language.Say("help3", user));
            rcon.Send(Language.Say("help4", user));
            rcon.Send(Language.Say("help5", user));
        }

        public void cmd_info_airdrop(User user, string command, string parameter, string[] parameters)
        {
            if (CommandTime("airdrop-global", DateTime.Now.AddSeconds(10), true).TotalSeconds > 0)
                return;

            rcon.Send(Language.Say("airdrop1", user));
            rcon.Send(Language.Say("airdrop2", user));
            rcon.Send(Language.Say("airdrop3", user));
            rcon.Send(Language.Say("airdrop4", user));
        }

        public void cmd_info_suicide(User user, string command, string parameter, string[] parameters)
        {
            if (CommandTime("suicide-global", DateTime.Now.AddSeconds(10), true).TotalSeconds > 0)
                return;

            rcon.Send(Language.Say("suicide", user));
        }

        public void cmd_info_remove(User user, string command, string parameter, string[] parameters)
        {
            if (CommandTime("remove-global", DateTime.Now.AddSeconds(10), true).TotalSeconds > 0)
                return;

            rcon.Send(Language.Say("remove", user));
        }

        public void cmd_info_pve(User user, string command, string parameter, string[] parameters)
        {
            if (CommandTime("pve-global", DateTime.Now.AddSeconds(10), true).TotalSeconds > 0)
                return;

            rcon.Send(Language.Say("pve", user));
        }

        public void cmd_info_services(User user, string command, string parameter, string[] parameters)
        {
            if (CommandTime("services-global", DateTime.Now.AddSeconds(10), true).TotalSeconds > 0)
                return;

            rcon.Send(Language.Say("services1", user));
            rcon.Send(Language.Say("services2", user));
            rcon.Send(Language.Say("services3", user));
        }

        private Dictionary<string, DateTime> lost_approve = new Dictionary<string, DateTime>();
        public void cmd_lost(User user, string command, string parameter, string[] parameters)
        {
            TimeSpan cmdtime = CommandTime(String.Format("lost-{0}", user.ID));
            if (cmdtime.TotalSeconds > 0) // Restriction
            {
                rcon.Send(Language.Say("cmdtime", user, user.Colour, user.Name, cmdtime));
                return;
            }

            if (!lost_approve.ContainsKey(user.ID)) // Not typed at least once
            {
                rcon.Send(Language.Say("validate", user, DateTime.Now.AddSeconds(30)));
                lost_approve.Add(user.ID, DateTime.Now.AddSeconds(30));
                return;
            }

            DateTime when = lost_approve[user.ID];
            if (DateTime.Now < when) // Not yet...
            {
                rcon.Send(Language.Say("wait", user, when - DateTime.Now));
            }
            else if (DateTime.Now > when.AddSeconds(30)) // Too late!
            {
                rcon.Send(Language.Say("late", user));
                lost_approve.Remove(user.ID);
            }
            else // Yup, 30 second window is hit!
            {
                bool messaged = false;
                if (user.Auth.Hierarchy == Auth.None.Hierarchy)
                    for (int i = 0; i < 3; i++)
                    {
                        Package tp = new Package(String.Format("teleport.topos \"{0}\" {1}", user.SafeName, Data.Instance.GetTeleport("tanks")));
                        tp.RegisterCallback((Package res) =>
                            {
                                if (res.Response == "command teleport.topos was executed")
                                    if (!messaged)
                                    {
                                        messaged = true;
                                        CommandTime(String.Format("lost-{0}", user.ID), DateTime.Now.AddMinutes(10), true);
                                        rcon.Send(Language.Say("rescued", user, user.Colour, user.Name));
                                    }
                            });
                        rcon.SendPackage(tp);
                        Thread.Sleep(30);
                    }
                else
                    for (int i = 0; i < 3; i++)
                    {
                        Package tp = new Package(String.Format("teleport.topos \"{0}\" {1}", user.SafeName, Data.Instance.GetTeleport("Big Radtown")));
                        tp.RegisterCallback((Package res) =>
                        {
                            if (res.Response == "command teleport.topos was executed")
                                if (!messaged)
                                {
                                    messaged = true;
                                    CommandTime(String.Format("lost-{0}", user.ID), DateTime.Now.AddHours(1), true);
                                    rcon.Send(Language.Say("rescued", user, user.Colour, user.Name));
                                }
                        });
                        rcon.SendPackage(tp);
                    }
                lost_approve.Remove(user.ID);
            }
        }

        public void cmd_quit(User user, string command, string parameter, string[] parameters)
        {
            if (user.Auth != Auth.None)
            {
                rcon.Send(Language.Say("noneonly", user, user.Colour));
                return;
            }

            rcon.Send("sleepers.on false");
            rcon.Send(String.Format("kick \"{0}\"", user.SafeName));
            rcon.Send("sleepers.on true");
            rcon.Send(Language.Say("nosleeper", user, user.Name));
        }

        private bool parachute_open = false;
        public void cmd_parachute(User user, string command, string parameter, string[] parameters)
        {
            TimeSpan cmdtime = CommandTime(String.Format("parachute-{0}", user.ID));
            if (cmdtime.TotalSeconds > 0)
            {
                rcon.Send(Language.Say("cmdtime", user, user.Colour, user.Name, cmdtime));
                return;
            }

            if (parachute_open)
            {
                rcon.Send(Language.Say("parachute-already", user));
                return;
            }

            parachute_open = true;

            CommandTime(String.Format("parachute-{0}", user.ID), DateTime.Now.AddHours(4), true);

            rcon.Send("falldamage.min_vel 10000");
            rcon.Send(Language.Say("parachute-open", user));
            Thread.Sleep(1000 * 7); // Wait 7 seconds
            rcon.Send("falldamage.min_vel 24");
            rcon.Send(Language.Say("parachute-closed", user));

            parachute_open = false;
        }

        public void cmd_who(User user, string command, string parameter, string[] parameters)
        {
            User target = user;
            if (!String.IsNullOrEmpty(parameter.Trim()))
            {
                List<User> targets = Data.Instance.GetClosestUserByName(parameter.Replace('*', '%'));
                if (targets == null)
                {
                    rcon.Send(Language.Say("nomatch", user, parameter));
                    return;
                }
                else if (targets.Count > 1)
                {
                    rcon.Send(Language.Say("manymatch", user, targets.Count, parameter));
                    return;
                }
                target = targets[0];
            }

            rcon.Send(Language.Say(target.Online ? "who_on" : "who_off", user, target.Colour, target.Name, target.Country, new TimeSpan(0, 0, target.Time), target.Auth));

            if (target.Member != 0)
            {
                Group? group = Data.Instance.GetGroupById(target.Member);
                if (group != null)
                    rcon.Send(Language.Say("who_group", user, target.Colour, target.Name, group.Value.Name));
            }

            if (!target.Online)
                rcon.Send(Language.Say("who_last", user, target.Colour, target.Name, new DateTime(target.Last).ToString("dd.MM.yyyy HH:mm:ss")));
        }

        public void cmd_count(User user, string command, string parameter, string[] parameters)
        {
            if (CommandTime("count-global", DateTime.Now.AddSeconds(10), true).TotalSeconds > 0)
                return;

            Package count = new Package("status");
            count.RegisterCallback((Package package) =>
            {
                Match match = Regex.Match(package.Response, Program.validations["playercount"]);
                int amount = int.Parse(match.Groups[1].Value);
                if (amount < 2)
                {
                    rcon.Send(Language.Say("alone", user));
                    return;
                }
                rcon.Send(Language.Say("count", user, match.Groups[1].Value));
            });
            rcon.SendPackage(count);
        }

        public void cmd_time(User user, string command, string parameter, string[] parameters)
        {
            Package time = new Package("env.time");
            time.RegisterCallback((Package result) =>
                {
                    Match match = Regex.Match(result.Response, Program.validations["time"]);
                    rcon.Send(Language.Say("time", user, DateTime.Now.ToString("HH:mm:ss"), new DateTime(1991, 1, 1).AddHours(double.Parse(match.Groups[1].Value)).ToString("HH:mm:ss")));
                });
            rcon.SendPackage(time);
        }

        public void cmd_auth(User user, string command, string parameter, string[] parameters)
        {
            if (Auth.Rights.ContainsValue(user.Auth))
            {
                user.Auth = Auth.None;
                user.Save();
                rcon.Send(Language.Say("deauth", user));
                return;
            }

            if (!Auth.Rights.ContainsKey(user.ID))
            {
                rcon.Send(Language.Say("noauth", user));
                return;
            }

            user.Auth = Auth.Rights[user.ID];
            user.Save();
            rcon.Send(Language.Say("auth", user, user.Name, user.Auth.ToString()));
        }

        public static void warn(User target, Rcon rcon)
        {
            if (target.Warn >= 3)
            {
                rcon.Send(Language.Say("warn-4", target, target.Colour, target.Name));
                rcon.Send(String.Format("banid {0} \"Banned after 3 warnings\"", target.ID));
                Thread.Sleep(2000);
                rcon.Send(String.Format("kick \"{0}\"", target.SafeName));
            }
            else if (target.Warn >= 2)
            {
                // Kick
                rcon.Send(Language.Say("warn-3", target, target.Colour, target.Name));
                Thread.Sleep(2000);
                rcon.Send(String.Format("kick \"{0}\"", target.SafeName));
            }
            else
                rcon.Send(Language.Say("warn-1", target, target.Colour, target.Name, target.Warn + 1));

            target.Warn++;
            target.Save();
        }

        public void cmd_warn(User user, string command, string parameter, string[] parameters)
        {
            if (user.Auth.Hierarchy < Auth.Mod.Hierarchy)
            {
                rcon.Send(Language.Say("noperm", user));
                return;
            }

            User target = user;
            if (!String.IsNullOrEmpty(parameter))
            {
                List<User> users = Data.Instance.GetClosestUserByName(String.Format("%{0}%", parameter));
                if (users == null)
                {
                    rcon.Send(Language.Say("nomatch", user, parameter));
                    return;
                }

                if (users.Count > 1)
                {
                    rcon.Send(Language.Say("manymatch", user, parameter));
                    return;
                }

                target = users[0];
            }

            if (target.ID == user.ID)
            {
                rcon.Send(Language.Say("nomatch", user, parameter));
                return;
            }

            Commands.warn(target, rcon);
        }

        public void cmd_unwarn(User user, string command, string parameter, string[] parameters)
        {
            if (user.Auth.Hierarchy < Auth.Mod.Hierarchy)
            {
                rcon.Send(Language.Say("noperm", user));
                return;
            }

            User target = user;
            if (!String.IsNullOrEmpty(parameter))
            {
                List<User> users = Data.Instance.GetClosestUserByName(String.Format("%{0}%", parameter));
                if (users == null)
                {
                    rcon.Send(Language.Say("nomatch", user, parameter));
                    return;
                }

                if (users.Count > 1)
                {
                    rcon.Send(Language.Say("manymatch", user, parameter));
                    return;
                }

                target = users[0];
            }

            if (target.ID == user.ID)
            {
                rcon.Send(Language.Say("nomatch", user, parameter));
                return;
            }

            if (target.Warn > 0)
            {
                target.Warn--;
                target.Save();
                rcon.Send(Language.Say("unwarn", user, user.Colour, user.Name, user.Warn));
                return;
            }
        }

        public void cmd_kit(User user, string command, string parameter, string[] parameters)
        {
            if (new Auth[] { Auth.Admin, Auth.ChiefAdmin, Auth.Mod, Auth.SubMod }.Contains(user.Auth))
            {
                if (parameters.Length > 0)
                {
                    List<string> kit = Data.Instance.GetKit(parameters[0].ToLower(), user.Auth);
                    if (kit == null)
                    {
                        rcon.Send(Language.Say("nokit", user, parameters[0]));
                        return;
                    }
                    if (kit.Count == 0)
                    {
                        rcon.Send(Language.Say("noperm", user, parameters[0]));
                        return;
                    }
                    foreach (string item in kit)
                    {
                        int amount = 1;
                        if (parameters.Length > 1)
                            int.TryParse(parameters[1], out amount);

                        rcon.Send(String.Format(item, user.SafeName, amount));
                    }
                    rcon.Send(Language.Say("authkit", user, user.Colour, user.Name, parameters[0][0].ToString().ToUpper() + parameters[0].Substring(1).ToLower()));
                    return;
                }
                rcon.Send(Language.Say("whichkit", user));
                return;
            }

            TimeSpan time = CommandTime(String.Format("kit-{0}", user.ID));
            if (time.TotalSeconds > 0)
            {
                rcon.Send(Language.Say("cmdtime", user, user.Colour, user.Name, time));
                return;
            }

            if (user.Auth.Hierarchy != Auth.None.Hierarchy)
            {
                rcon.Send(String.Format("inv.giveplayer \"{0}\" \"Small Water Bottle\"", user.SafeName));
                rcon.Send(String.Format("inv.giveplayer \"{0}\" \"Handmade Lockpick\"", user.SafeName));
                rcon.Send(String.Format("inv.giveplayer \"{0}\" \"Rock\" 36", user.SafeName));
                CommandTime(String.Format("kit-{0}", user.ID), DateTime.Now.AddHours(1), true);
            }
            else
            {
                rcon.Send(String.Format("inv.giveplayer \"{0}\" \"Rock\"", user.SafeName));
                rcon.Send(String.Format("inv.giveplayer \"{0}\" \"Torch\"", user.SafeName));
                rcon.Send(String.Format("inv.giveplayer \"{0}\" \"Bandage\" 2", user.SafeName));
                if (CommandTime(String.Format("kit-aid-{0}", user.ID)).TotalSeconds == 0)
                {
                    rcon.Send(String.Format("inv.giveplayer \"{0}\" \"Small Medkit\"", user.SafeName));
                    rcon.Send(String.Format("inv.giveplayer \"{0}\" \"Chocolate Bar\"", user.SafeName));
                    CommandTime(String.Format("kit-aid-{0}", user.ID), DateTime.Now.AddMinutes(5), true);
                }
                CommandTime(String.Format("kit-{0}", user.ID), DateTime.Now.AddSeconds(20), true);
            }
            rcon.Send(Language.Say("kit", user, user.Colour, user.Name));
        }

        public void cmd_teleport_request(User user, string command, string parameter, string[] parameters)
        {
            if (user.Auth.Hierarchy != Auth.None.Hierarchy)
            {
                rcon.Send(Language.Say("noneonly", user, user.Colour));
                return;
            }

            TimeSpan cmdtime = CommandTime(String.Format("tpa-{0}", user.ID));
            if (cmdtime.TotalSeconds > 0)
            {
                rcon.Send(Language.Say("cmdtime", user, user.Colour, user.Name, cmdtime));
                return;
            }

            if (String.IsNullOrEmpty(parameter))
            {
                rcon.Send(Language.Say("tprhow", user));
                return;
            }

            List<User> targets = Data.Instance.GetClosestUserByName(parameter);
            if (targets == null)
            {
                rcon.Send(Language.Say("nomatch", user, parameter));
                return;
            }

            if (targets.Count > 1)
            {
                targets = targets.Where(match => match.Online).ToList();
                if (targets.Count == 0)
                {
                    rcon.Send(Language.Say("noonline", user, parameter));
                    return;
                }
                else if (targets.Count > 1)
                {
                    rcon.Send(Language.Say("manymatch", user, targets.Count, parameter));
                    return;
                }
            }

            User target = targets[0];
            if (!target.Online)
            {
                rcon.Send(Language.Say("offline", user, target.Colour, target.Name));
                return;
            }

            if (teleport_ids.ContainsKey(target.ID))
            {
                if (teleport_ids[target.ID] != user.ID && teleport_approve.ContainsKey(target.ID))
                    teleport_approve.Remove(target.ID);

                teleport_ids.Remove(target.ID);
            }

            teleport_ids.Add(target.ID, user.ID);
            rcon.Send(Language.Say("tprrequest", user, target.Colour, target.Name, user.Colour, user.Name));

            if (!teleport_approve.ContainsKey(target.ID))
                new Task(() =>
                        {
                            Thread.Sleep(1000 * 30);
                            if (!teleport_approve.ContainsKey(target.ID))
                            {
                                rcon.Send(Language.Say("late", user, user.Colour, user.Name));
                                teleport_approve.Remove(user.ID);
                                teleport_ids.Remove(target.ID);
                            }
                        }).Start();
        }

        private Dictionary<string, string> teleport_ids = new Dictionary<string, string>();
        private Dictionary<string, DateTime> teleport_approve = new Dictionary<string, DateTime>();
        public void cmd_teleport_accept(User user, string command, string parameter, string[] parameters)
        {
            if (!teleport_ids.ContainsKey(user.ID))
            {
                rcon.Send(Language.Say("tpanobody", user));
                return;
            }

            if (!teleport_approve.ContainsKey(user.ID)) // Not typed at least once
            {
                rcon.Send(Language.Say("validate", user, DateTime.Now.AddSeconds(30)));
                teleport_approve.Add(user.ID, DateTime.Now.AddSeconds(30));
                return;
            }

            DateTime when = teleport_approve[user.ID];
            if (DateTime.Now < when) // Not yet...
            {
                rcon.Send(Language.Say("wait", user, when - DateTime.Now));
            }
            else if (DateTime.Now > when.AddSeconds(30)) // Too late!
            {
                rcon.Send(Language.Say("late", user));
                teleport_approve.Remove(user.ID);
                teleport_ids.Remove(user.ID);
            }
            else // Yup, 30 second window is hit!
            {
                User from = Data.Instance.GetUserById(teleport_ids[user.ID]);

                bool messaged = false;

                for (int i = 0; i < 3; i++)
                {
                    Package tp = new Package(String.Format("teleport.toplayer \"{0}\" \"{1}\"", from.SafeName, user.SafeName));
                    tp.RegisterCallback((Package res) =>
                        {
                            if (res.Response.StartsWith("moved 1 player(s) named"))
                                if (!messaged)
                                {
                                    messaged = true;
                                    rcon.Send(Language.Say("tpa", user, from.Colour, from.Name, user.Colour, user.Name));
                                    CommandTime(String.Format("tpa-{0}", from.ID), DateTime.Now.AddDays(1), true);
                                }
                        });
                    rcon.SendPackage(tp);
                }
                teleport_approve.Remove(user.ID);
                teleport_ids.Remove(user.ID);
            }
        }

        public void cmd_log(User user, string command, string parameter, string[] parameters)
        {
            if (CommandTime("log-global", DateTime.Now.AddSeconds(10), true).TotalSeconds > 0)
                return;

            int amount = 6;
            int.TryParse(parameter, out amount);
            List<ChatLog> chatlog = Data.Instance.ReadChatLines(amount > 6 ? 6 : amount < 1 ? 6 : amount);
            foreach (ChatLog log in chatlog)
                rcon.Send(Language.Say("chatlog", log.User.Colour, log.User.Name, log.Text));
        }

        public void cmd_watch(User user, string command, string parameter, string[] parameters)
        {
            if (user.Auth.Hierarchy < Auth.SubMod.Hierarchy)
            {
                rcon.Send(Language.Say("noperm"));
                return;
            }

            if (String.IsNullOrEmpty(parameter))
            {
                rcon.Send(Language.Say("watchwho", user));
                return;
            }

            List<User> find = Data.Instance.GetClosestUserByName(parameter);
            if (find == null)
            {
                rcon.Send(Language.Say("nomatch", user, parameter));
                return;
            }

            if (find.Count > 1)
            {
                find = find.Where(match => match.Online).ToList();
                if (find.Count == 0)
                {
                    rcon.Send(Language.Say("noonline", user, parameter));
                    return;
                }
                else if (find.Count > 1)
                {
                    rcon.Send(Language.Say("manymatch", user, find.Count, parameter));
                    return;
                }
            }

            User found = find[0];
            found.Watched = true;
            found.Save();
            rcon.Send(Language.Say("watched", found, found.Colour, found.Name));
        }

        public void cmd_unwatch(User user, string command, string parameter, string[] parameters)
        {
            if (user.Auth.Hierarchy < Auth.SubMod.Hierarchy)
            {
                rcon.Send(Language.Say("noperm"));
                return;
            }

            if (String.IsNullOrEmpty(parameter))
            {
                rcon.Send(Language.Say("watchwho", user));
                return;
            }

            List<User> find = Data.Instance.GetClosestUserByName(parameter);
            if (find == null)
            {
                rcon.Send(Language.Say("nomatch", user, parameter));
                return;
            }

            if (find.Count > 1)
            {
                find = find.Where(match => match.Online).ToList();
                if (find.Count == 0)
                {
                    rcon.Send(Language.Say("noonline", user, parameter));
                    return;
                }
                else if (find.Count > 1)
                {
                    rcon.Send(Language.Say("manymatch", user, find.Count, parameter));
                    return;
                }
            }

            User found = find[0];
            if (!found.Watched)
            {
                rcon.Send(Language.Say("nowatch", found, found.Colour, found.Name));
                return;
            }

            found.Watched = false;
            found.Save();
            rcon.Send(Language.Say("unwatched", found, found.Colour, found.Name));
        }

        public void cmd_teleport(User user, string command, string parameter, string[] parameters)
        {
            if (user.Auth.Hierarchy < Auth.SubMod.Hierarchy)
            {
                rcon.Send(Language.Say("noperm"));
                return;
            }

            if (String.IsNullOrEmpty(parameter))
            {
                rcon.Send(Language.Say("tpwhere", user));
                return;
            }

            string target = Data.Instance.GetTeleport(String.Format("%{0}%", parameter));
            if (String.IsNullOrEmpty(target))
            {
                List<User> find = Data.Instance.GetClosestUserByName(parameter);
                if (find == null)
                {
                    rcon.Send(Language.Say("nomatch", user, parameter));
                    return;
                }

                if (find.Count > 1)
                {
                    find = find.Where(match => match.Online).ToList();
                    if (find.Count == 0)
                    {
                        rcon.Send(Language.Say("noonline", user, parameter));
                        return;
                    }
                    else if (find.Count > 1)
                    {
                        rcon.Send(Language.Say("manymatch", user, find.Count, parameter));
                        return;
                    }
                }

                User found = find[0];
                if (!found.Online)
                {
                    rcon.Send(Language.Say("offline", user, found.Colour, found.Name));
                    return;
                }

                bool messaged = false;

                for (int i = 0; i < 3; i++)
                {
                    Package tp = new Package(String.Format("teleport.toplayer \"{0}\" \"{1}\"", user.SafeName, found.SafeName));
                    tp.RegisterCallback((Package res) =>
                        {
                            if (res.Response.StartsWith("moved 1 player(s) named"))
                                if (!messaged)
                                {
                                    messaged = true;
                                    rcon.Send(Language.Say("tpdone", user, user.Colour, user.Name));
                                }
                        });
                    rcon.SendPackage(tp);
                }
            }
            else
            {
                bool messaged = false;
                for (int i = 0; i < 3; i++)
                {
                    Package tp = new Package(String.Format("teleport.topos \"{0}\" {1}", user.SafeName, target));
                    tp.RegisterCallback((Package res) =>
                    {
                        if (res.Response == "command teleport.topos was executed")
                            if (!messaged)
                            {
                                messaged = true;
                                rcon.Send(Language.Say("tpdone", user, user.Colour, user.Name));
                            }
                    });
                    rcon.SendPackage(tp);
                }
            }
        }

        public void cmd_authed(User user, string command, string parameter, string[] parameters)
        {
            List<User> onauth = new List<User>();
            List<User> offauth = new List<User>();

            foreach (string authid in Auth.Rights.Keys)
            {
                User authed = Data.Instance.GetUserById(authid);
                if (authed != null && authed.Online && authed.Auth.Hierarchy >= Auth.SubMod.Hierarchy)
                    onauth.Add(authed);
                else if (authed != null && authed.Online)
                    offauth.Add(authed);
            }

            if (onauth.Count > 0)
            {
                rcon.Send(Language.Say("onauth", user, onauth.Count));
                List<string> send = Language.List(onauth);
                foreach (string s in send)
                    rcon.Send(String.Format("say \"{0}\"", s));

                return;
            }

            if (offauth.Count > 0)
            {
                rcon.Send(Language.Say("offauth", user, offauth.Count));
                List<string> send = Language.List(offauth);
                foreach (string s in send)
                    rcon.Send(String.Format("say \"{0}\"", s));

                return;
            }

            rcon.Send(Language.Say("nobodyauth", user));
        }

        private Dictionary<string, DateTime> towns_approve = new Dictionary<string, DateTime>();
        public void cmd_towns(User user, string command, string parameter, string[] parameters)
        {
            if (user.Auth.Hierarchy == Auth.None.Hierarchy)
            {
                rcon.Send(Language.Say("viponly"));
                return;
            }

            TimeSpan cmdtime = CommandTime(String.Format("towns-{0}", user.ID));
            if (cmdtime.TotalSeconds > 0) // Restriction
            {
                rcon.Send(Language.Say("cmdtime", user, user.Colour, user.Name, cmdtime));
                return;
            }

            if (command != "pvp" && String.IsNullOrEmpty(parameter))
            {
                rcon.Send(Language.Say("travel", user));
                List<string> towns = Data.Instance.GetTeleportNamesByAccessRights(user.Auth);
                foreach (string dest in towns.Where(match => match == "Mediterrasurgit" || match == "Orientem Maris"))
                    rcon.Send(String.Format("say \"[color#BBBBBB]* {0}\"", dest));

                return;
            }

            string destination = null;
            if (command == "pvp")
                destination = Data.Instance.GetTeleport(String.Format("pvp-{0}", new Random().Next(1, 4)));
            else
                destination = Data.Instance.GetTeleport(String.Format("%{0}%", parameter), user.Auth);

            if (destination == null)
            {
                rcon.Send(Language.Say("notravel", user, parameter));
                return;
            }

            if (!towns_approve.ContainsKey(user.ID)) // Not typed at least once
            {
                rcon.Send(Language.Say("validate", user, DateTime.Now.AddSeconds(30)));
                towns_approve.Add(user.ID, DateTime.Now.AddSeconds(30));
                return;
            }

            DateTime when = towns_approve[user.ID];
            if (DateTime.Now < when) // Not yet...
            {
                rcon.Send(Language.Say("wait", user, when - DateTime.Now));
            }
            else if (DateTime.Now > when.AddSeconds(30)) // Too late!
            {
                rcon.Send(Language.Say("late", user));
                towns_approve.Remove(user.ID);
            }
            else // Yup, 30 second window is hit!
            {
                bool messaged = false;

                for (int i = 0; i < 3; i++)
                {
                    Package tp = new Package(String.Format("teleport.topos \"{0}\" {1}", user.SafeName, destination));
                    tp.RegisterCallback((Package res) =>
                        {
                            if (res.Response == "command teleport.topos was executed")
                                if (!messaged)
                                {
                                    messaged = true;
                                    rcon.Send(Language.Say("traveled", user, user.Colour, user.Name));
                                    CommandTime(String.Format("lost-{0}", user.ID), DateTime.Now.AddMinutes(10), true);
                                    CommandTime(String.Format("towns-{0}", user.ID), DateTime.Now.AddMinutes(50), true);
                                }
                        });
                    rcon.SendPackage(tp);
                }

                towns_approve.Remove(user.ID);
            }
        }

        public void cmd_ping(User user, string command, string parameter, string[] parameters)
        {
            if (CommandTime(String.Format("ping-{0}", user.ID), DateTime.Now.AddSeconds(10), true).TotalSeconds > 0)
                return;

            User target = user;
            if (!String.IsNullOrEmpty(parameter))
            {
                List<User> targets = Data.Instance.GetClosestUserByName(parameter);
                if (targets == null)
                {
                    rcon.Send(Language.Say("nomatch", user, parameter));
                    return;
                }

                if (targets.Count > 1)
                {
                    targets = targets.Where(match => match.Online).ToList();
                    if (targets.Count == 0)
                    {
                        rcon.Send(Language.Say("noonline", user, parameter));
                        return;
                    }
                    else if (targets.Count > 1)
                    {
                        rcon.Send(Language.Say("manymatch", user, targets.Count, parameter));
                        return;
                    }
                }

                target = targets[0];
            }

            Package count = new Package("status");
            count.RegisterCallback((Package package) =>
            {
                MatchCollection matches = Regex.Matches(package.Response, Program.validations["user-lines"]);
                foreach (Match match in matches)
                {
                    if (match.Groups[1].Value == target.ID)
                        rcon.Send(Language.Say("ping", user, target.Colour, target.Name, match.Groups[3].Value));
                }
            });
            rcon.SendPackage(count);
        }

        public void cmd_location(User user, string command, string parameter, string[] parameters)
        {
            if (CommandTime("location-global", DateTime.Now.AddSeconds(10), true).TotalSeconds > 0)
                return;
            Location location = Data.Instance.GetLocation(user.ID);
            rcon.Send(Language.Say("coordinates", location.X, location.Y, location.Z));
        }

        public void cmd_report(User user, string command, string parameter, string[] parameters)
        {
            if (String.IsNullOrEmpty(parameter))
            {
                rcon.Send(Language.Say("reporthow", user));
                return;
            }

            rcon.Send(Language.Say("reported", user, user.Colour, user.Name));
            Data.Instance.WriteReport(user, parameter);
        }

        public void cmd_karma(User user, string command, string parameter, string[] parameters)
        {
            User target = user;
            if (!String.IsNullOrEmpty(parameter))
            {
                List<User> users = Data.Instance.GetClosestUserByName(String.Format("%{0}%", parameter));
                if (users == null)
                {
                    rcon.Send(Language.Say("nomatch", user, parameter));
                    return;
                }

                if (users.Count > 1)
                {
                    rcon.Send(Language.Say("manymatch", user, parameter));
                    return;
                }

                target = users[0];
            }

            if (target.Karma >= 10)
                rcon.Send(Language.Say("karma-good", user, target.Colour, target.Name, target.Karma));
            else if (target.Karma <= -10)
                rcon.Send(Language.Say("karma-bad", user, target.Colour, target.Name, target.Karma));
            else
                rcon.Send(Language.Say("karma-neutral", user, target.Colour, target.Name, target.Karma));
        }

        public void cmd_karma_good(User user, string command, string parameter, string[] parameters)
        {
            if (String.IsNullOrEmpty(parameter))
            {
                rcon.Send(Language.Say("karmahow", user));
                return;
            }

            List<User> users = Data.Instance.GetClosestUserByName(String.Format("%{0}%", parameter));
            if (users == null)
            {
                rcon.Send(Language.Say("nomatch", user, parameter));
                return;
            }

            if (users.Count > 1)
            {
                rcon.Send(Language.Say("manymatch", user, parameter));
                return;
            }

            User target = users[0];
            if (target.ID == user.ID)
            {
                rcon.Send(Language.Say("selfkarma", user));
                return;
            }

            Group? group = null;

            if (user.Member != 0)
            {
                group = Data.Instance.GetGroupById(user.Member);
                if (group != null)
                {
                    users = users.Where(match => group.Value.Members.Count(submatch => submatch.ID == match.ID) > 0).ToList();

                    if (users.Count != 0)
                    {
                        rcon.Send(Language.Say("ismember", user, parameter));
                        return;
                    }

                    TimeSpan cmdtime = CommandTime(String.Format("karma-{0}-{1}", group.Value.Name, target.ID));
                    if (cmdtime.TotalSeconds > 0) // Restriction
                    {
                        rcon.Send(Language.Say("cmdtime_gk", user, target.Colour, target.Name, cmdtime));
                        return;
                    }
                }
            }
            else
            {
                TimeSpan cmdtime = CommandTime(String.Format("karma-{0}", user.ID));
                if (cmdtime.TotalSeconds > 0) // Restriction
                {
                    rcon.Send(Language.Say("cmdtime", user, user.Colour, user.Name, cmdtime));
                    return;
                }
            }


            if (group != null)
                CommandTime(String.Format("karma-{0}-{1}", group.Value.Name, target.ID), DateTime.Now.AddHours(4), true);
            CommandTime(String.Format("karma-{0}", user.ID), DateTime.Now.AddHours(4), true);

            target.Karma++;
            target.Save();
            cmd_karma(user, "karma", target.Name, null);
        }

        public void cmd_karma_bad(User user, string command, string parameter, string[] parameters)
        {
            if (String.IsNullOrEmpty(parameter))
            {
                rcon.Send(Language.Say("karmahow", user));
                return;
            }

            List<User> users = Data.Instance.GetClosestUserByName(String.Format("%{0}%", parameter));
            if (users == null)
            {
                rcon.Send(Language.Say("nomatch", user, parameter));
                return;
            }

            if (users.Count > 1)
            {
                rcon.Send(Language.Say("manymatch", user, parameter));
                return;
            }

            User target = users[0];
            if (target.ID == user.ID)
            {
                rcon.Send(Language.Say("selfkarma", user));
                return;
            }
            Group? group = null;

            if (user.Member != 0)
            {
                group = Data.Instance.GetGroupById(user.Member);
                if (group != null)
                {
                    users = users.Where(match => group.Value.Members.Count(submatch => submatch.ID == match.ID) > 0).ToList();

                    if (users.Count != 0)
                    {
                        rcon.Send(Language.Say("ismember", user, parameter));
                        return;
                    }

                    TimeSpan cmdtime = CommandTime(String.Format("karma-{0}-{1}", group.Value.Name, target.ID));
                    if (cmdtime.TotalSeconds > 0) // Restriction
                    {
                        rcon.Send(Language.Say("cmdtime_gk", user, target.Colour, target.Name, cmdtime));
                        return;
                    }
                }
            }
            else
            {
                TimeSpan cmdtime = CommandTime(String.Format("karma-{0}", user.ID));
                if (cmdtime.TotalSeconds > 0) // Restriction
                {
                    rcon.Send(Language.Say("cmdtime", user, user.Colour, user.Name, cmdtime));
                    return;
                }
            }


            if (group != null)
                CommandTime(String.Format("karma-{0}-{1}", group.Value.Name, target.ID), DateTime.Now.AddHours(4), true);
            CommandTime(String.Format("karma-{0}", user.ID), DateTime.Now.AddHours(4), true);
            target.Karma--;
            target.Save();
            cmd_karma(user, "karma", target.Name, null);
        }

        /**
         * ############
         * G R O U P S
         * ############
         **/
        public void cmd_group(User user, string command, string parameter, string[] parameters)
        {
            if (String.IsNullOrEmpty(parameter))
            {
                // Show all active groups
                List<Group> online_groups = new List<Group>();
                List<User> online_users = Data.Instance.GetOnlineUsers();
                foreach (User online in online_users)
                {
                    if (online.Member != 0 && online_groups.Count(x => x.ID == online.Member) == 0)
                    {
                        Group? group = Data.Instance.GetGroupById(online.Member);
                        if (group != null)
                            online_groups.Add((Group)group);
                    }
                }
                rcon.Send(Language.Say("groups", user, online_groups.Count, Data.Instance.GetGroupCount()));
                foreach (string send in Language.List(online_groups))
                    rcon.Send(String.Format("say \"{0}\"", send));
            }
            else
            {
                List<Group> groups = Data.Instance.GetClosestGroupByName(String.Format("%{0}%", parameter));
                if (groups == null)
                {
                    rcon.Send(Language.Say("nogmatch", user, parameter));
                    return;
                }

                if (groups.Count > 1)
                {
                    rcon.Send(Language.Say("manygmatch", user, groups.Count, parameter));
                    return;
                }

                Group group = groups[0];
                rcon.Send(Language.Say("group_members", user, group.Name));
                foreach (string send in Language.List(Data.Instance.GetUsersByGroup(group.ID)))
                    rcon.Send(String.Format("say \"{0}\"", send));
                if (group.Alliance != 0)
                    rcon.Send(Language.Say("group_alliance", user, group.Name, Data.Instance.GetAllianceById(group.Alliance).Value.Name));
            }
        }

        public void cmd_group_create(User user, string command, string parameter, string[] parameters)
        {
            if (user.Member != 0)
            {
                rcon.Send(Language.Say("group_already", user));
                return;
            }

            if (String.IsNullOrEmpty(parameter))
            {
                rcon.Send(Language.Say("group_create_how", user));
                return;
            }

            if (Data.Instance.GetClosestGroupByName(parameter) != null)
            {
                rcon.Send(Language.Say("group_exist", user, parameter));
                return;
            }

            user.Member = Data.Instance.CreateGroup(user, parameter).ID;
            user.Save();
            rcon.Send(Language.Say("group_created", user, user.Colour, user.Name, parameter));
        }

        private Dictionary<string, int> group_invites = new Dictionary<string, int>();
        public void cmd_group_invite(User user, string command, string parameter, string[] parameters)
        {
            if (user.Member == 0)
            {
                rcon.Send(Language.Say("group_noadmin", user));
                return;
            }

            Group? group = Data.Instance.GetGroupById(user.Member);
            if (group == null || group.Value.Admin.ID != user.ID)
            {
                rcon.Send(Language.Say("group_noadmin", user));
                return;
            }

            if (String.IsNullOrEmpty(parameter))
            {
                rcon.Send(Language.Say("group_invite_how", user));
                return;
            }

            List<User> users = Data.Instance.GetClosestUserByName(String.Format("%{0}%", parameter));
            if (users == null)
            {
                rcon.Send(Language.Say("nomatch", user, parameter));
                return;
            }

            if (users.Count > 1)
            {
                rcon.Send(Language.Say("manymatch", user, parameter));
                return;
            }

            User target = users[0];

            if (target.Member != 0)
            {
                rcon.Send(Language.Say("group_already_specific", user, target.Colour, target.Name));
                return;
            }

            if (group_invites.ContainsKey(target.ID))
                group_invites.Remove(target.ID);
            group_invites.Add(target.ID, group.Value.ID);
            rcon.Send(Language.Say("group_invite", target, target.Colour, target.Name, user.Colour, user.Name, group.Value.Name));
        }

        public void cmd_group_accept(User user, string command, string parameter, string[] parameters)
        {
            if (!group_invites.ContainsKey(user.ID))
            {
                rcon.Send(Language.Say("group_noinvite", user));
                return;
            }

            user.Member = group_invites[user.ID];
            user.Save();
            rcon.Send(Language.Say("group_accepted", user, user.Colour, user.Name, Data.Instance.GetGroupById(group_invites[user.ID]).Value.Name));
            group_invites.Remove(user.ID);
        }

        public void cmd_group_kick(User user, string command, string parameter, string[] parameters)
        {
            if (user.Member == 0)
            {
                rcon.Send(Language.Say("group_noadmin", user));
                return;
            }

            Group? group = Data.Instance.GetGroupById(user.Member);
            if (group == null || group.Value.Admin.ID != user.ID)
            {
                rcon.Send(Language.Say("group_noadmin", user));
                return;
            }

            if (String.IsNullOrEmpty(parameter))
            {
                rcon.Send(Language.Say("group_kick_how", user));
                return;
            }

            List<User> users = Data.Instance.GetClosestUserByName(String.Format("%{0}%", parameter));
            if (users == null)
            {
                rcon.Send(Language.Say("nomatch", user, parameter));
                return;
            }

            if (users.Count > 1)
            {
                users = users.Where(match => group.Value.Members.Count(submatch => submatch.ID == match.ID) > 0).ToList();

                if (users.Count == 0)
                {
                    rcon.Send(Language.Say("nomember", user, parameter));
                    return;
                }

                if (users.Count > 1)
                {
                    rcon.Send(Language.Say("manymatch", user, parameter));
                    return;
                }
            }

            User target = users[0];

            if (target.ID == user.ID)
            {
                rcon.Send(Language.Say("gselfkick", user));
                return;
            }

            target.Member = 0;
            target.Save();
            rcon.Send(Language.Say("gkicked", target, target.Colour, target.Name, group.Value.Name));
        }

        public void cmd_group_quit(User user, string command, string parameter, string[] parameters)
        {
            if (user.Member == 0)
            {
                rcon.Send(Language.Say("nogroup", user));
                return;
            }

            Group? group = Data.Instance.GetGroupById(user.Member);
            if (group == null)
            {
                rcon.Send(Language.Say("nogroup", user));
                return;
            }

            user.Member = 0;
            user.Save();
            rcon.Send(Language.Say("gquit", user, user.Colour, user.Name, group.Value.Name));

            if (group.Value.Admin.ID == user.ID)
            {
                User nextadmin = group.Value.Members.Where(x => x.ID != user.ID).FirstOrDefault();
                if (nextadmin == null)
                    Data.Instance.DeleteGroup(group.Value.ID);
                else
                {
                    Group ng = (Group)group;
                    ng.Admin = nextadmin;
                    Data.Instance.SaveGroup(ng);
                }
            }
        }

        /**
         * ############
         * A L L I A N C E S
         * ############
         **/
        public void cmd_alliance(User user, string command, string parameter, string[] parameters)
        {
            if (String.IsNullOrEmpty(parameter))
            {
                // Show all active groups
                List<Alliance> online_alliances = new List<Alliance>();
                List<Group> online_groups = new List<Group>();
                List<User> online_users = Data.Instance.GetOnlineUsers();
                foreach (User online in online_users)
                {
                    if (online.Member != 0 && online_groups.Count(x => x.ID == online.Member) == 0)
                    {
                        Group? group = Data.Instance.GetGroupById(online.Member);
                        if (group != null)
                            online_groups.Add((Group)group);
                    }
                }
                foreach (Group group in online_groups)
                {
                    if (group.Alliance != 0 && online_alliances.Count(x => x.ID == group.Alliance) == 0)
                    {
                        Alliance? alliance = Data.Instance.GetAllianceById(group.Alliance);
                        if (alliance != null)
                            online_alliances.Add((Alliance)alliance);
                    }
                }
                rcon.Send(Language.Say("alliances", user, online_alliances.Count, Data.Instance.GetAllianceCount()));
                foreach (string send in Language.List(online_alliances))
                    rcon.Send(String.Format("say \"{0}\"", send));
            }
            else
            {
                List<Alliance> alliances = Data.Instance.GetClosestAllianceByName(String.Format("%{0}%", parameter));
                if (alliances == null)
                {
                    rcon.Send(Language.Say("noamatch", user, parameter));
                    return;
                }

                if (alliances.Count > 1)
                {
                    rcon.Send(Language.Say("manyamatch", user, parameter));
                    return;
                }

                Alliance alliance = alliances[0];
                rcon.Send(Language.Say("alliance_members", user, alliance.Name));
                foreach (string send in Language.List(alliance.Members))
                    rcon.Send(String.Format("say \"{0}\"", send));
            }
        }

        public void cmd_alliance_create(User user, string command, string parameter, string[] parameters)
        {
            if (user.Member == 0)
            {
                rcon.Send(Language.Say("group_noadmin", user));
                return;
            }

            Group? tgroup = Data.Instance.GetGroupById(user.Member);
            if (tgroup == null || tgroup.Value.Admin.ID != user.ID)
            {
                rcon.Send(Language.Say("group_noadmin", user));
                return;
            }

            Group group = (Group)tgroup;

            if (group.Alliance != 0)
            {
                rcon.Send(Language.Say("alliance_already", user));
                return;
            }

            if (String.IsNullOrEmpty(parameter))
            {
                rcon.Send(Language.Say("alliance_create_how", user));
                return;
            }

            if (Data.Instance.GetClosestAllianceByName(parameter) != null)
            {
                rcon.Send(Language.Say("alliance_exist", user, parameter));
                return;
            }

            group.Alliance = Data.Instance.CreateAlliance(user, parameter).ID;
            Data.Instance.SaveGroup(group);
            rcon.Send(Language.Say("alliance_created", user, user.Colour, user.Name, parameter));
        }

        private Dictionary<int, int> alliance_invites = new Dictionary<int, int>();
        public void cmd_alliance_invite(User user, string command, string parameter, string[] parameters)
        {
            if (user.Member == 0)
            {
                rcon.Send(Language.Say("group_noadmin", user));
                return;
            }

            Group? group = Data.Instance.GetGroupById(user.Member);
            if (group == null || group.Value.Admin.ID != user.ID)
            {
                rcon.Send(Language.Say("group_noadmin", user));
                return;
            }

            if (group.Value.Alliance == 0)
            {
                rcon.Send(Language.Say("alliance_noadmin", user));
                return;
            }

            Alliance? alliance = Data.Instance.GetAllianceById(group.Value.Alliance);
            if (alliance == null || alliance.Value.Admin.ID != user.ID)
            {
                rcon.Send(Language.Say("alliance_noadmin", user));
                return;
            }

            if (String.IsNullOrEmpty(parameter))
            {
                rcon.Send(Language.Say("alliance_invite_how", user));
                return;
            }

            List<Group> groups = Data.Instance.GetClosestGroupByName(String.Format("%{0}%", parameter));
            if (groups == null)
            {
                rcon.Send(Language.Say("nogmatch", user, parameter));
                return;
            }

            if (groups.Count > 1)
            {
                rcon.Send(Language.Say("manygmatch", user, parameter));
                return;
            }

            Group target = groups[0];

            if (target.Alliance != 0)
            {
                rcon.Send(Language.Say("alliance_already_specific", user, target.Name));
                return;
            }

            if (alliance_invites.ContainsKey(target.ID))
                alliance_invites.Remove(target.ID);
            alliance_invites.Add(target.ID, alliance.Value.ID);
            rcon.Send(Language.Say("alliance_invite", target.Admin, target.Admin.Colour, target.Admin.Name, user.Colour, user.Name, alliance.Value.Name));
        }

        public void cmd_alliance_accept(User user, string command, string parameter, string[] parameters)
        {
            if (user.Member == 0)
            {
                rcon.Send(Language.Say("group_noadmin", user));
                return;
            }

            Group? tgroup = Data.Instance.GetGroupById(user.Member);
            if (tgroup == null || tgroup.Value.Admin.ID != user.ID)
            {
                rcon.Send(Language.Say("group_noadmin", user));
                return;
            }
            Group group = (Group)tgroup;

            if (!alliance_invites.ContainsKey(group.ID))
            {
                rcon.Send(Language.Say("alliance_noinvite", user));
                return;
            }

            group.Alliance = alliance_invites[group.ID];
            Data.Instance.SaveGroup(group);
            rcon.Send(Language.Say("alliance_accepted", user, group.Name, Data.Instance.GetAllianceById(alliance_invites[group.ID]).Value.Name));
            alliance_invites.Remove(group.ID);
        }

        public void cmd_alliance_kick(User user, string command, string parameter, string[] parameters)
        {
            if (user.Member == 0)
            {
                rcon.Send(Language.Say("group_noadmin", user));
                return;
            }

            Group? tgroup = Data.Instance.GetGroupById(user.Member);
            if (tgroup == null || tgroup.Value.Admin.ID != user.ID)
            {
                rcon.Send(Language.Say("group_noadmin", user));
                return;
            }
            Group group = (Group)tgroup;

            if (group.Admin.ID != user.ID)
            {
                rcon.Send(Language.Say("alliance_noadmin", user));
                return;
            }

            if (group.Alliance == 0)
            {
                rcon.Send(Language.Say("alliance_noadmin", user));
                return;
            }

            if (String.IsNullOrEmpty(parameter))
            {
                rcon.Send(Language.Say("alliance_kick_how", user));
                return;
            }

            List<Group> groups = Data.Instance.GetClosestGroupByName(String.Format("%{0}%", parameter));
            if (groups == null)
            {
                rcon.Send(Language.Say("nogmatch", user, parameter));
                return;
            }

            Alliance? talliance = Data.Instance.GetAllianceById(group.Alliance);
            if (talliance == null || talliance.Value.Admin.ID != user.ID)
            {
                rcon.Send(Language.Say("alliance_noadmin", user));
                return;
            }
            Alliance alliance = (Alliance)talliance;

            if (groups.Count > 1)
            {
                groups = groups.Where(match => alliance.Members.Count(submatch => submatch.ID == match.ID) > 0).ToList();

                if (groups.Count == 0)
                {
                    rcon.Send(Language.Say("noamember", user, parameter));
                    return;
                }

                if (groups.Count > 1)
                {
                    rcon.Send(Language.Say("manygmatch", user, parameter));
                    return;
                }
            }

            Group target = groups[0];

            if (target.ID == group.ID)
            {
                rcon.Send(Language.Say("aselfkick", user));
                return;
            }

            target.Alliance = 0;
            Data.Instance.SaveGroup(target);
            rcon.Send(Language.Say("akicked", target.Admin, target.Name, alliance.Name));
        }

        public void cmd_alliance_quit(User user, string command, string parameter, string[] parameters)
        {
            if (user.Member == 0)
            {
                rcon.Send(Language.Say("nogroup", user));
                return;
            }

            Group? tgroup = Data.Instance.GetGroupById(user.Member);
            if (tgroup == null)
            {
                rcon.Send(Language.Say("nogroup", user));
                return;
            }
            Group group = (Group)tgroup;

            if (group.Alliance == 0)
            {
                rcon.Send(Language.Say("noalliance", user));
                return;
            }

            Alliance? talliance = Data.Instance.GetAllianceById(group.Alliance);
            if (talliance == null)
            {
                rcon.Send(Language.Say("noalliance", user));
                return;
            }
            Alliance alliance = (Alliance)talliance;

            group.Alliance = 0;
            Data.Instance.SaveGroup(group);
            rcon.Send(Language.Say("aquit", user, group.Name, alliance.Name));

            if (alliance.Admin != null && alliance.Admin.ID == user.ID)
            {
                Group? nextadmin = alliance.Members.Where(x => x.ID != group.ID).FirstOrDefault();
                if (nextadmin == null)
                    Data.Instance.DeleteAlliance(alliance.ID);
                else
                {
                    alliance.Admin = nextadmin.Value.Admin;
                    Data.Instance.SaveAlliance(alliance);
                }
            }

            if (alliance.Members.Count(match => match.ID != group.ID) == 0)
                Data.Instance.DeleteAlliance(alliance.ID);
        }

        public void cmd_test_ping(User user, string command, string parameter, string[] parameters)
        {
            rcon.Send("say \"Pong\"");
        }

        public void cmd_test_bans(User user, string command, string parameter, string[] parameters)
        {
            Package bans = new Package("banlistex");
            bans.RegisterCallback((Package p) =>
                {
                    Console.WriteLine(p.Response);
                    rcon.Send("say \"OK\"");
                });
            rcon.SendPackage(bans);
        }
    }
}
