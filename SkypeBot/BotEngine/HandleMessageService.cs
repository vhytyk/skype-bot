using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ChatterBotAPI;

namespace SkypeBot.BotEngine
{
    public class HandleMessageService : IHandleMessageService
    {
        private ChatterBotSession _chatterBot = null;

        public HandleMessageService()
        {
            _chatterBot = new ChatterBotFactory().Create(ChatterBotType.PANDORABOTS, "b0dafd24ee35a477").CreateSession();
        }
        public void HandleIncomeMessage(string source, string message, Action<string, string> responseAction)
        {
            Debug.WriteLine("Message received from {0}: {1}", source, message);
            string response = _chatterBot.Think(message);
            responseAction(source, response);
        }
    }
}
