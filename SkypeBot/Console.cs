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
                    outputBox.SelectionStart = 0;
                }));
            }));
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            _botCoreService.InitSkype();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            
        }
    }
}
