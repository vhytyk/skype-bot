using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkypeBot.BotEngine
{
    public class BotCoreService : IBotCoreService
    {
        private readonly ISkypeInitService _initService;
        private readonly ISkypeSendMessageService _sendMessageService;

        public BotCoreService(ISkypeInitService initService, ISkypeSendMessageService sendMessageService)
        {
            _initService = initService;
            _sendMessageService = sendMessageService;
        }

        public void InitSkype()
        {
            _initService.Initialize();
        }

        public void SendMessage(string contact, string message)
        {
            _sendMessageService.SendMessage(contact, message);
        }
    }
}
