using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Collections.Concurrent;
using System.Net;
using System.IO;

namespace RustRconManager
{
    class Data
    {
        public static LookupService geoip = new LookupService(@"/tmp/GeoIP.dat", LookupService.GEOIP_MEMORY_CACHE);

        private static Data instance = null;
        public static Data Instance
        {
            get
            {
                if (instance == null)
                    instance = new Data();

                return instance;
            }
        }

        private Data()
        {

        }

        private MySqlConnection connect()
        {
            MySqlConnection c = new MySqlConnection("Server=localhost;Database=...;Uid=...;Pwd=...;Pooling=false;charset=utf8");//builder.ToString());
            c.Open();
            return c;
        }

        public User GetUserById(string id)
        {
            UInt64 proper_id = 0;
            if (!UInt64.TryParse(id, out proper_id))
                return null;

            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("SELECT `id`, `name`, `time`, `ip`, `online`, `last`, `member`, `karma`, `vac`, `first`, `votes`, `auth`, `watched`, `warn` FROM `users` WHERE `id` = ?id;", connection);
                query.Prepare();
                query.Parameters.Add("?id", proper_id);


                using (MySqlDataReader reader = query.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return null;

                    reader.Read();

                    User result = new User(reader["id"].ToString(), reader["name"].ToString(), (UInt32)reader["time"], reader["ip"].ToString(), (sbyte)reader["online"] == 1, (long)(UInt64)reader["last"], (UInt32)reader["member"], (int)reader["karma"], (sbyte)reader["vac"] == 1, (DateTime)reader["first"], (UInt32)reader["votes"], (String.IsNullOrEmpty(reader["auth"].ToString()) ? Auth.None : Auth.Parse(reader["auth"].ToString())), (sbyte)reader["watched"] == 1, (int)reader["warn"]);
                    return result;
                }
            }
        }

        public User GetUserByName(string name)
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("SELECT `id` FROM `users` WHERE `name` = ?name;", connection);
                query.Prepare();
                query.Parameters.Add("?name", name);


                using (MySqlDataReader reader = query.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return null;

                    reader.Read();
                    string id = reader["id"].ToString();
                    return this.GetUserById(id);
                }
            }
        }

        public List<User> GetOnlineUsers()
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("SELECT `id` FROM `users` WHERE `online` = 1;", connection);
                using (MySqlDataReader reader = query.ExecuteReader())
                {
                    List<User> online = new List<User>();
                    while (reader.Read())
                        online.Add(GetUserById(reader["id"].ToString()));

                    return online;
                }
            }
        }

        public List<User> GetClosestUserByName(string name)
        {
            name = name.Trim();
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("SELECT `id`, `name` FROM `users` WHERE `name` LIKE ?name;", connection);
                query.Prepare();
                query.Parameters.Add("?name", "%" + name + "%");


                using (MySqlDataReader reader = query.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return null;


                    List<User> result = new List<User>();
                    while (reader.Read())
                    {
                        if (reader["name"].ToString().Trim().ToLower() == name.ToLower())
                        {
                            return new List<User>() { GetUserById(reader["id"].ToString()) };
                        }
                        result.Add(GetUserById(reader["id"].ToString()));
                    }

                    return result;
                }
            }
        }

        public List<User> GetUsersByGroup(int group)
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("SELECT `id` FROM `users` WHERE `member` = ?group;", connection);
                query.Prepare();
                query.Parameters.Add("?group", group);

                using (MySqlDataReader reader = query.ExecuteReader())
                {
                    List<User> result = new List<User>();

                    while (reader.Read())
                        result.Add(GetUserById(reader["id"].ToString()));

                    return result;
                }
            }
        }

        public void SaveUser(User user)
        {
            if (user.ID == "-1")
                return;
            /*
            UInt64 proper_id = UInt64.Parse(user.ID);
            BlockingCollection<User> new_cache = new BlockingCollection<User>();
            new_cache.Add(user);
            */

            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("INSERT INTO `users` (`id`, `name`, `time`, `ip`, `online`, `last`, `member`, `karma`, `vac`, `first`, `votes`, `auth`, `watched`, `warn`) VALUES (?id, ?name, ?time, ?ip, ?online, ?last, ?member, ?karma, ?vac, NOW(), ?votes, ?auth, ?watched, ?warn) ON DUPLICATE KEY UPDATE `id` = ?id, `name` = ?name, `time` = IF(`time` < ?time, ?time, `time`), `ip` = ?ip, `online` = ?online, `last` = ?last, `member` = ?member, `karma` = ?karma, `vac` = ?vac, `votes` = ?votes, `auth` = ?auth, `watched` = ?watched, `warn` = ?warn;", connection);
                query.Prepare();
                query.Parameters.Add("?id", UInt64.Parse(user.ID));
                query.Parameters.Add("?name", user.Name);
                query.Parameters.Add("?time", user.Time);
                query.Parameters.Add("?ip", user.IP);
                query.Parameters.Add("?online", user.Online);
                query.Parameters.Add("?last", user.Last);
                query.Parameters.Add("?member", user.Member);
                query.Parameters.Add("?karma", user.Karma);
                query.Parameters.Add("?vac", user.VAC);
                query.Parameters.Add("?votes", user.Votes);
                query.Parameters.Add("?auth", user.Auth.ToString());
                query.Parameters.Add("?watched", user.Watched);
                query.Parameters.Add("?warn", user.Warn);

                query.ExecuteNonQuery();
            }
        }

        public void UserAddTime(User user, int time)
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("INSERT INTO `users` (`id`, `name`, `time`, `ip`, `online`, `last`, `member`, `karma`, `vac`, `first`, `votes`, `auth`, `watched`, `warn`) VALUES (?id, ?name, ?time, ?ip, ?online, ?last, ?member, ?karma, ?vac, NOW(), ?votes, ?auth, ?watched, ?warn) ON DUPLICATE KEY UPDATE `time` = `time` + ?time, `last` = ?last;", connection);
                query.Prepare();
                query.Parameters.Add("?id", UInt64.Parse(user.ID));
                query.Parameters.Add("?name", user.Name);
                query.Parameters.Add("?time", time);
                query.Parameters.Add("?ip", user.IP);
                query.Parameters.Add("?online", user.Online);
                query.Parameters.Add("?last", user.Last);
                query.Parameters.Add("?member", user.Member);
                query.Parameters.Add("?karma", user.Karma);
                query.Parameters.Add("?vac", user.VAC);
                query.Parameters.Add("?votes", user.Votes);
                query.Parameters.Add("?auth", user.Auth.ToString());
                query.Parameters.Add("?watched", user.Watched);
                query.Parameters.Add("?warn", user.Warn);

                query.ExecuteNonQuery();
            }
        }

        internal DateTime GetCommandTime(string ident)
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("SELECT `time` FROM `command_times` WHERE `id` = ?id;", connection);
                query.Prepare();
                query.Parameters.Add("?id", ident);


                using (MySqlDataReader reader = query.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return DateTime.Now;

                    reader.Read();
                    DateTime result = new DateTime((long)reader["time"]);

                    return result;
                }
            }
        }

        internal void SetCommandTime(string ident, DateTime time)
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("INSERT INTO `command_times` (`id`, `time`) VALUES (?id, ?time) ON DUPLICATE KEY UPDATE `time` = ?time;", connection);
                query.Prepare();
                query.Parameters.Add("?id", ident);
                query.Parameters.Add("?time", time.Ticks);

                query.ExecuteNonQuery();
            }
        }

        public List<string> GetKit(string kit, Auth permission)
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("SELECT `command`, `permission` FROM `kits` WHERE `name` LIKE ?kit;", connection);
                query.Prepare();
                query.Parameters.Add("?kit", kit);


                using (MySqlDataReader reader = query.ExecuteReader())
                {

                    if (!reader.HasRows)
                        return null;

                    List<string> result = new List<string>();
                    while (reader.Read())
                    {
                        if (permission.Hierarchy >= Auth.Parse(reader["permission"].ToString()).Hierarchy)
                            result.Add(reader["command"].ToString());
                    }

                    return result;
                }
            }
        }

        internal Dictionary<string, DateTime> GetCrons()
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("SELECT `action`, `time` FROM `cron`;", connection);
                using (MySqlDataReader reader = query.ExecuteReader())
                {
                    Dictionary<string, DateTime> crons = new Dictionary<string, DateTime>();
                    while (reader.Read())
                        crons.Add(reader["action"].ToString(), new DateTime((long)(UInt64)reader["time"]));

                    return crons;
                }
            }
        }

        internal void SaveCron(string ident, DateTime next)
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("INSERT INTO `cron` (`action`, `time`) VALUES (?action, ?time) ON DUPLICATE KEY UPDATE `time` = ?time;", connection);

                query.Parameters.Add("?action", ident);
                query.Parameters.Add("?time", next.Ticks);
                query.Prepare();
                query.ExecuteNonQuery();
            }
        }

        internal string GetTeleport(string name)
        {
            return GetTeleport(name, Auth.Admin);
        }

        internal string GetTeleport(string name, Auth auth)
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("SELECT `location`, `permission` FROM `teleports` WHERE `name` LIKE ?name;", connection);
                query.Prepare();
                query.Parameters.Add("?name", name);

                using (MySqlDataReader reader = query.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return null;
                    reader.Read();

                    if (auth.Hierarchy >= Auth.Parse(reader["permission"].ToString()).Hierarchy)
                        return reader["location"].ToString();
                }
            }
            return null;
        }

        internal List<string> GetTeleportNamesByAccessRights(Auth right)
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("SELECT `name`, `permission` FROM `teleports`;", connection);
                using (MySqlDataReader reader = query.ExecuteReader())
                {
                    List<string> result = new List<string>();
                    while (reader.Read())
                    {
                        Auth permission = Auth.Parse(reader["permission"].ToString());
                        if (right.Hierarchy >= permission.Hierarchy)
                            result.Add(reader["name"].ToString());
                    }
                    return result;
                }
            }
        }

        internal void WriteChatLine(User user, string text)
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("INSERT INTO `chatlog` (`user`, `time`, `text`) VALUES (?user, ?time, ?text);", connection);
                query.Prepare();
                query.Parameters.Add("?user", user.ID);
                query.Parameters.Add("?time", DateTime.Now.Ticks);
                query.Parameters.Add("?text", text);

                query.ExecuteNonQuery();
            }
        }

        internal List<ChatLog> ReadChatLines(int amount)
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("SELECT `user`, `time`, `text` FROM `chatlog` ORDER BY `time` DESC LIMIT 0, ?amount;", connection);
                query.Prepare();
                query.Parameters.Add("?amount", amount);


                List<ChatLog> log = new List<ChatLog>();

                using (MySqlDataReader reader = query.ExecuteReader())
                    while (reader.Read())
                        log.Add(new ChatLog() { User = GetUserById(reader["user"].ToString()), Time = new DateTime((long)(UInt64)reader["time"]), Text = reader["text"].ToString() });

                log.Reverse();
                return log;
            }
        }

        internal Location GetLocation(string id)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(String.Format("ftp://.../avatars/{0}/avatar.bin", id));
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential("daniel", "....");

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            byte[] avatar = Encoding.UTF8.GetBytes(new StreamReader(response.GetResponseStream()).ReadToEnd());
            response.Close();

            return new Location() { X = BitConverter.ToSingle(avatar, 3), Y = BitConverter.ToSingle(avatar, 5), Z = BitConverter.ToSingle(avatar, 7) };
        }

        internal Group? GetGroupById(int id)
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("SELECT `name`, `admin`, `alliance` FROM `groups` WHERE `id` = ?id;", connection);
                query.Prepare();
                query.Parameters.Add("?id", id);

                using (MySqlDataReader reader = query.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return null;

                    reader.Read();
                    return new Group() { ID = id, Name = reader["name"].ToString(), Admin = GetUserById(reader["admin"].ToString()), Members = GetUsersByGroup(id), Alliance = (int)(UInt32)reader["alliance"] };
                }
            }
        }

        internal List<Group> GetClosestGroupByName(string name)
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("SELECT `id` FROM `groups` WHERE `name` LIKE ?name;", connection);
                query.Prepare();
                query.Parameters.Add("?name", name);

                using (MySqlDataReader reader = query.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return null;

                    List<Group> result = new List<Group>();

                    while (reader.Read())
                    {
                        Group? group = GetGroupById((int)(UInt32)reader["id"]);
                        if (group != null)
                            result.Add((Group)group);
                    }

                    return result;
                }
            }
        }

        internal List<Group> GetGroupsByAlliance(int id)
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("SELECT `id` FROM `groups` WHERE `alliance` = ?alliance;", connection);
                query.Prepare();
                query.Parameters.Add("?alliance", id);


                using (MySqlDataReader reader = query.ExecuteReader())
                {
                    List<Group> result = new List<Group>();

                    while (reader.Read())
                    {
                        Group? group = GetGroupById((int)(UInt32)reader["id"]);
                        if (group != null)
                            result.Add((Group)group);
                    }

                    return result;
                }
            }
        }

        internal int GetGroupCount()
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("SELECT COUNT(`id`) AS `count` FROM `groups`;", connection);

                using (MySqlDataReader reader = query.ExecuteReader())
                {
                    reader.Read();

                    return int.Parse(reader["count"].ToString());
                }
            }
        }

        internal Alliance? GetAllianceById(int id)
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("SELECT `name`, `admin` FROM `alliances` WHERE `id` = ?id;", connection);
                query.Prepare();
                query.Parameters.Add("?id", id);

                using (MySqlDataReader reader = query.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return null;

                    reader.Read();
                    return new Alliance() { ID = id, Name = reader["name"].ToString(), Admin = GetUserById(reader["admin"].ToString()), Members = GetGroupsByAlliance(id) };
                }
            }
        }

        internal List<Alliance> GetClosestAllianceByName(string name)
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("SELECT `id` FROM `alliances` WHERE `name` LIKE ?name;", connection);
                query.Prepare();
                query.Parameters.Add("?name", name);


                using (MySqlDataReader reader = query.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return null;

                    List<Alliance> result = new List<Alliance>();

                    while (reader.Read())
                    {
                        Alliance? alliance = GetAllianceById((int)(UInt32)reader["id"]);
                        if (alliance != null)
                            result.Add((Alliance)alliance);
                    }

                    return result;
                }
            }
        }

        internal Group CreateGroup(User user, string name)
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("INSERT INTO `groups` (`name`, `admin`) VALUES (?name, ?admin);", connection);
                query.Prepare();
                query.Parameters.Add("?name", name);
                query.Parameters.Add("?admin", user.ID);

                query.ExecuteNonQuery();

                List<Group> groups = GetClosestGroupByName(name);
                if (groups == null)
                    throw new Exception("Group created but not in DB.");

                return groups[0];
            }
        }

        internal object GetAllianceCount()
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("SELECT COUNT(`id`) AS `count` FROM `alliances`;", connection);

                using (MySqlDataReader reader = query.ExecuteReader())
                {
                    reader.Read();

                    return int.Parse(reader["count"].ToString());
                }
            }
        }

        internal Alliance CreateAlliance(User user, string name)
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("INSERT INTO `alliances` (`name`, `admin`) VALUES (?name, ?admin);", connection);
                query.Prepare();
                query.Parameters.Add("?name", name);
                query.Parameters.Add("?admin", user.ID);

                query.ExecuteNonQuery();

                List<Alliance> alliances = GetClosestAllianceByName(name);
                if (alliances == null)
                    throw new Exception("Alliance created but not in DB.");

                return alliances[0];
            }
        }

        internal void SaveGroup(Group group)
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("UPDATE `groups` SET `alliance` = ?alliance, `admin` = ?admin WHERE `id` = ?id;", connection);
                query.Prepare();
                query.Parameters.Add("?alliance", group.Alliance);
                query.Parameters.Add("?admin", group.Admin.ID);
                query.Parameters.Add("?id", group.ID);

                query.ExecuteNonQuery();
            }
        }

        internal void DeleteGroup(int id)
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("DELETE FROM `groups` WHERE `id` = ?id;", connection);
                query.Prepare();
                query.Parameters.Add("?id", id);

                query.ExecuteNonQuery();
                foreach (User user in GetUsersByGroup(id))
                {
                    user.Member = 0;
                    user.Save();
                }
            }
        }

        internal void DeleteAlliance(int id)
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("DELETE FROM `alliances` WHERE `id` = ?id;", connection);
                query.Prepare();
                query.Parameters.Add("?id", id);

                query.ExecuteNonQuery();
                foreach (Group group in GetGroupsByAlliance(id))
                {
                    Group ng = group;
                    ng.Alliance = 0;
                    SaveGroup(ng);
                }
            }
        }

        internal void SaveAlliance(Alliance alliance)
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("UPDATE `alliances` SET `admin` = ?admin WHERE `id` = ?id;", connection);
                query.Prepare();
                query.Parameters.Add("?admin", alliance.Admin == null ? "0" : alliance.Admin.ID);
                query.Parameters.Add("?id", alliance.ID);

                query.ExecuteNonQuery();
            }
        }

        internal void HardLog(string log)
        {
            int timeout = 0;
            do
            {
                try
                {
                    timeout++;
                    if (timeout > 50)
                    {
                        Console.WriteLine("Could not HardLog: {0}", log);
                        return;
                    }

                    File.AppendAllText(@"/home/daniel/bin/rustman/data/violations.txt", String.Format("[???] {0}{1}", log, Environment.NewLine));
                    return;
                }
                catch (Exception)
                {
                }
            } while (true);
        }

        internal void WriteToIRC(ChatLog log)
        {
            int timeout = 0;
            do
            {
                try
                {
                    timeout++;
                    if (timeout > 50)
                    {
                        Console.WriteLine("Could not write to IRC: {0}", log.Text);
                        return;
                    }

                    File.AppendAllText(@"/tmp/rust_???_irc_in", String.Format("{0}\t{1}{2}", log.User.Name, log.Text, Environment.NewLine));
                    return;
                }
                catch (Exception)
                {
                }
            } while (true);
        }

        internal List<ChatLog> ReadFromIRC()
        {
            string[] irc = File.ReadAllLines(@"/tmp/rust_???_irc_out");
            if (irc == null || irc.Length == 0)
                return null;

            File.WriteAllText(@"/tmp/rust_???_irc_out", "");
            List<ChatLog> result = new List<ChatLog>();
            foreach (string irc_line in irc)
            {
                string[] irc_detail = irc_line.Split('\t');
                result.Add(new ChatLog() { Text = irc_detail[1], Time = DateTime.Now, User = new User("-1", irc_detail[0]) });
            }
            return result;
        }

        internal void WriteReport(User user, string report)
        {
            File.AppendAllText("/home/daniel/bin/rustman/data/reports.txt", String.Format("[???] {0}: {1}", user.Name, report));
        }

        internal void WriteJoin(User user)
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("INSERT INTO `visitors` (`user`, `action`, `time`) VALUES (?user, ?action, ?time);", connection);
                query.Prepare();
                query.Parameters.Add("?user", user.ID);
                query.Parameters.Add("?action", "join");
                query.Parameters.Add("?time", DateTime.Now.Ticks);

                query.ExecuteNonQuery();
            }
        }

        internal void WriteQuit(User user)
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("INSERT INTO `visitors` (`user`, `action`, `time`) VALUES (?user, ?action, ?time);", connection);
                query.Prepare();
                query.Parameters.Add("?user", user.ID);
                query.Parameters.Add("?action", "quit");
                query.Parameters.Add("?time", DateTime.Now.Ticks);

                query.ExecuteNonQuery();
            }
        }

        internal void WriteKey(int key)
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("INSERT INTO `visitors` (`user`, `action`, `time`) VALUES (?user, ?action, ?time);", connection);
                query.Prepare();
                query.Parameters.Add("?user", key);
                query.Parameters.Add("?action", "key");
                query.Parameters.Add("?time", DateTime.Now.Ticks);

                query.ExecuteNonQuery();
            }
        }

        internal void WriteViolation(User user, int violation)
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("INSERT INTO `violations` (`user`, `violation`, `amount`) VALUES (?user, ?violation, 1) ON DUPLICATE KEY UPDATE `amount` = `amount` + 1;", connection);
                query.Prepare();
                query.Parameters.Add("?user", user.ID);
                query.Parameters.Add("?violation", violation);

                query.ExecuteNonQuery();
            }
        }

        internal Dictionary<int, string> GetActions()
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("SELECT `id`, `action` FROM `action` ORDER BY `id` ASC;", connection);
                query.Prepare();

                using (MySqlDataReader reader = query.ExecuteReader())
                {
                    if (!reader.HasRows)
                        return result;

                    reader.Read();

                    result.Add((int)reader["id"], (string)reader["action"]);
                }
            }

            return result;
        }

        internal void RemoveAction(int id)
        {
            using (MySqlConnection connection = connect())
            {
                MySqlCommand query = new MySqlCommand("DELETE FROM `action` WHERE `id` = ?id", connection);
                query.Prepare();
                query.Parameters.Add("?id", id);

                query.ExecuteNonQuery();
            }
        }
    }

    internal struct Group
    {
        public int ID;
        public string Name;
        public User Admin;
        public int Alliance;

        public List<User> Members;
    }

    internal struct Alliance
    {
        public int ID;
        public string Name;
        public User Admin;

        public List<Group> Members;
    }

    internal struct ChatLog
    {
        public User User;
        public DateTime Time;
        public string Text;
    }

    internal struct Location
    {
        public float X;
        public float Y;
        public float Z;
    }
}
