using System;
using System.Collections.Generic;
using System.Threading;

namespace SkypeBot.BotEngine
{
    public class BotCoreService : IBotCoreService
    {
        private readonly ISkypeInitService _initService;
        private readonly ISkypeSendMessageService _sendMessageService;
        private readonly ISkypeListener _skypeListener;
        private readonly IRmqListener _rmqListener;
        private readonly IHandleMessageService _handeMessageService;
        private readonly Queue<SkypeMessage> _skypeMessages = new Queue<SkypeMessage>();
        private Timer _processTimer;
        
        public BotCoreService(ISkypeInitService initService, ISkypeSendMessageService sendMessageService, ISkypeListener skypeListener, IHandleMessageService handeMessageService, IRmqListener rmqListener)
        {
            _initService = initService;
            _sendMessageService = sendMessageService;
            _skypeListener = skypeListener;
            _handeMessageService = handeMessageService;
            _rmqListener = rmqListener;
        }

        public void InitSkype()
        {
            _initService.Initialize(() =>
            {
                _rmqListener.Initialize();
                _skypeListener.Initialize();
                _rmqListener.SkypeMessageReceived += _rmqListener_SkypeMessageReceived;
                _skypeListener.SkypeMessageReceived += _skypeListener_SkypeMessageReceived;
                _processTimer = new Timer(ProcessQueue, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            });
        }

        void _rmqListener_SkypeMessageReceived(string source, string message)
        {
            SendMessage(source, message);
        }

        private void ProcessQueue(object state)
        {
            lock (_locker)
            {
                if (_skypeMessages.Count == 0)
                {
                    return;
                }

                SkypeMessage message = _skypeMessages.Dequeue();
                _sendMessageService.SendMessage(message.Contact, message.Message);
            }
        }

        private void _skypeListener_SkypeMessageReceived(string source, string message)
        {
            _handeMessageService.HandleIncomeMessage(source, message, SendMessage);
        }

        public void SendMessage(string contact, string message)
        {
            AddMessageToQueue(new SkypeMessage
            {
                Contact = contact,
                Message = message
            });
        }
        private readonly object _locker = new object();
        void AddMessageToQueue(SkypeMessage message)
        {
            lock (_locker)
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
