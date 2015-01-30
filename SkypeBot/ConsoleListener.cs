using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SkypeBot
{
    public class ConsoleListener : TraceListener
    {
        private readonly WriteDelegate _writeDelegate;

        public delegate void WriteDelegate(string message);

        public ConsoleListener(WriteDelegate writeDelegate)
        {
            if (writeDelegate == null)
            {
                throw new ArgumentException("Cannot be null", "writeDelegate");
            }
            _writeDelegate = writeDelegate;

        }

        private bool lastNewLine = true;
        string HandleMessage(string message, bool newLine = false)
        {
            string result = string.Empty;
            if (!lastNewLine)
            {
                result = string.Format("{0}{1}", message,
                    newLine ? Environment.NewLine : string.Empty);

            }
            else
            {
                result = string.Format("{0}: {1}{2}", DateTime.Now.ToLocalTime(), message,
                    newLine ? Environment.NewLine : string.Empty);
            }

            lastNewLine = newLine;

            return result;

        }
        public override void Write(string message)
        {
            _writeDelegate(HandleMessage(message));
        }

        public override void WriteLine(string message)
        {
            _writeDelegate(HandleMessage(message, true));
        }
    }
}
