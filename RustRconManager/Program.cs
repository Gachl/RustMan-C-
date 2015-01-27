using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RustRcon;
using System.Threading;
using System.Text.RegularExpressions;

namespace RustRconManager
{
    class Program
    {
        private static int fps = 30; // Idle speed

        private static List<string> whiteCountries = new List<string>()
        {
            "Iceland",

            "Norway",
            "Sweden",
            "Finland",

            "Denmark",
            "Germany",
            "Switzerland",
            "Austria",
            "Belgium",
            "Netherlands",
            "Liechtenstein",
            "Luxembourg",

            "United Kingdom",
            "Ireland",

            "France",
            "Italy",
            "San Marino",
            "Monaco",

            "Andorra",
            "Spain",
            "Portugal",

            "Estonia",
            "Lithuania",
            "Latvia",

            "Belarus",
            "Ukraine",
            "Moldovia",
            "Romania",
            "Bulgaria",
            "Macedonia",
            "Albania",
            "Montenegro",
            "Serbia",
            "Bosnia and Herzegovina",
            "Croatia",
            "Slovenia",
            "Slovakia",
            "Hungary",
            "Slowakia",
            "Czech Republic",
            "Poland",

            "Greece"
        };

        public static Dictionary<string, string> validations = new Dictionary<string, string>()
        {
            // Relevant
            {"command", "^\\[CHAT\\] \\\"(.*)\\\":\\\"[!?/]+(.*)\\\"$"},
            {"join", "^User Connected: (.*) \\((\\d{17})\\)$"},
            {"quit", "^User Disconnected: (.*)$"},
            {"violation", "^\\[T\\]\\[(\\d+)\\]\\[NetUser\\|(.*)\\|(\\d+)\\|(\\d+)\\] kicked for violation \\d+$"},
            {"save", "^.+Saved (\\d+) Object\\(s\\)\\. Took (\\d.?\\d*) seconds\\.$"},
            {"save2", "^Saving to '.*"},
            {"ticket", "^Kicking (.*) \\(\\d+\\) - their ticket was cancelled$"},
            {"entry", "^Denying entry to (\\d+) because they're already connected$"},
            {"user-lines", "(\\d{17}) +\\\"(.*)\\\" +(\\d+) +(\\d+)s +(\\d+\\.\\d+\\.\\d+\\.\\d+)"},
            {"chat", "^\\[CHAT\\] \\\"(.*)\\\":\\\"(.*)\\\"$"},
            {"suicide", "^(.*) has suicided$"},
            {"ips", "^\\[CHAT\\] \\\"(.*)\\\":\\\".*\\d+\\.\\d+\\.\\d+\\.\\d+.*\\\"$"},
            {"disposed", "^OnDestroy$"},

            // Ignore
            {"status", "^hostname: "},
            {"sayre", "^command global\\.say was executed$"},
            {"banlist", "^1 "},
            {"validator", "^Invalid command$"},
            {"globalsay", "^command global\\.say was executed$"},
            {"time", "^Current Time: (\\d+\\.?\\d*)$"},
            {"playercount", "players : (\\d+) \\(\\d+ max\\)"},
            {"items", "^Gave .*"},
            {"topos", "^command teleport.topos was executed$"},
            {"moved", "^moved \\d+ player\\(s\\) named .*$"}
        };

        static void Main(string[] args)
        {
            Rcon rcon = new Rcon("...", 0, "...");
            Language.init();

            Commands commands = new Commands(rcon);
            Cron cron = new Cron();

            int prev_save = 0;

            Task passives = new Task(() =>
                {
                    while (true)
                    {
                        Package package = rcon.ReadPackage();
                        if (package == null)
                        {
                            Thread.Sleep(1000 / fps);
                            continue;
                        }

                        // Handle passive traffic
                        new Task(() =>
                            {
                                try
                                {
                                    /*
                                    * IP Filter in Chat
                                    */
                                    if (Regex.IsMatch(package.Response, validations["ips"]))
                                    {
                                        Match match = Regex.Match(package.Response, validations["ips"]);
                                        User user = Data.Instance.GetUserByName(match.Groups[1].Value);
                                        Commands.warn(user, rcon);

                                        Data.Instance.HardLog(String.Format("Received IP in chat from user {0}, issuing warning.", user.Name));
                                    }
                                    /*
                                     * Commands
                                     */
                                    else if (Regex.IsMatch(package.Response, validations["command"]))
                                    {
                                        Match match = Regex.Match(package.Response.Replace("\\\"", "\""), validations["command"]);
                                        User user = User.GetUserByName(match.Groups[1].Value);
                                        if (user == null)
                                            return;

                                        string command = match.Groups[2].Value.Trim().ToLower();
                                        while (command.Contains(' '))
                                            command = command.Substring(0, command.IndexOf(' '));

                                        string parameter = match.Groups[2].Value.Trim().Substring(command.Length).Trim();
                                        string[] parameters = parameter.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                                        Data.Instance.WriteToIRC(new ChatLog() { Text = String.Format("/{0}", match.Groups[2].Value), User = user, Time = DateTime.Now });

                                        Data.Instance.HardLog(String.Format("Received command {0} from user {1}", command, user.Name));

                                        new Task(() => { commands.Run(user, command, parameter, parameters); }).Start();
                                    }

                                    /*
                                     * Join messages
                                     */
                                    else if (Regex.IsMatch(package.Response, validations["join"]))
                                    {
                                        Match match = Regex.Match(package.Response, validations["join"]);

                                        User userbyid = User.GetUserById(match.Groups[2].Value);
                                        User userbyname = User.GetUserByName(match.Groups[1].Value);

                                        Data.Instance.HardLog(String.Format("Detected join of {0} ({1})", match.Groups[1].Value, match.Groups[2].Value));

                                        // User already online
                                        if (userbyid != null && userbyid.Online)
                                        {
                                            Data.Instance.HardLog(String.Format("But {0} is already online, ignoring join.", userbyid.Name));
                                            return;
                                        }

                                        // Nick already in use?
                                        if (userbyid != null && userbyname != null && userbyid.ID != userbyname.ID)
                                        {
                                            Data.Instance.WriteViolation(userbyid, 900001);
                                            rcon.Send("sleepers.on false");
                                            rcon.Send(String.Format("kick \"{0}\"", userbyname.SafeName));
                                            rcon.Send(Language.Say("kick-name", null, userbyname.SafeName));
                                            rcon.Send("sleepers.on true");
                                            Data.Instance.WriteToIRC(new ChatLog() { Text = String.Format("Duplicate nick: {0}", userbyid.Name), User = new User("-1", "#KICK"), Time = DateTime.Now });
                                            Data.Instance.HardLog(String.Format("Duplicate name {0}, kicking both without sleeper", userbyname.Name));
                                            return;
                                        }

                                        // Nick changed?
                                        if (userbyid != null && userbyname == null)
                                        {
                                            Data.Instance.HardLog(String.Format("Nick changed from {0} to {1}", userbyid.Name, match.Groups[1].Value));
                                            rcon.Send(Language.Say("nick-change", userbyid, userbyid.Colour, userbyid.Name, match.Groups[1].Value));
                                            userbyid.Name = match.Groups[1].Value;
                                            userbyid.Save();
                                        }

                                        // No such user?
                                        if (userbyid == null && userbyname == null)
                                        {
                                            string id = match.Groups[2].Value;
                                            Package createUser = new Package("status");
                                            createUser.RegisterCallback((Package result) =>
                                                {
                                                    match = null;
                                                    MatchCollection matches = Regex.Matches(result.Response, validations["user-lines"]);
                                                    foreach (Match submatch in matches)
                                                    {
                                                        if (submatch.Groups[1].Value == id)
                                                        {
                                                            match = submatch;
                                                            break;
                                                        }
                                                    }

                                                    if (match == null)
                                                    {
                                                        Data.Instance.HardLog(String.Format("New user {0} does not show up in status, aborting", match.Groups[1].Value));
                                                        throw new Exception("New user joined but did not show up in status.");
                                                    }

                                                    userbyid = new User(match.Groups[1].Value, match.Groups[2].Value, match.Groups[4].Value, match.Groups[5].Value);

                                                    if (!whiteCountries.Contains(userbyid.Country))
                                                    {
                                                        Data.Instance.HardLog(String.Format("User {0} joined from {1} which is blacklisted", userbyid.Name, userbyid.Country));
                                                        rcon.Send(Language.Say("blacklist", userbyid, match.Groups[2].Value, userbyid.Country));
                                                        rcon.Send(String.Format("banid {0} \"Blacklisted country: {1}\" \"{2}\"", match.Groups[1].Value, userbyid.Country, match.Groups[2].Value));
                                                        rcon.Send(String.Format("kick \"{0}\"", match.Groups[2].Value));
                                                        Data.Instance.WriteToIRC(new ChatLog() { Text = String.Format("Backlisted: {0} ({1})", userbyid.Name, userbyid.Country), User = new User("-1", "#KICK"), Time = DateTime.Now });
                                                        return;
                                                    }

                                                    VACResult vac = VACCheck.Check(userbyid.ID);
                                                    if (vac.Count > 0 && vac.Days < 366)
                                                    {
                                                        Data.Instance.HardLog(String.Format("User {0} has {1} VACs in {2} days", userbyid.Name, vac.Count, vac.Days));
                                                        rcon.Send(Language.Say("vacd", userbyid, userbyid.Name, vac.Count, vac.Days));
                                                        rcon.Send(String.Format("banid {0} \"{1} VAC bans on record ({2} days)\" \"{3}\"", userbyid.ID, vac.Count, vac.Days, userbyid.SafeName));
                                                        rcon.Send(String.Format("kick \"{0}\"", userbyid.SafeName));
                                                        Data.Instance.WriteToIRC(new ChatLog() { Text = String.Format("VAC: {0} has {1} in {2} days", userbyid.Name, vac.Count, vac.Days), User = new User("-1", "#KICK"), Time = DateTime.Now });
                                                        return;
                                                    }
                                                    else if (vac.Count > 0)
                                                        rcon.Send(Language.Say("vacok", userbyid, userbyid.Colour, userbyid.Name, vac.Count, vac.Days));

                                                    /*VACResult cheatpunch = VACCheck.CheatPunchCheck(userbyid.ID);
                                                    if (cheatpunch.Count > 0 && cheatpunch.Days > 0)
                                                    {
                                                        rcon.Send(Language.Say("cpd", userbyid, userbyid.Name));

                                                        rcon.Send(String.Format("banid {0} \"CheatPunch'd\"", userbyid.ID));
                                                        rcon.Send(String.Format("kick \"{0}\"", userbyid.SafeName));
                                                        return;
                                                    }*/

                                                    userbyid.Online = true;
                                                    userbyid.Save();

                                                    rcon.Send(Language.Say("join", userbyid, userbyid.Colour, userbyid.Name, userbyid.Country));
                                                    rcon.Send(Language.Say("firstwelcome", userbyid, userbyid.Name));
                                                    rcon.Send(Language.Say("firstgreeting", userbyid));

                                                    Data.Instance.HardLog("New user creation successful.");

                                                    Data.Instance.WriteJoin(userbyid);
                                                    Data.Instance.WriteToIRC(new ChatLog() { Text = String.Format("{0} ({1}) (new)", userbyid.Name, userbyid.Country), User = new User("-1", "#JOIN"), Time = DateTime.Now });
                                                });

                                            rcon.SendPackage(createUser);
                                            return;
                                        }

                                        Data.Instance.HardLog("Old user activation successful.");
                                        userbyid.Online = true;
                                        userbyid.Save();
                                        Data.Instance.WriteJoin(userbyid);
                                        rcon.Send(Language.Say("join", userbyid, userbyid.Colour, userbyid.Name, userbyid.Country));
                                        rcon.Send(Language.Say("welcome", userbyid, userbyid.Colour, userbyid.Name));
                                        Data.Instance.WriteToIRC(new ChatLog() { Text = String.Format("{0} ({1})", userbyid.Name, userbyid.Country), User = new User("-1", "#JOIN"), Time = DateTime.Now });
                                    }

                                    /*
                                     * Quit messages
                                     */
                                    else if (Regex.IsMatch(package.Response, validations["quit"]))
                                    {
                                        Match match = Regex.Match(package.Response, validations["quit"]);

                                        Data.Instance.HardLog(String.Format("Detected disconnect of {0}", match.Groups[1].Value));

                                        User user = User.GetUserByName(match.Groups[1].Value);
                                        if (user != null && user.Online)
                                        {
                                            Package checkUserQuit = new Package("status");
                                            checkUserQuit.RegisterCallback((Package result) =>
                                                {
                                                    match = null;
                                                    MatchCollection matches = Regex.Matches(result.Response, validations["user-lines"]);
                                                    foreach (Match submatch in matches)
                                                    {
                                                        if (submatch.Groups[1].Value == user.ID)
                                                        {
                                                            match = submatch;
                                                            break;
                                                        }
                                                    }

                                                    if (match != null)
                                                    {
                                                        Data.Instance.HardLog("But user is still connected...");
                                                        return; // User still connected
                                                    }

                                                    // TODO: Scenario:
                                                    // User A and user B have the same nick, they have never been on the server
                                                    // A joins, gets the nick registered and alt tabs out of the game
                                                    // B joins, same nick is detected, kick is issued
                                                    // User Disconnected message for user B with nick of both A and B
                                                    // A is not in the status list, as he has tabbed out
                                                    // A is detected as having disconnected, breaking time counting and commands
                                                    user.Last = DateTime.Now.Ticks;
                                                    rcon.Send(Language.Say("quit", user, user.Colour, user.Name));
                                                    user.Online = false;
                                                    user.Save();
                                                    Data.Instance.WriteQuit(user);
                                                    Data.Instance.WriteToIRC(new ChatLog() { Text = user.Name, User = new User("-1", "#QUIT"), Time = DateTime.Now });
                                                    Data.Instance.HardLog("Successfully disconnected");
                                                });
                                            rcon.SendPackage(checkUserQuit);
                                        }
                                    }

                                    /*
                                     * Chat log
                                     */
                                    else if (Regex.IsMatch(package.Response, validations["chat"]))
                                    {
                                        Match match = Regex.Match(package.Response, validations["chat"]);
                                        User user = Data.Instance.GetUserByName(match.Groups[1].Value);
                                        Data.Instance.WriteChatLine(user, match.Groups[2].Value);
                                        Data.Instance.WriteToIRC(new ChatLog() { Text = match.Groups[2].Value, User = user, Time = DateTime.Now });
                                        if (match.Groups[2].Value.ToLower().Contains("admin"))
                                            rcon.Send(Language.Say("admhelp", user, user.Colour, user.Name));
                                    }

                                    /*
                                     * Suicide messages
                                     */
                                    else if (Regex.IsMatch(package.Response, validations["suicide"]))
                                    {
                                        Match match = Regex.Match(package.Response, validations["suicide"]);
                                        User user = Data.Instance.GetUserByName(match.Groups[1].Value);
                                        rcon.Send(Language.Say("suicided", user, user.Colour, user.Name));
                                        Data.Instance.WriteToIRC(new ChatLog() { Text = user.Name, User = new User("-1", "#SUICIDE"), Time = DateTime.Now });
                                        Data.Instance.HardLog(String.Format("Suicide {0}", user.Name));
                                    }

                                    /*
                                     * Violation messages
                                     */
                                    else if (Regex.IsMatch(package.Response, validations["violation"]))
                                    {
                                        Match match = Regex.Match(package.Response, validations["violation"]);
                                        User user = Data.Instance.GetUserById(match.Groups[3].Value);
                                        rcon.Send(Language.Say("violation", user, user.Colour, user.Name, match.Groups[1].Value));
                                        Data.Instance.WriteViolation(user, int.Parse(match.Groups[1].Value));
                                        Data.Instance.WriteToIRC(new ChatLog() { Text = String.Format("Violation {0}: {1}", match.Groups[1].Value, user.Name), User = new User("-1", "#KICK"), Time = DateTime.Now });
                                        Data.Instance.HardLog(String.Format("Violation {0} by user {1}", match.Groups[1].Value, user.Name));
                                    }

                                    /*
                                     * Save messages
                                     */
                                    else if (Regex.IsMatch(package.Response, validations["save"]))
                                    {
                                        Match match = Regex.Match(package.Response, validations["save"]);
                                        rcon.Send(Language.Say("save", null, match.Groups[1].Value));
                                        Data.Instance.HardLog(String.Format("Saved {0} objects (diff {1})", match.Groups[1].Value, int.Parse(match.Groups[1].Value) - prev_save));
                                        prev_save = int.Parse(match.Groups[1].Value);
                                    }

                                    /*
                                     * Save2 messages
                                     */
                                    else if (Regex.IsMatch(package.Response, validations["save2"]))
                                    {
                                        // Ignore.
                                    }

                                    /*
                                     * Ticket cancelled messages
                                     */
                                    else if (Regex.IsMatch(package.Response, validations["ticket"]))
                                    {
                                        Match match = Regex.Match(package.Response, validations["ticket"]);
                                        User user = Data.Instance.GetUserById(match.Groups[2].Value);
                                        rcon.Send(Language.Say("ticket", user, user.Colour, user.Name));
                                        Data.Instance.HardLog(String.Format("Ticket for {0} was cancelled", user.Name));
                                    }

                                    /*
                                     * Entry denied messages
                                     */
                                    else if (Regex.IsMatch(package.Response, validations["entry"]))
                                    {
                                        Match match = Regex.Match(package.Response, validations["entry"]);
                                        User user = Data.Instance.GetUserById(match.Groups[1].Value);
                                        rcon.Send(Language.Say("entry", user, user.Colour, user.Name));
                                        Data.Instance.HardLog(String.Format("Entry for {0} was denied", user.Name));
                                    }

                                    /*
                                     * Validation messages
                                     */
                                    else if (Regex.IsMatch(package.Response, validations["validator"]))
                                    {
                                        // TODO: handle.
                                    }

                                    /*
                                     * global.say messages
                                     */
                                    else if (Regex.IsMatch(package.Response, validations["globalsay"]))
                                    {
                                        // TODO: handle.
                                    }

                                    /*
                                     * status messages
                                     */
                                    else if (Regex.IsMatch(package.Response, validations["status"]))
                                    {
                                        // TODO: handle.
                                    }

                                    /*
                                     * time messages
                                     */
                                    else if (Regex.IsMatch(package.Response, validations["time"]))
                                    {
                                        // TODO: handle.
                                    }

                                    /*
                                     * items messages
                                     */
                                    else if (Regex.IsMatch(package.Response, validations["items"]))
                                    {
                                        // TODO: handle.
                                    }

                                    /*
                                     * topos messages
                                     */
                                    else if (Regex.IsMatch(package.Response, validations["topos"]))
                                    {
                                        // TODO: handle.
                                        Data.Instance.HardLog(package.Response);
                                    }

                                    /*
                                     * move messages
                                     */
                                    else if (Regex.IsMatch(package.Response, validations["moved"]))
                                    {
                                        // TODO: handle.
                                        Data.Instance.HardLog(package.Response);
                                    }

                                    /*
                                     * ondestroy messages
                                     */
                                    else if (Regex.IsMatch(package.Response, validations["disposed"]))
                                    {
                                        commands.cmd_report(new User("-1", "CRITICAL"), "report", "SERVER WENT DOWN", new string[] { "SERVER", "WENT", "DOWN" });
                                        Data.Instance.HardLog("SERVER WENT DOWN");
                                    }

                                    /*
                                     * Everything else
                                     */
                                    else
                                    {
                                        Console.WriteLine("Unhandled package:");
                                        Data.Instance.HardLog("Unhandled package received");
                                        Console.WriteLine(package.Response);
                                    }

                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                    Console.WriteLine(e.StackTrace);
                                    if (e.InnerException != null)
                                        Console.WriteLine(e.InnerException.Message);
                                    Data.Instance.HardLog(e.Message);
                                }
                            }).Start();
                    }
                });
            Task crons = new Task(() =>
                {
                    cron.Run(rcon);
                });

            passives.Start();
            crons.Start();

            Task.WaitAll(new Task[] { passives, crons });
        }
    }
}
