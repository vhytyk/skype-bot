using System;

namespace SkypeBot.SkypeDB
{
    public static class Exstensions
    {
        public static DateTime UnixTimeStampToDateTime(this double unixTimeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
