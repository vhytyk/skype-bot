using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using SkypeBot.SkypeDB;

namespace SkypeBot.BotEngine.EngineImplementations._7._0
{
    public class SkypeListener70 : ISkypeListener, IDisposable
    {
        private readonly IDictionary<string, long> _lastMessageIds = new ConcurrentDictionary<string, long>();
        private Timer _readTimer;
        public event SkypeMessageHandler SkypeMessageReceived;
        public event FoundContactHandler FoundNewContact;
        private List<string> acceptedList = new List<string>();

        private void OnSkypeMessageReceived(string source, SkypeMessage message)
        {
            if (null != SkypeMessageReceived)
            {
                SkypeMessageReceived(source, message);
            }
        }
        private void OnFoundNewContact(string contact)
        {
            if (null != FoundNewContact)
            {
                FoundNewContact(contact);
            }
        }
        private readonly object _locker = new object();

        private void ScanConversations(bool raiseEvents = true)
        {
            lock (_locker)
            {
                using (var skypeDal = UnityConfiguration.Instance.Reslove<ISkypeDal>())
                {
                    skypeDal.GetAllConversations().ForEach(conversation =>
                    {
                        long lastMessageId = _lastMessageIds.ContainsKey(conversation.Name)
                            ? _lastMessageIds[conversation.Name]
                            : -1;
                        skypeDal.GetLastMessages(lastMessageId, conversation.Id).ForEach(message =>
                        {
                            if (raiseEvents && message.Author != ConfigurationManager.AppSettings["botSkypeName"])
                            {
                                OnSkypeMessageReceived(conversation.DisplayName, message);
                            }
                            _lastMessageIds[conversation.Name] = message.Id;
                        });
                    });
                    
                    //skypeDal.GetAllContacts().ForEach(contact =>
                    //{
                    //    if (!acceptedList.Contains(contact.Name) 
                    //        && !contact.IsAuthorized && contact.Name != ConfigurationManager.AppSettings["botSkypeName"])
                    //    {
                    //        if (raiseEvents)
                    //        {
                    //            acceptedList.Add(contact.Name);
                    //            OnFoundNewContact(contact.Name);
                    //        }
                    //    }
                    //});
                }
            }
        }

        public void Initialize()
        {
            ScanConversations(false);
            _readTimer = new Timer(Scan, null, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2));
        }

        private void Scan(object state)
        {
            ScanConversations();
        }

        public void Dispose()
        {
            if (_readTimer != null)
            {
                _readTimer.Dispose();
                _readTimer = null;
            }
        }
    }
}
