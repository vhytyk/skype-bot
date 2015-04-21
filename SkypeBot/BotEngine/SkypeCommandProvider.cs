using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SkypeBot.BotEngine.Commands;

namespace SkypeBot.BotEngine
{
    public class SkypeCommandInfo
    {
        public string Name { get; set; }
        public string Command { get; set; }
        public string ShortCommand { get; set; }
        public string Description { get; set; }
        public Type CommandClassType { get; set; }
    }

    public class SkypeCommandProvider : ISkypeCommandProvider
    {
        #region commands
        private IList<SkypeCommandInfo> _allCommandsInfos = new SkypeCommandInfo[]
        {
            new SkypeCommandInfo
            {
                Name = "RallyLink",
                Command = "rallylink",
                ShortCommand = "rl",
                CommandClassType = typeof (RallyLinkSkypeCommand),
                Description = "Get rally weblink by id. Usage: bot#[rallylink|rl] [JA|DE]####"
            }
        };
        #endregion

        public ISkypeCommand GetCommand(string commandMessage)
        {
            Match commandMatch = Regex.Match(commandMessage, @"^bot#(\w+)\s+(.*)");
            if (commandMatch.Success)
            {
                ISkypeCommand result = null;
                string commandName = commandMatch.Groups[1].Value;
                SkypeCommandInfo info =
                    _allCommandsInfos.FirstOrDefault(s => s.Command == commandName || s.ShortCommand == commandName);
                if (info != null)
                {
                    var command = Activator.CreateInstance(info.CommandClassType) as ISkypeCommand;
                    if (command != null)
                    {
                        command.Init(commandMatch.Groups[2].Value);
                        return command;
                    }
                }
            }
            return null;
        }
    }
}
