using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkypeBot.Wiki;
namespace SkypeBot.BotEngine.Commands
{
    public class WikiSkypeCommand : ISkypeCommand
    {
        private string _searchPhrase;
        public string RunCommand()
        {
            string result = null;
            if (!string.IsNullOrEmpty(_searchPhrase))
            {
                var service = new ConfluenceSoapServiceService();

                var token = service.login("vhytyk", "Trylystnyk_2015");
                result = string.Join("\r",
                    service.search(token, _searchPhrase, 20)
                        .Where(r => r.type == "page")
                        .Take(3)
                        .ToList()
                        .Select(r => string.Format("{0} ({1})", r.url, r.title)));
            }

            return result;
        }

        public void Init(string arguments)
        {
            _searchPhrase = arguments;
        }
    }
}
