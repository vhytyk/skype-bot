using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkypeBot.BotEngine
{
    public interface IHandleMessageService
    {
        void HandleIncomeMessage(string source, string message, Action<string, string> responseAction);
    }
}
