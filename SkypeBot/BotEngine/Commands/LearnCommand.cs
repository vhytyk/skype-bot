using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using NUnit.Framework.Constraints;
using SkypeBotRulesLibrary;

namespace SkypeBot.BotEngine.Commands
{
    public class LearnCommand : ISkypeCommand
    {
        private bool argumentsError = false;
        private string _arguments = "";
        private string stringToListen = string.Empty;
        private string stringToResponse = string.Empty;
        public string RunCommand()
        {
            if (argumentsError)
            {
                return
                    string.Format(@"Command has invalid arguments ({0}). Please, follow help guide (bot#learn '<string_to_listen>' '<string_to_response>')",_arguments);
            }
            if (SaveToDb(new Guid().ToString(), stringToListen, stringToResponse))
            {
                return string.Format(@"'{0}' was successfully learned", stringToListen);
            }
            return "";
        }

        bool SaveToDb(string name, string listen, string response)
        {
            return UnityConfiguration.Instance.Reslove<IRuleService>().AddRule(new SkypeBotRule
            {
                Name = name,
                Rule = listen,
                Value = response
            });
        }

        public void Init(string arguments)
        {
            _arguments = arguments.Replace("&apos;", "'");
            if (string.IsNullOrEmpty(arguments))
            {
                argumentsError = true;
            }
            else
            {
                Match argumentsMatch = Regex.Match(_arguments, @"\'([\w\s\,\.\d\:\?\!\;]+)\'\s+\'([\w\s\,\.\d\:\?\!\;]+)\'");
                if (argumentsMatch.Success)
                {
                    stringToListen = argumentsMatch.Groups[1].Value;
                    stringToResponse = argumentsMatch.Groups[2].Value;
                }
                else
                {
                    argumentsError = true;
                }
            }
        }
    }
}
