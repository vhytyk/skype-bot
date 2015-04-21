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
    public static class RallyHelper
    {
        public static QueryResult RequetQuery(string artifact, Query query)
        {
            var restApi = new RallyRestApi();
            restApi.Authenticate("victor.hytyk@justanswer.com", "Qwerty123$", "https://rally1.rallydev.com", proxy: null, allowSSO: false);

            Request request = new Request(artifact);
            request.Query = query;
            return restApi.Query(request);
        }
    }

    public class RallyLinkSkypeCommand : ISkypeCommand
    {
        private string storyId = string.Empty;
        private bool isDefect = false;
        public void Init(string arguments)
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
                dynamic result =
                    RallyHelper.RequetQuery(isDefect ? "defect" : "hierarchicalrequirement",
                        new Query("FormattedID", Query.Operator.Equals, storyId)).Results.FirstOrDefault();
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
