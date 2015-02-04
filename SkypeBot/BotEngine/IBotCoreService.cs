using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkypeBot.BotEngine
{
    public interface IBotCoreService : IDisposable
    {
        void InitSkype();
        void SendMessage(string contact, string message);
    }
}
