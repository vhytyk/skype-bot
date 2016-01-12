using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using SkypeBot.InputDevices;
using SkypeBot.WindowsAPI;

namespace SkypeBot.BotEngine.EngineImplementations._7._0
{
    public class SkypeSendMessageService7_17: SkypeSendMessageService70
    {
        public SkypeSendMessageService7_17(ISkypeInitService initService) : base(initService)
        {
        }

        public override void SendMessage(string contact, string message)
        {
            _initService.Initialize(() =>
            {
                Mouse.Instance.Click(_initService.GetMainWindow().FindFirst(TreeScope.Descendants,
                    new PropertyCondition(AutomationElement.ClassNameProperty, "TSearchControl")).GetClickablePoint());
                SelectAllAndRemove();
                Keyboard.Instance.Send(contact);

                Mouse.Instance.Click(_initService.GetMainWindow().GetElementByName("Contacts")
                    .FindFirst(TreeScope.Children, Condition.TrueCondition).GetClickablePoint());
                Point point =
                    _initService.GetMainWindow()
                        .FindFirst(TreeScope.Descendants,
                            new PropertyCondition(AutomationElement.ClassNameProperty, "TChatEntryControl")).GetClickablePoint();
                point.Y += 20;
                
                Mouse.Instance.Click(point);
                //Thread.Sleep(500);
                SelectAllAndRemove();
                Keyboard.Instance.Send(message);
                Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.RETURN);
            });
        }
    }
}
