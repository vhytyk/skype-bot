using System;
using System.Diagnostics;
using SkypeBot.BotEngine;

namespace SkypeBot
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //SkypeBot.Configuration.CoreAppXmlConfiguration.Instance.LoggerFactory = new StreamLoggerFactory();
            UnityConfiguration.Instance.RegisterTypes();

            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run(new Console());

        }
    }
}
