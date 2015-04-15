using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatterBotAPI;

namespace SkypeBot.BotEngine
{
    public class ChatBotProvider : IChatBotProvider
    {
        private ChatterBotSession _chatterBot = null;

        public ChatBotProvider()
        {
            _chatterBot = new ChatterBotFactory().Create(ChatterBotType.PANDORABOTS, "b0dafd24ee35a477").CreateSession();
        }

        public string Think(string message)
        {
            try
            {
                return _chatterBot.Think(message);
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}
