﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SkypeBot.BotEngine.Commands
{
    public class HelpCommand : ISkypeCommand
    {
        private string commandName = null;
        public string RunCommand()
        {
            if (commandName != null)
            {
                SkypeCommandInfo info = SkypeCommandProvider.GetCommandByName(commandName);
                if (info != null)
                {
                    return string.Format("{0}: {1}", info.Name, info.Description);
                }
            }
            else
            {
                string commands = string.Join(", ", SkypeCommandProvider.AllCommandsMetaData.Select(s => s.Command));
                return string.Format("Commands: {0}\rTo get detailed info about any of command type bot#help [command name]", commands);
            }
            return null;
        }

        public void Init(string arguments)
        {
            Match commandNameMatch = Regex.Match(arguments, @"^(\w+)\s*");
            if (commandNameMatch.Success)
            {
                commandName = commandNameMatch.Groups[1].Value;
            }
        }
    }
}
