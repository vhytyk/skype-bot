using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SkypeBot.BotEngine.Commands;

namespace SkypeBot.BotEngine
{
    public class SkypeCommandProvider : ISkypeCommandProvider
    {
        public ISkypeCommand GetCommand(string commandMessage)
        {
            Match commandMatch = Regex.Match(commandMessage, @"^bot#(\w+)\s+(.*)");
            if (commandMatch.Success)
            {
                string command = commandMatch.Groups[1].Value;
                switch (command.ToLower())
                {
                    case "rl":
                    case "rallylink":
                        return new RallyLinkSkypeCommand(commandMatch.Groups[2].Value);
                }
            }
            return null;
        }
    }
}
