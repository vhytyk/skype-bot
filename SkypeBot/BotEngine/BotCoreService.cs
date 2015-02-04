using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Automation;
using ChatterBotAPI;

namespace SkypeBot.BotEngine
{
    public class BotCoreService : IBotCoreService
    {
        private readonly ISkypeInitService _initService;
        private readonly ISkypeSendMessageService _sendMessageService;
        private readonly ISkypeListener _skypeListener;
        private readonly Queue<SkypeMessage> _skypeMessages = new Queue<SkypeMessage>();
        private Timer _processTimer;
        private ChatterBotSession _chatterBot = null;
        public BotCoreService(ISkypeInitService initService, ISkypeSendMessageService sendMessageService, ISkypeListener skypeListener)
        {
            _initService = initService;
            _sendMessageService = sendMessageService;
            _skypeListener = skypeListener;
            ChatterBotFactory factory = new ChatterBotFactory();

            _chatterBot = factory.Create(ChatterBotType.CLEVERBOT).CreateSession();
        }

        public void InitSkype()
        {
            _initService.Initialize();
            _skypeListener.Initialize();
            _skypeListener.SkypeMessageReceived += _skypeListener_SkypeMessageReceived;
            _processTimer = new Timer(ProcessQueue, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
        }

        private void ProcessQueue(object state)
        {
            lock (locker)
            {
                if (_skypeMessages.Count == 0)
                {
                    return;
                }

                SkypeMessage message = _skypeMessages.Dequeue();
                _sendMessageService.SendMessage(message.Contact, message.Message);
            }
        }

        void _skypeListener_SkypeMessageReceived(string source, string message)
        {
            Debug.WriteLine("Message received from {0}: {1}", source, message);
            string contact =
                Regex.Matches(source, @"[#\$]([\w.]+)[/;]")
                    .Cast<Match>()
                    .FirstOrDefault(m => !m.Groups[1].Value.Contains(ConfigurationManager.AppSettings["botSkypeName"]))
                    .Groups[1].Value;
            string response = _chatterBot.Think(message);
            SendMessage(contact, response);
        }

        public void SendMessage(string contact, string message)
        {
            AddMessageToQueue(new SkypeMessage()
            {
                Contact = contact,
                Message = message
            });
        }
        private object locker = new object();
        void AddMessageToQueue(SkypeMessage message)
        {
            lock (locker)
            {
                _skypeMessages.Enqueue(message);
            }
        }

        public void Dispose()
        {
            if (_processTimer != null)
            {
                _processTimer.Dispose();
                _processTimer = null;
            }
        }
    }

    public struct SkypeMessage
    {
        public string Contact { get; set; }
        public string Message { get; set; }
    }
}
