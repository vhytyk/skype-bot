using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Threading;
using SkypeBot.WindowsAPI;

namespace SkypeBot.InputDevices
{
    public class Mouse  
    {
        [DllImport("user32", EntryPoint = "SendInput")]
        static extern int SendInput(uint numberOfInputs, ref Input input, int structSize);

        [DllImport("user32", EntryPoint = "SendInput")]
        static extern int SendInput64(int numberOfInputs, ref Input64 input, int structSize);

        [DllImport("user32.dll")]
        static extern IntPtr GetMessageExtraInfo();

        [DllImport("user32.dll")]

        static extern bool GetCursorPos(ref Point cursorInfo);

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        static extern bool GetCursorInfo(ref CursorInfo cursorInfo);

        [DllImport("user32.dll")]
        static extern short GetDoubleClickTime();

        [DllImport("user32.dll")]
        static extern int GetSystemMetrics(SystemMetric smIndex);

        public static Mouse Instance = new Mouse();
        DateTime lastClickTime = DateTime.Now;
        readonly short doubleClickTime = GetDoubleClickTime();
        System.Windows.Point lastClickLocation;
        const int ExtraMillisecondsBecauseOfBugInWindows = 13;

        Mouse() { }

        public virtual System.Windows.Point Location
        {
            get
            {
                var point = new Point();
                GetCursorPos(ref point);
                return new System.Windows.Point(point.X, point.Y);
            }
            set
            {
                SetCursorPos((int)value.X, (int)value.Y);
            }
        }

        public virtual MouseCursor Cursor
        {
            get
            {
                CursorInfo cursorInfo = CursorInfo.New();
                GetCursorInfo(ref cursorInfo);
                int i = cursorInfo.handle.ToInt32();
                return new MouseCursor(i);
            }
        }

        private static int RightMouseButtonDown
        {
            get { return GetSystemMetrics(SystemMetric.SM_SWAPBUTTON) == 0 ? WindowsConstants.MOUSEEVENTF_RIGHTDOWN : WindowsConstants.MOUSEEVENTF_LEFTDOWN; }
        }

        private static int RightMouseButtonUp
        {
            get { return GetSystemMetrics(SystemMetric.SM_SWAPBUTTON) == 0 ? WindowsConstants.MOUSEEVENTF_RIGHTUP : WindowsConstants.MOUSEEVENTF_LEFTUP; }
        }

        private static int LeftMouseButtonDown
        {
            get { return GetSystemMetrics(SystemMetric.SM_SWAPBUTTON) == 0 ? WindowsConstants.MOUSEEVENTF_LEFTDOWN : WindowsConstants.MOUSEEVENTF_RIGHTDOWN; }
        }

        private static int LeftMouseButtonUp
        {
            get { return GetSystemMetrics(SystemMetric.SM_SWAPBUTTON) == 0 ? WindowsConstants.MOUSEEVENTF_LEFTUP : WindowsConstants.MOUSEEVENTF_RIGHTUP; }
        }

        public virtual void RightClick()
        {
            SendInput(InputFactory.Mouse(MouseInput(RightMouseButtonDown)));
            SendInput(InputFactory.Mouse(MouseInput(RightMouseButtonUp)));
        }

        public virtual void Click()
        {
            System.Windows.Point clickLocation = Location;
            if (lastClickLocation.Equals(clickLocation))
            {
                int timeout = doubleClickTime - DateTime.Now.Subtract(lastClickTime).Milliseconds;
                if (timeout > 0) Thread.Sleep(timeout + ExtraMillisecondsBecauseOfBugInWindows);
            }
            MouseLeftButtonUpAndDown();
            lastClickTime = DateTime.Now;
            lastClickLocation = Location;
        }

        public static void LeftUp()
        {
            SendInput(InputFactory.Mouse(MouseInput(LeftMouseButtonUp)));
        }

        public static void LeftDown()
        {
            SendInput(InputFactory.Mouse(MouseInput(LeftMouseButtonDown)));
        }

        public virtual void DoubleClick(System.Windows.Point point)
        {
            Location = point;
            MouseLeftButtonUpAndDown();
            Thread.Sleep(200);
            MouseLeftButtonUpAndDown();
        }

        private static void SendInput(Input input)
        {
            // Added check for 32/64 bit  
            if (IntPtr.Size == 4)
                SendInput(1, ref input, Marshal.SizeOf(typeof(Input)));
            else
            {
                var input64 = new Input64(input);
                SendInput64(1, ref input64, Marshal.SizeOf(typeof(Input)));
            }
        }

        private static MouseInput MouseInput(int command)
        {
            return new MouseInput(command, GetMessageExtraInfo());
        }

        public virtual void RightClick(System.Windows.Point point)
        {
            Location = point;
            RightClickHere();
        }

       

        internal virtual void RightClickHere()
        {
            RightClick();
        }

     public virtual void Click(System.Windows.Point point)
        {
            Location = point;
            Click();
        }

      

        private void HoldForDrag()
        {
            LeftDown();
        }

        public static void MouseLeftButtonUpAndDown()
        {
            LeftDown();
            LeftUp();
        }

        public virtual void MoveOut()
        {
            Location = new System.Windows.Point(0, 0);
        }

    }
}