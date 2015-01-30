using System;

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

            
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run(new Console());

        }
    }
}
