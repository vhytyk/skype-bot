using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace SkypeBot.BotEngine.EngineImplementations._7._0
{
    public class SkypeInitService70 : SkypeBaseService, ISkypeInitService
    {
        public int X { get; set; }

        private Application _application;
        private readonly object _lockObject = new object();

        public void Initialize(Action afterInitAction = null)
        {
            Process(() =>
            {
                lock (_lockObject)
                {
                    string userName = ConfigurationManager.AppSettings["botSkypeName"];
                    string password = ConfigurationManager.AppSettings["botSkypePassword"];

                    _cachedMainWindow =
                        AutomationElement.RootElement.FindAll(TreeScope.Children, Condition.TrueCondition)
                            .Cast<AutomationElement>()
                            .FirstOrDefault(el => el.Current.Name.Contains(userName));

                    if (_cachedMainWindow != null)
                    {
                        Debug.WriteLine("attaching to skype");
                        _application = Application.Attach(_cachedMainWindow.Current.ProcessId);
                    }
                    else
                    {
                        Debug.WriteLine("running skype");
                        _application = Application.Launch(
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

                    var windowPattern = GetMainWindow().GetCurrentPattern(WindowPattern.Pattern) as WindowPattern;
                    if (windowPattern != null)
                        windowPattern.SetWindowVisualState(WindowVisualState.Normal);

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
            var windowPattern = mainWindow.GetCurrentPattern(WindowPattern.Pattern) as WindowPattern;
            if (windowPattern != null)
                windowPattern.SetWindowVisualState(WindowVisualState.Normal);
            Debug.Write(string.Format("loggin in, username: {0}...", username));

            mainWindow.SetValue(ControlType.ComboBox, "Skype Name", username);
            mainWindow.SetValue(ControlType.Edit, "Password", password);
            mainWindow.ButtonClick("Sign in");


            Debug.WriteLine("done");
        }

        internal AutomationElement GetMainLoginWindow()
        {
            var andcondition = new AndCondition(new PropertyCondition(AutomationElement.ProcessIdProperty, _application.Process.Id),
                new PropertyCondition(AutomationElement.ClassNameProperty, "TLoginForm"));
            return AutomationElement.RootElement.FindFirst(TreeScope.Descendants, andcondition);
        }



        private AutomationElement _cachedMainWindow;
        public AutomationElement GetMainWindow()
        {
            if (_cachedMainWindow != null)
            {
                return _cachedMainWindow;
            }
            var andcondition = new AndCondition(new PropertyCondition(AutomationElement.ProcessIdProperty, _application.Process.Id),
                new PropertyCondition(AutomationElement.ClassNameProperty, "tSkMainForm"));
            _cachedMainWindow = AutomationElement.RootElement.FindFirst(TreeScope.Descendants, andcondition);
            return _cachedMainWindow;
        }
    }
}
