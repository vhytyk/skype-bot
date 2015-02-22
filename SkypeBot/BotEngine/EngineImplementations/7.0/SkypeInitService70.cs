﻿using SkypeBot.BotEngine.EngineImplementations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace SkypeBot.BotEngine
{
    public class SkypeInitService70 : SkypeBaseService, ISkypeInitService
    {
        public int X { get; set; }

        private Application application = null;
        private object lockObject = new object();

        public void Initialize(Action afterInitAction = null)
        {
            Process(() =>
            {
                lock (lockObject)
                {
                    string userName = ConfigurationManager.AppSettings["botSkypeName"];
                    string password = ConfigurationManager.AppSettings["botSkypePassword"];

                    cachedMainWindow =
                        AutomationElement.RootElement.FindAll(TreeScope.Children, Condition.TrueCondition)
                            .Cast<AutomationElement>()
                            .FirstOrDefault(el => el.Current.Name.Contains(userName));

                    if (cachedMainWindow != null)
                    {
                        Debug.WriteLine("attaching to skype");
                        application = Application.Attach(cachedMainWindow.Current.ProcessId);
                    }
                    else
                    {
                        Debug.WriteLine("running skype");
                        application = Application.Launch(
                            new ProcessStartInfo
                            {
                                FileName = @"C:\Program Files (x86)\Skype\Phone\Skype.exe",
                                Arguments = " /secondary"
                            }
                            );
                    }
                    if (GetMainWindow() == null)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            while (GetMainLoginWindow() == null)
                            {
                            }
                        }).Wait(TimeSpan.FromSeconds(10));
                        LogIn(userName, password);
                    }
                    Task.Factory.StartNew(() =>
                    {
                        while (GetMainWindow() == null)
                        {
                        }
                    }).Wait(TimeSpan.FromSeconds(10));

                    (GetMainWindow().GetCurrentPattern(WindowPattern.Pattern) as WindowPattern).SetWindowVisualState(WindowVisualState.Normal);

                    Debug.WriteLine("skype is ready");
                    if (afterInitAction != null)
                    {
                        afterInitAction();
                    }

                }
            });

        }


        private void LogIn(string username, string password)
        {
            AutomationElement mainWindow = GetMainLoginWindow();
            (mainWindow.GetCurrentPattern(WindowPattern.Pattern) as WindowPattern).SetWindowVisualState(WindowVisualState.Normal);
            Debug.Write(string.Format("loggin in, username: {0}...", username));

            mainWindow.SetValue(ControlType.ComboBox, "Skype Name", username);
            mainWindow.SetValue(ControlType.Edit, "Password", password);
            mainWindow.ButtonClick("Sign in");


            Debug.WriteLine("done");
        }

        internal AutomationElement GetMainLoginWindow()
        {
            AndCondition andcondition = new AndCondition(new PropertyCondition(AutomationElement.ProcessIdProperty, application.Process.Id),
                new PropertyCondition(AutomationElement.ClassNameProperty, "TLoginForm"));
            return AutomationElement.RootElement.FindFirst(TreeScope.Descendants, andcondition);
        }



        private AutomationElement cachedMainWindow;
        public AutomationElement GetMainWindow()
        {
            if (cachedMainWindow != null)
            {
                return cachedMainWindow;
            }
            AndCondition andcondition = new AndCondition(new PropertyCondition(AutomationElement.ProcessIdProperty, application.Process.Id),
                new PropertyCondition(AutomationElement.ClassNameProperty, "tSkMainForm"));
            cachedMainWindow = AutomationElement.RootElement.FindFirst(TreeScope.Descendants, andcondition);
            return cachedMainWindow;
        }
    }
}