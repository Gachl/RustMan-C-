using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RustRconManager
{
    class User
    {
        private static Dictionary<Auth, int> vip = new Dictionary<Auth, int>()
        {
            {Auth.None, 0},
            {Auth.Bronze, 36000},
            {Auth.Silver, 72000},
            {Auth.Gold, 180000},
            {Auth.Platinum, 360000},
            {Auth.Diamond, 720000}
        };

        private string id;
        private string name;
        private int time;
        private string ip;
        private bool online;
        private long last;
        private int member;
        private int karma;
        private bool vac;
        private DateTime first;
        private int votes;
        private bool watched;
        private int warn = 0;
        private string safe_name;

        private string country;

        public User(string id, string name)
        {
            this.id = id;
            this.name = name;
            this.safe_name = name.Replace("\"", "\\\"");
        }

        public User(string id, string name, string time, string ip)
        {
            this.id = id;
            this.name = name;
            this.time = int.Parse(time);
            this.ip = ip;
            this.safe_name = name.Replace("\"", "\\\"");
        }

        public User(string id, string name, UInt32 time, string ip, bool online, long last, UInt32 member, int karma, bool vac, DateTime first, UInt32 votes, Auth auth, bool watched, int warn)
        {
            this.id = id;
            this.name = name;
            this.time = (int)time;
            this.ip = ip;
            this.online = online;
            this.last = last;
            this.member = (int)member;
            this.karma = karma;
            this.vac = vac;
            this.first = first;
            this.votes = (int)votes;
            this.auth = auth;
            this.warn = warn;
            this.safe_name = name.Replace("\"", "\\\"");
        }
        public static User GetUserByName(string name)
        {
            return Data.Instance.GetUserByName(name);
        }

        internal static User GetUserById(string id)
        {
            return Data.Instance.GetUserById(id);
        }

        public string ID { get { return this.id; } }

        public string Name { get { return this.name; } set { this.name = value; this.safe_name = this.name.Replace("\"", "\\\""); } }

        public string SafeName { get { return this.safe_name; } }

        public int Language { get { if (this.Country == "Germany" || this.Country == "Switzerland" || this.Country == "Austria") return RustRconManager.Language.GERMAN; return RustRconManager.Language.ENGLISH; } }

        public string Colour
        {
            get
            {
                if (this.Auth.Hierarchy == Auth.None.Hierarchy)
                    return "#FF66FF";
                if (this.Auth.Hierarchy == Auth.Bronze.Hierarchy)
                    return "#CD7F32";
                if (this.Auth.Hierarchy == Auth.Silver.Hierarchy)
                    return "#BCC6CC";
                if (this.Auth.Hierarchy == Auth.Gold.Hierarchy)
                    return "#FDD017";
                if (this.Auth.Hierarchy == Auth.Platinum.Hierarchy)
                    return "#858482";
                if (this.Auth.Hierarchy == Auth.Diamond.Hierarchy)
                    return "#81DAF5";
                return "#FF0000";
            }
        }

        internal void Save()
        {
            Data.Instance.SaveUser(this);
        }

        public bool Online { get { return this.online; } set { this.online = value; } }

        public string Country { get { if (this.country == null) { Country country = Data.geoip.getCountry(this.ip); this.country = country.getName(); } return this.country; } }

        public int Time { get { return this.time; } set { this.time = value; } }

        public string IP { get { return this.ip; } set { this.ip = value; } }

        public long Last { get { return this.last; } set { this.last = value; } }

        public int Member { get { return this.member; } set { this.member = value; } }

        public int Karma { get { return this.karma; } set { this.karma = value; } }

        public bool VAC { get { return this.vac; } set { this.vac = value; } }

        public int Votes { get { return this.votes; } set { this.votes = value; } }

        public bool Watched { get { return this.watched; } set { this.watched = value; } }

        public int Warn { get { return this.warn; } set { this.warn = value; } }

        private Auth auth = Auth.None;
        public Auth Auth
        {
            get
            {
                if (this.auth.Hierarchy == Auth.None.Hierarchy)
                {
                    Auth auth = Auth.None;
                    foreach (Auth test in vip.Keys)
                    {
                        if (this.Time >= vip[test])
                            auth = test;
                    }
                    return auth;
                }
                return this.auth;
            }

            set
            {
                this.auth = value;
            }
        }
    }
}
