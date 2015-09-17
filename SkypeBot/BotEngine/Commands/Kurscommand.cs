using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using NUnit.Framework.Constraints;

namespace SkypeBot.BotEngine.Commands
{
    public class KursCommand : ISkypeCommand
    {
        public string RunCommand()
        {
            WebClient client = new WebClient();
            string _UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
            client.Headers.Add(HttpRequestHeader.UserAgent, _UserAgent);
            string kursPage = client.DownloadString("http://minfin.com.ua/currency/mb/");

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
            return string.Format("banks: {0}({1}) - {2}({3})", kurs1Num, diff1Num, kurs2Num, diff2Num);
        }

        public void Init(string arguments)
        {
            
        }
    }
}
