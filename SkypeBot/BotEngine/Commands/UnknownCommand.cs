using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkypeBot.BotEngine.Commands
{
    public class UnknownCommand : ISkypeCommand
    {
        private string unknownCommandName = string.Empty;
        public string RunCommand() { return string.Format("sorry pal, have no idea about '{0}' command.", unknownCommandName); }

        public void Init(string arguments) { unknownCommandName = arguments; }
    }
}
