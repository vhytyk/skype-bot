using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace SkypeBot
{
    /// <summary>
    /// Represents a process which contains windows.
    /// </summary>
    public class Application : IDisposable
    {
        private readonly Process process;

        protected Application()
        {
        }

        private Application(Process process)
        {
            this.process = process;
        }

        /// <summary>
        /// Runs the process identified by the executable and creates Application object for this executable
        /// </summary>
        /// <param name="executable">Path to the executable</param>
        /// <exception cref="ArgumentNullException">No process info passed</exception>
        /// <exception cref="WhiteException">White Failed to Launch or Attached to process</exception>
        public static Application Launch(string executable)
        {
            var processStartInfo = new ProcessStartInfo(executable);
            return Launch(processStartInfo);
        }

        /// <summary>
        /// Lauches the process and creates and Application object for it
        /// </summary>
        /// <exception cref="ArgumentNullException">No process info passed</exception>
        /// <exception cref="WhiteException">White Failed to Launch or Attached to process</exception>
        public static Application Launch(ProcessStartInfo processStartInfo)
        {
            if (string.IsNullOrEmpty(processStartInfo.WorkingDirectory)) processStartInfo.WorkingDirectory = ".";
            Process process;
            try
            {
                process = Process.Start(processStartInfo);
            }
            catch (Win32Exception ex)
            {
                var error =
                    string.Format(
                        "[Failed Launching process:{0}] [Working directory:{1}] [Process full path:{2}] [Current Directory:{3}]",
                        processStartInfo.FileName,
                        new DirectoryInfo(processStartInfo.WorkingDirectory).FullName,
                        new FileInfo(processStartInfo.FileName).FullName,
                        Environment.CurrentDirectory);
                throw new Exception(error, ex);
            }
            return Attach(process);
        }

        /// <summary>
        /// Attaches White to an existing process by process id 
        /// </summary>
        /// <exception cref="WhiteException">White Failed to Attach to process</exception>
        public static Application Attach(int processId)
        {
            Process process;
            try
            {
                process = Process.GetProcessById(processId);
            }
            catch (ArgumentException e)
            {
                throw new Exception("Could not find process with id: " + processId, e);
            }
            return new Application(process);
        }

        /// <summary>
        /// Attaches White to an existing process
        /// </summary>
        /// <exception cref="WhiteException">White Failed to Attach to process</exception>
        public static Application Attach(Process process)
        {
            return new Application(process);
        }

        /// <summary>
        /// Attaches with existing process
        /// </summary>
        /// <exception cref="WhiteException">White Failed to Attach to process with specified name</exception>
        public static Application Attach(string executable)
        {
            Process[] processes = Process.GetProcessesByName(executable);
            if (processes.Length == 0) throw new Exception("Could not find process named: " + executable);
            return Attach(processes[0]);
        }

        /// <summary>
        /// Attaches to the process if it is running or launches a new process
        /// </summary>
        /// <param name="processStartInfo"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="WhiteException">White Failed to Launch or Attach to process</exception>
        public static Application AttachOrLaunch(ProcessStartInfo processStartInfo)
        {
            string processName = ReplaceLast(processStartInfo.FileName, ".exe", string.Empty);
            processName = Path.GetFileName(processName);
            Process[] processes = Process.GetProcessesByName(processName);
            if (processes.Length == 0) return Launch(processStartInfo);
            return Attach(processes[0]);
        }

        private static string ReplaceLast(string replaceIn, string replace, string with)
        {
            int index = replaceIn.LastIndexOf(replace);
            if (index == -1) return replaceIn;
            return replaceIn.Substring(0, index) + with + replaceIn.Substring(index + replace.Length);
        }

        /// <summary>
        /// Name of the process
        /// </summary>
        public virtual string Name
        {
            get { return process.ProcessName; }
        }

        /// <summary>
        /// Kills the applications and waits till it is closed
        /// </summary>
        public virtual void Kill()
        {
            try
            {
                if (Process.HasExited) return;
                process.Kill();
                process.WaitForExit();
                Process.Dispose();
            }
            catch { }
        }
        /// <summary>
        /// Returns whether process has exited
        /// </summary>
        public virtual bool HasExited
        {
            get { return process.HasExited; }
        }

        /// <summary>
        /// Two applications are equal if they have the same process
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            var application = obj as Application;
            if (application == null) return false;
            return Equals(process, application.process);
        }

        public override int GetHashCode()
        {
            return process.GetHashCode();
        }

        public virtual void Dispose()
        {
            Kill();
        }


        public virtual Process Process
        {
            get { return process; }
        }

        /// <summary>
        /// Kills the application. Read Application.Kill.
        /// It also saves the application test execution state. This saves the position of the window UIItems which would be loaded next time
        /// automatically for improved performance. You would need to use InitializedOption.AndIdentifiedBy for specifying the identification of window.
        /// </summary>
        public virtual void KillAndSaveState()
        {
            Kill();
        }
    }
}