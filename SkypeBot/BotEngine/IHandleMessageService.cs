using System;

namespace SkypeBot.BotEngine
{
    public interface IHandleMessageService
    {
        void HandleIncomeMessage(string source, string message, Action<string, string> responseAction);
    }
}
