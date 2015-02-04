using SKYPE4COMLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Automation;

namespace SkypeBot.BotEngine.EngineImplementations._7._0
{
    public class SkypeListener70 : ISkypeListener
    {
        private readonly ISkypeInitService _initService;

        public SkypeListener70(ISkypeInitService initService)
        {
            _initService = initService;
        }

        public event SkypeMessageHandler SkypeMessageReceived;

        private void OnSkypeMessageReceived(string source, string message)
        {
            if (null != SkypeMessageReceived)
            {
                SkypeMessageReceived(source, message);
            }
        }

        private Skype skype = null;

        public void Initialize()
        {

            skype = new Skype();

            skype.Attach(Wait: false);
            Thread.Sleep(5000);

            var allowAccessButton = _initService.GetMainWindow()
                .FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Allow access"));
            if (allowAccessButton != null)
            {
                (allowAccessButton.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern).Invoke();
                Debug.WriteLine("Allowing access to our app");
            }

            skype.MessageStatus += skype_MessageStatus;


        }

        void skype_MessageStatus(ChatMessage pMessage, TChatMessageStatus Status)
        {
            if (Status == TChatMessageStatus.cmsReceived)
            {
                OnSkypeMessageReceived(pMessage.ChatName, pMessage.Body);
            }
        }
    }
}
