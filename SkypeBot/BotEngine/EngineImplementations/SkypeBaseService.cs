using System;
using System.Diagnostics;
using System.Threading;
using SkypeBot.InputDevices;
using SkypeBot.WindowsAPI;

namespace SkypeBot.BotEngine.EngineImplementations
{
    public class SkypeBaseService
    {
        protected void Process(Action invokeAction)
        {
            new Thread(() => ProcessInThread(invokeAction)).Start();
        }

        protected void ProcessInThread(Action invokeAction)
        {
            try
            {
                if (null != invokeAction)
                {
                    invokeAction();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.LogError("Error: " + ex +
                                (ex.InnerException != null ? "; " + ex.InnerException.Message : ""));
            }
        }

        protected void SelectAllAndRemove()
        {
            Keyboard.Instance.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
            Keyboard.Instance.Send("a");
            Keyboard.Instance.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
            Keyboard.Instance.PressSpecialKey(KeyboardInput.SpecialKeys.DELETE);
        }

    }
}
