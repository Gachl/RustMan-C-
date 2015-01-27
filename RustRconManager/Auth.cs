using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RustRconManager
{
    class Auth
    {
        public static Auth None = new Auth("None");
        public static Auth Bronze = new Auth("Bronze");
        public static Auth Silver = new Auth("Silver");
        public static Auth Gold = new Auth("Gold");
        public static Auth Platinum = new Auth("Platinum");
        public static Auth Diamond = new Auth("Diamond");
        public static Auth SubMod = new Auth("SubMod");
        public static Auth Mod = new Auth("Mod");
        public static Auth ChiefAdmin = new Auth("ChiefAdmin");
        public static Auth Admin = new Auth("Admin");

        public static readonly Dictionary<string, Auth> Rights = new Dictionary<string, Auth>()
            {
                {"76561197976732818", Auth.Admin}, // Raven
                {"76561198069332364", Auth.SubMod}, // Tankton
                {"76561198025524624", Auth.ChiefAdmin}, // Kanlo
                {"76561198077235495", Auth.SubMod} // SalatFisten
            };

        private Auth(string auth)
        {
            this.auth = auth;

            this.hierarchy = Array.FindIndex(new string[]
            {
                "None",
                "Bronze",
                "Silver",
                "Gold",
                "Platinum",
                "Diamond",
                "SubMod",
                "Mod",
                "ChiefAdmin",
                "Admin"
            }, h => h == auth);
        }

        private string auth;

        public override string ToString()
        {
            return this.auth;
        }

        private int hierarchy;

        public int Hierarchy
        {
            get { return this.hierarchy; }
        }

        internal static Auth Parse(string name)
        {
            switch (name.ToLower().Trim())
            {
                case "none":
                    return Auth.None;
                case "bronze":
                    return Auth.Bronze;
                case "silver":
                    return Auth.Silver;
                case "gold":
                    return Auth.Gold;
                case "platinum":
                    return Auth.Platinum;
                case "diamond":
                    return Auth.Diamond;
                case "submod":
                    return Auth.SubMod;
                case "mod":
                    return Auth.Mod;
                case "chiefadmin":
                    return Auth.ChiefAdmin;
                case "admin":
                    return Auth.Admin;
            }
            return null;
        }
    }
}
