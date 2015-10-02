using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using NUnit.Framework.Constraints;

namespace SkypeBot.BotEngine.Commands
{
    public class KursCommand : ISkypeCommand
    {
        public string RunCommand()
        {
            string kursPage = GetPageSource("http://minfin.com.ua/currency/mb/");

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(kursPage);
            HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode("//table[0]");
            string[] kurs1 = htmlDocument.DocumentNode.SelectSingleNode(
                "//table[@class='mb-table-currency']/tbody/tr[1]/td[2]")
                .InnerText.Split('\n');
            string[] kurs2 = htmlDocument.DocumentNode.SelectSingleNode(
                "//table[@class='mb-table-currency']/tbody/tr[2]/td[2]")
                .InnerText.Split('\n');
            double kurs1Num = double.Parse(kurs1[0].Trim());
            double diff1Num = double.Parse(kurs1[1].Trim());
            double kurs2Num = double.Parse(kurs2[0].Trim());
            double diff2Num = double.Parse(kurs2[1].Trim());

            kursPage = GetPageSource("http://minfin.com.ua/currency/banks/");
            htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(kursPage);
            node = htmlDocument.DocumentNode.SelectSingleNode("//td[@class='mfm-text-nowrap']");
            Match kurs3Match = Regex.Match(node.InnerText, @"(\d+\.\d+)\s+(-*\d+\.\d+)\s+\/\s+(-*\d+\.\d+)\s+(\d+\.\d+)");

            return string.Format("Mizhbank: {0}({1}), {2}({3})\rBanks: {4}, {5}", kurs1Num, diff1Num, kurs2Num, diff2Num, kurs3Match.Groups[1].Value, kurs3Match.Groups[4].Value);
        }

        private string GetPageSource(string page)
        {
            WebClient client = new WebClient();
            string _UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
            client.Headers.Add(HttpRequestHeader.UserAgent, _UserAgent);
            return client.DownloadString(page);
        }

        public void Init(string arguments)
        {
            
        }
    }
}
