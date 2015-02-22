using System;
using System.Windows.Automation;

namespace SkypeBot.BotEngine
{
    public interface ISkypeInitService
    {
        void Initialize(Action afterInitAction = null);
        AutomationElement GetMainWindow();
        int X { get; set; }
    }
}
