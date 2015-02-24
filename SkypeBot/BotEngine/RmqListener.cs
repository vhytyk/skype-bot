using System;
using System.Diagnostics;
using System.Threading;
using SkypeBotRMQ;

namespace SkypeBot.BotEngine
{
    public class RmqListener : IRmqListener
    {
        private Timer _timer;
        private bool pulling;
        private void PullFromRmq(object state)
        {
            if (pulling)
            {
                return;
            }
            try
            {
                pulling = true;
                var service = UnityConfiguration.Instance.Reslove<IRmqSkypeService>();
                RmqSkypeMessage message = service.PullMessage();
                if (message != null)
                {
                    OnSkypeMessageReceived(message.Conversation, message.Message);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                pulling = false;
            }
        }

        private void OnSkypeMessageReceived(string source, string message)
        {
            if (null != SkypeMessageReceived)
            {
                SkypeMessageReceived(source, message);
            }
        }

        public void Initialize()
        {
            _timer = new Timer(PullFromRmq, null, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2));
        }

        public event SkypeMessageHandler SkypeMessageReceived;

        public void Dispose()
        {
            if (null != _timer)
            {
                _timer.Dispose();
                _timer = null;
            }
        }
    }
}
