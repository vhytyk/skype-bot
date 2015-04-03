using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using SkypeBot.InputDevices;
using SkypeBot.WindowsAPI;

namespace SkypeBot.BotEngine.EngineImplementations._7._0
{
    public class SkypeSendMessageService70: SkypeBaseService, ISkypeSendMessageService
    {
        private readonly ISkypeInitService _initService;

        public SkypeSendMessageService70(ISkypeInitService initService)
        {
            _initService = initService;
        }

        public void SendMessage(string contact, string message)
        {
            _initService.Initialize(() =>
            {
                Debug.WriteLine("searching for contact: " + contact);
                Mouse.Instance.Click(_initService.GetMainWindow().FindFirst(TreeScope.Descendants,
                    new PropertyCondition(AutomationElement.ClassNameProperty, "TSearchControl")).GetClickablePoint());
                SelectAllAndRemove();
                Keyboard.Instance.Send(contact);
                
                Mouse.Instance.Click(_initService.GetMainWindow().GetElementByName("Contacts")
                    .FindFirst(TreeScope.Children, Condition.TrueCondition).GetClickablePoint());
                Debug.WriteLine("sending message to contact: " + contact);
                Point point = _initService.GetMainWindow().GetElementByName("Insert emoticon").GetClickablePoint();
                point.X -= 50;
                Mouse.Instance.Click(point);
                SelectAllAndRemove();
                Keyboard.Instance.Send(message);
                Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
            });
        }


        public void AcceptContact(string contact)
        {
            _initService.Initialize(() =>
            {
                Debug.WriteLine("searching for contact to add: " + contact);
                Mouse.Instance.Click(_initService.GetMainWindow().FindFirst(TreeScope.Descendants,
                    new PropertyCondition(AutomationElement.ClassNameProperty, "TSearchControl")).GetClickablePoint());
                SelectAllAndRemove();
                Keyboard.Instance.Send(contact);

                Mouse.Instance.Click(_initService.GetMainWindow().GetElementByName("Contacts")
                    .FindFirst(TreeScope.Children, Condition.TrueCondition).GetClickablePoint());
                Thread.Sleep(3000);
                AutomationElement acceptElement = _initService.GetMainWindow()
                    .FindFirst(TreeScope.Descendants,
                        new PropertyCondition(AutomationElement.NameProperty, "Accept"));
                if (null != acceptElement)
                {
                    Debug.WriteLine("accepting contact: "+contact);
                    Mouse.Instance.Click(acceptElement.GetClickablePoint());
                }

            });
        }
    }
}
