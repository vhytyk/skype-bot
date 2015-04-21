using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace SkypeBot
{
    public static class ErrorLog
    {
        public static void LogError(string error, params object[] args)
        {
            Debug.WriteLine(error, args);
            File.AppendAllText("error.log", string.Format(error, args));
        }
    }
}
