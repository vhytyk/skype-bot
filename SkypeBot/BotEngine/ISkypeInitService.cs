using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace SkypeBot.BotEngine
{
    public interface ISkypeInitService
    {
        void Initialize(Action afterInitAction = null);
        AutomationElement GetMainWindow();
    }
}
