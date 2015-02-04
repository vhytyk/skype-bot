using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using SkypeBot.BotEngine.EngineImplementations;
using SkypeBot.InputDevices;
using SkypeBot.WindowsAPI;
using System.Windows;

namespace SkypeBot.BotEngine
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
    }
}
