using System;

namespace SkypeBot.BotEngine
{
    public interface IBotCoreService : IDisposable
    {
        void InitSkype();
        void SendMessage(string contact, string message);
    }
}
