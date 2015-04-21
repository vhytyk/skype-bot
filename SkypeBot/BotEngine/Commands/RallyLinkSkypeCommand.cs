using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Rally.RestApi;
using Rally.RestApi.Response;

namespace SkypeBot.BotEngine.Commands
{
    public class RallyLinkSkypeCommand : ISkypeCommand
    {
        private string storyId = string.Empty;
        private bool isDefect = false;
        public RallyLinkSkypeCommand(string arguments)
        {
            if (!string.IsNullOrWhiteSpace(arguments))
            {
                Match storyIdMatch = Regex.Match(arguments, @"^(\w\w)(\d+)");
                if (storyIdMatch.Success)
                {
                    storyId = storyIdMatch.Value;
                    isDefect = storyIdMatch.Groups[1].Value.ToLower() == "de";
                }
            }
        }
        public string RunCommand()
        {
            if (!string.IsNullOrWhiteSpace(storyId))
            {
                var restApi = new RallyRestApi();
                restApi.Authenticate("victor.hytyk@pearl.com", "trylystnyk_2015", "https://rally1.rallydev.com", proxy: null, allowSSO: false);

                Request request = new Request(isDefect ? "defect" : "hierarchicalrequirement");
                request.Query = new Query("FormattedID", Query.Operator.Equals, storyId);
                QueryResult queryResult = restApi.Query(request);
                dynamic result = queryResult.Results.FirstOrDefault();
                if (null != result)
                {
                    string url = string.Format("https://rally1.rallydev.com/#/detail/{0}/{1}",
                        isDefect ? "defect" : "userstory", result["ObjectID"]);

                    return url;
                }
            }
            return null;
        }
    }
}
