using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SkypeCore
{
    public static class Utils
    {
        public static DateTime UnixTimeStampToDateTime(this double unixTimeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        

        public static string[] GetSkypeAccountsNames()
        {
            var result = new List<string>();
            string appDatadir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var skypeDir = new DirectoryInfo(appDatadir + "\\Skype");
            foreach (var info in skypeDir.GetDirectories())
            {
                if (info.GetFiles("*.db").Any(f => f.Name == "main.db"))
                {
                    result.Add(info.Name);
                }
            }

            return result.ToArray();
        }
    }
}
