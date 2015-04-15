using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChatterBotAPI;

namespace SkypeBot.BotEngine
{
    public class ChatBotProvider : IChatBotProvider
    {
        private DateTime lastBotResponse = DateTime.MinValue;

        private ChatterBotSession _chatterBot = null;

        private void InitBot()
        {
            _chatterBot = new ChatterBotFactory().Create(ChatterBotType.PANDORABOTS, "b0dafd24ee35a477").CreateSession();
        }

        public ChatBotProvider()
        {
            InitBot();
        }

        public string Think(string message)
        {
            try
            {
                if ((DateTime.Now - lastBotResponse).TotalMinutes > 5)
                {
                    InitBot();
                }
                return _chatterBot.Think(message);
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}
