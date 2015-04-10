using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkypeBot.BotEngine
{
    public interface IChatBotProvider
    {
        string Think(string message);
    }
}
