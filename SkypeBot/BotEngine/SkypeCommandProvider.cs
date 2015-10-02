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

        public static IList<SkypeCommandInfo> AllCommandsMetaData = new []
        {
             new SkypeCommandInfo
            {
                Name = "Help",
                Command = "help",
                ShortCommand = "h",
                CommandClassType = typeof (HelpCommand),
                Description = "Provides list of commands or help for particular one.\rUsage: bot#[help|h] [command]"
            },
            new SkypeCommandInfo
            {
                Name = "Rally Link",
                Command = "rallylink",
                ShortCommand = "rl",
                CommandClassType = typeof (RallyLinkSkypeCommand),
                Description = "Get rally weblink by id.\rUsage: bot#[rallylink|rl] [JA|DE]####"
            },
            new SkypeCommandInfo
            {
                Name = "Release Number",
                Command = "relver",
                ShortCommand = "rv",
                CommandClassType = typeof (ReleaseVersionSkypeCommand),
                Description = "Returns current release number.\rUsage: bot#[relver|rv]"
            },
            new SkypeCommandInfo
            {
                Name = "External ID info",
                Command = "extid",
                ShortCommand = "eid",
                CommandClassType = typeof (ExternalId),
                Description = "Searches for the internal id by external one and vice-a-versa.\rUsage: bot#[extid|eid] [internalId|externalId]"
            },
             new SkypeCommandInfo
            {
                Name = "Bank rates",
                Command = "kurs",
                ShortCommand = "kurs",
                CommandClassType = typeof (KursCommand),
                Description = "Shows bank rates UAH/USD. \rUsage: bot#kurs"
            },
             new SkypeCommandInfo
            {
                Name = "Learn",
                Command = "learn",
                ShortCommand = "learn",
                CommandClassType = typeof (LearnCommand),
                Description = "Can learn any phrase/response pair. Usage: bot#learn (<phrase>) (<response>)"
            }
        };

        public static SkypeCommandInfo GetCommandByName(string name)
        {
            return AllCommandsMetaData.FirstOrDefault(s => s.Command == name || s.ShortCommand == name);
        }

        #endregion

        public ISkypeCommand GetCommand(string commandMessage)
        {
            Match commandMatch = Regex.Match(commandMessage, @"^bot#(\w+)\s*(.*)");
            if (commandMatch.Success)
            {
                string commandName = commandMatch.Groups[1].Value;
                SkypeCommandInfo info = GetCommandByName(commandName);
                if (info != null)
                {
                    var command = Activator.CreateInstance(info.CommandClassType) as ISkypeCommand;
                    if (command != null)
                    {
                        command.Init(commandMatch.Groups[2].Value);
                        return command;
                    }
                }
                else
                {
                    UnknownCommand unknownCommand = new UnknownCommand();
                    unknownCommand.Init(commandName);
                    return unknownCommand;
                }
            }
            return null;
        }
    }
}
