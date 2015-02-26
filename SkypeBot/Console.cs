using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SkypeBot.BotEngine;
using SkypeBot.BotEngine.EngineImplementations._7._0;
using SkypeBotRMQ;

namespace SkypeBot
{
    public partial class Console : Form
    {
        IBotCoreService _botCoreService = UnityConfiguration.Instance.Reslove<IBotCoreService>();
        public Console()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Debug.Listeners.Add(new ConsoleListener(message =>
            {
                Invoke(new MethodInvoker(() =>
                {
                    outputBox.Text = message + outputBox.Text;
                    if (outputBox.Text.Length > 1000)
                    {
                        outputBox.Text = outputBox.Text.Substring(0, 500);
                    }
                    outputBox.SelectionStart = 0;
                }));
            }));
            _botCoreService.InitSkype();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            _botCoreService.Dispose();
            Process.GetCurrentProcess().Kill();
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            var service = new RmqSkypeService();
            service.PushMessage(new RmqSkypeMessage(){Conversation = "test", Message = "lalala"});
            RmqSkypeMessage message = service.PullMessage();
            Debug.WriteLine(message.Conversation);
            Debug.WriteLine(message.Message);
            //new WindowTree(UnityConfiguration.Instance.Reslove<ISkypeInitService>().GetMainWindow()).ShowDialog();
        }
    }
}
