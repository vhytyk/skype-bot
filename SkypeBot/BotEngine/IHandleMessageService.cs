using System;
using SkypeBot.SkypeDB;

namespace SkypeBot.BotEngine
{
    public interface IHandleMessageService
    {
        void HandleIncomeMessage(string source, SkypeMessage message, Action<string, string> responseAction);
    }
}
