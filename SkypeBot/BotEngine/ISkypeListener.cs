using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkypeCore;

namespace SkypeBot.BotEngine
{
    public delegate void SkypeMessageHandler(string source, string message);
    public interface ISkypeListener
    {
        event SkypeMessageHandler SkypeMessageReceived;
        void Initialize();
    }
}
