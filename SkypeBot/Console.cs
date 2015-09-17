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
using SkypeBot.BotEngine.Commands;
using SkypeBot.BotEngine.EngineImplementations._7._0;
using SkypeBot.SkypeDB.SkypeDalImplementations;
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
            MessageBox.Show(new KursCommand().RunCommand());
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
            //new SkypeDal7().GetAllContacts().ForEach(x => Debug.WriteLine("{0}: {1}",x.Name, x.IsAuthorized));
            new SkypeSendMessageService70(new SkypeInitService70()).AcceptContact("nauroskype");
            //System.Console.WriteLine(e.ToString());
            //var service = new RmqSkypeService();
            //service.PushMessage(new RmqSkypeMessage(){Conversation = "ivan.sokolovich.prl", Message = "You were added on CR: http://crucible/cru/CR-248"});
            //service.PushMessage(new RmqSkypeMessage() { Conversation = "tkonyk.ja", Message = "You were added on CR: http://crucible/cru/CR-248" });
            ///*RmqSkypeMessage message = service.PullMessage();
            //Debug.WriteLine(message.Conversation);
            //Debug.WriteLine(message.Message);*/
            //RmqListener listener = new RmqListener();
            //listener.SkypeMessageReceived += listener_SkypeMessageReceived;
            //listener.Initialize();
            ////new WindowTree(UnityConfiguration.Instance.Reslove<ISkypeInitService>().GetMainWindow()).ShowDialog();
        }

        void listener_SkypeMessageReceived(string source, string message)
        {
            Debug.WriteLine(source);
            Debug.WriteLine(message);
        }

        private void toolStripButton1_Click(object sender, EventArgs e) { }
    }
}
