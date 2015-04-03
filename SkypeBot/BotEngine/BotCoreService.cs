using System;
using System.Collections.Generic;
using System.Threading;
using SkypeBot.SkypeDB;

namespace SkypeBot.BotEngine
{
    public class BotCoreService : IBotCoreService
    {
        private readonly ISkypeInitService _initService;
        private readonly ISkypeSendMessageService _sendMessageService;
        private readonly ISkypeListener _skypeListener;
        private readonly IRmqListener _rmqListener;
        private readonly IHandleMessageService _handeMessageService;
        private readonly Queue<SkypeAction> _skypeActions = new Queue<SkypeAction>();
        
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
                _skypeListener.FoundNewContact += _skypeListener_FoundNewContact;
                _processTimer = new Timer(ProcessQueue, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            });
        }

        void _skypeListener_FoundNewContact(string source)
        {
            AddActionToQueue(new SkypeAction
            {
                ActionType = SkypeActionType.AcceptContact,
                Contact = source
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
                if (_skypeActions.Count == 0)
                {
                    return;
                }

                SkypeAction message = _skypeActions.Dequeue();
                switch (message.ActionType)
                {
                    case SkypeActionType.SendMessage:
                        _sendMessageService.SendMessage(message.Contact, message.Message);
                        break;
                    case SkypeActionType.AcceptContact:
                        _sendMessageService.AcceptContact(message.Contact);
                        break;
                }
            }
        }

        private void _skypeListener_SkypeMessageReceived(string source, string message)
        {
            _handeMessageService.HandleIncomeMessage(source, message, SendMessage);
        }

        public void SendMessage(string contact, string message)
        {
            AddActionToQueue(new SkypeAction
            {
                ActionType = SkypeActionType.SendMessage,
                Contact = contact,
                Message = message
            });
        }
        private readonly object _locker = new object();
        void AddActionToQueue(SkypeAction message)
        {
            lock (_locker)
            {
                _skypeActions.Enqueue(message);
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

    public enum SkypeActionType { SendMessage, AcceptContact}

    public struct SkypeAction
    {
        public SkypeActionType ActionType { get; set; }
        public string Contact { get; set; }
        public string Message { get; set; }
    }
}
