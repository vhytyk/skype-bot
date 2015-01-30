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

namespace SkypeBot
{
    public partial class Console : Form
    {
        IBotCoreService _botCoreService = new BotCoreService(new SkypeInitService70(), new SkypeSendMessageService70(new SkypeInitService70()));
        public Console()
        {
            InitializeComponent();
            System.Console.SetOut(new StreamWriter("blabla.log"));
            Debug.Listeners.Add(new ConsoleListener(message =>
            {
                Invoke(new MethodInvoker(() =>
                {
                    outputBox.Text += message;
                    outputBox.SelectionStart = outputBox.Text.Length - 1;
                }));
            }));
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            _botCoreService.SendMessage("viktoriia532", "hello world!");
        }
    }
}
