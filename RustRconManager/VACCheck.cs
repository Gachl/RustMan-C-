using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;

namespace RustRconManager
{
    struct VACResult
    {
        public int Days;
        public int Count;
    }

    class VACCheck
    {
        public static VACResult Check(string id)
        {
            WebClient client = new WebClient();
            string profile = client.DownloadString(String.Format("http://steamcommunity.com/profiles/{0}", id));
            if (!Regex.IsMatch(profile, "([0-9]+) VAC ban\\(s\\) on record[^0-9]*([0-9]+) day\\(s\\) since last ban"))
                return new VACResult() { Count = 0, Days = 0 };

            Match match = Regex.Match(profile, "([0-9]+) VAC ban\\(s\\) on record[^0-9]*([0-9]+) day\\(s\\) since last ban");
            return new VACResult() { Count = int.Parse(match.Groups[1].Value), Days = int.Parse(match.Groups[2].Value) };
        }

        public static VACResult CheatPunchCheck(string id)
        {
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            string profile = client.UploadString("https://playrust.eu/bancheck.php", String.Format("steamid={0}", id));
            if (!Regex.IsMatch(profile, "<td>Ban Reason</td><td>(.*)</td></tr><tr class=\"info-row\""))
                return new VACResult() { Count = 0, Days = 0 };

            return new VACResult() { Count = 1, Days = 1 };
        }
    }
}
