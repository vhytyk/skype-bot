using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rally.RestApi;

namespace SkypeBot.BotEngine.Commands
{
    public class ReleaseVersionSkypeCommand : ISkypeCommand
    {
        public DateTime IterationDateTime { get; set; }
        public string RunCommand()
        {
            dynamic result = RallyHelper.RequetQuery("iteration",
                new Query("((StartDate <= \"" + IterationDateTime.ToString("yyyy-MM-dd") + "\") AND (EndDate >= \"" +
                          IterationDateTime.AddDays(-7).ToString("yyyy-MM-dd") + "\"))")).Results.LastOrDefault();
            if (result != null)
            {
                string ver = result["Name"];
                return ver;
            }
            return null;
        }

        public void Init(string arguments)
        {
            IterationDateTime = DateTime.Now;
        }
    }
}
