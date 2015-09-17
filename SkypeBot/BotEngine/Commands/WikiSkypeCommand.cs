using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitlyDotNET.Implementations;
using BitlyDotNET.Interfaces;
using SkypeBot.Wiki;
namespace SkypeBot.BotEngine.Commands
{
    public class WikiSkypeCommand : ISkypeCommand
    {
        private string _searchPhrase;
        private static string wikiToken = null;
        public string RunCommand()
        {
            IBitlyService bitlyService = new BitlyService("vhytyk", "R_5d31d4dc7a364cf6af291c7533655e55");

            string result = null;
            if (!string.IsNullOrEmpty(_searchPhrase))
            {
                var wikiService = new ConfluenceSoapServiceService();
                if (null == wikiToken)
                {
                    wikiToken = wikiService.login("vhytyk", "");
                    
                }

                var resultList = new List<string>();
                var searchList = wikiService.search(wikiToken, _searchPhrase, 20)
                    .Where(r => r.type == "page")
                    .Take(3)
                    .ToList();
                
                searchList.ForEach(s =>
                {
                    resultList.Add(string.Format("{1} ({0})",
                        bitlyService.Shorten(s.url.Replace("wiki/", "wiki.justanswer.local/")), s.title));
                });



                result = string.Join("\r", resultList);
            }

            return result;
        }

        public void Init(string arguments)
        {
            _searchPhrase = arguments;
        }
    }
}
