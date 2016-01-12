using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace SkypeBot.BotEngine.Commands
{
    class GotomeetingCommand : ISkypeCommand
    {
        public string RunCommand()
        {
            string URI = "https://api.citrixonline.com/G2MFREE/rest/v1/session";

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                string HtmlResult = wc.UploadString(URI, "");
                Match idValueMatch = Regex.Match(HtmlResult, @"""webUrl"":""(.*?)""");
                return idValueMatch.Groups[1].Value;
            }

        }

        public void Init(string arguments)
        {
            
        }
    }
}
