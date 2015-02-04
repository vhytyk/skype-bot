using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace SkypeBot.BotEngine
{
    public static class AutomationExtensions
    {
        public static void ButtonClick(this AutomationElement window, string name)
        {
            AutomationElement element =
                window.FindAll(TreeScope.Descendants, Condition.TrueCondition)
                    .Cast<AutomationElement>()
                    .FirstOrDefault(el => el.Current.ControlType == ControlType.Button && el.Current.Name == name);
            InvokePattern pattern = element.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
            pattern.Invoke();
        }

        public static void SetValue(this AutomationElement window, ControlType type, string name, string value)
        {
            AutomationElement element =
                window.FindAll(TreeScope.Descendants, Condition.TrueCondition)
                    .Cast<AutomationElement>()
                    .FirstOrDefault(el => el.Current.ControlType == type && el.Current.Name == name);
            ValuePattern pattern = element.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
            pattern.SetValue(value);
        }

        public static AutomationElement GetElementByName(this AutomationElement element, string name)
        {
            return element.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, name));
        }

        public static void LogPatterns(this AutomationElement element)
        {
            AutomationPattern[] patterns = element.GetSupportedPatterns();
            Debug.WriteLine(string.Join(",", patterns.Select(p => p.ProgrammaticName)));
        }
    }
}
