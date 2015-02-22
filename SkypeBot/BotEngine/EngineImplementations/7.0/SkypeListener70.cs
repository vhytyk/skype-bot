using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using SkypeCore;

namespace SkypeBot.BotEngine.EngineImplementations._7._0
{
    public class SkypeListener70 : ISkypeListener, IDisposable
    {
        private IDictionary<string, long> _lastMessageIds = new ConcurrentDictionary<string, long>();
        private Timer _readTimer = null;
        private SkypeDAL _skypeDal = null;
        public event SkypeMessageHandler SkypeMessageReceived;

        private void OnSkypeMessageReceived(string source, string message)
        {
            if (null != SkypeMessageReceived)
            {
                SkypeMessageReceived(source, message);
            }
        }
        private object _locker = new object();
        private void ScanConversations(bool raiseEvents = true)
        {
            lock (_locker)
            {
                _skypeDal.GetAllConversations().ForEach(conversation =>
                {
                    long lastMessageId = _lastMessageIds.ContainsKey(conversation.Name)
                        ? _lastMessageIds[conversation.Name]
                        : -1;
                    _skypeDal.GetLastMessages(lastMessageId, conversation.Id).ForEach(message =>
                    {
                        if (raiseEvents && message.Author != ConfigurationManager.AppSettings["botSkypeName"])
                        {
                            OnSkypeMessageReceived(conversation.DisplayName, message.Message);
                        }
                        _lastMessageIds[conversation.Name] = message.Id;
                    });
                });
            }
        }

        public void Initialize()
        {
            _skypeDal = new SkypeDAL(ConfigurationManager.AppSettings["botSkypeName"]);
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
