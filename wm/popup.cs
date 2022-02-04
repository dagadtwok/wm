using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wm
{
    public partial class popup : Form
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);

        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        int delay = 1;
        bool exit = false;

        public popup()
        {
            InitializeComponent();
            RegisterHotKey(this.Handle, 0, 0x0001, Keys.K.GetHashCode());
            RegisterHotKey(this.Handle, 1, 0x0001, Keys.I.GetHashCode());
            RegisterHotKey(this.Handle, 2, 0x0001, Keys.J.GetHashCode());
            RegisterHotKey(this.Handle, 3, 0x0001, Keys.L.GetHashCode());
            RegisterHotKey(this.Handle, 4, 0x0001, Keys.O.GetHashCode());
            RegisterHotKey(this.Handle, 5, 0x0001, Keys.U.GetHashCode());

            RegisterHotKey(this.Handle, 99, 0x0001, Keys.Q.GetHashCode());
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 0x0312)
            {
                IntPtr currentWindow = GetForegroundWindow();
                int currentPID;
                GetWindowThreadProcessId(currentWindow, out currentPID);
                if (Process.GetCurrentProcess().Id != currentPID) {
                    
                    Screen[] screens = Screen.AllScreens;
                    Rect idk = new Rect();
                    GetWindowRect(currentWindow, ref idk);
                    int winpos = idk.Left + 8;
                    int monitornum = 99;
                    for (int i = 0; i < screens.Length; i++)
                    {
                        if (winpos <= screens[i].WorkingArea.Location.X+8 && winpos >= (screens[i].WorkingArea.Location.X) + screens[i].WorkingArea.Width)
                        {
                            monitornum = i;
                        }
                        if (monitornum == 99 && winpos >= screens[i].WorkingArea.Location.X) {
                            monitornum = i;
                        }
                    }
                    System.Drawing.Rectangle ScreenSize = Screen.AllScreens[monitornum].WorkingArea;
                    
                    switch (m.WParam.ToInt32()) {
                        case 0:
                            ShowWindow(currentWindow, (int)1);
                            SetWindowPos(currentWindow, IntPtr.Zero, 0 + ScreenSize.Location.X, ScreenSize.Height / 2, ScreenSize.Width, ScreenSize.Height / 2, 0x0);
                            showInfo("🢃");
                            break;
                        case 1:
                            ShowWindow(currentWindow, (int)1);
                            SetWindowPos(currentWindow, IntPtr.Zero, 0 + ScreenSize.Location.X, 0, ScreenSize.Width, ScreenSize.Height / 2, 0x0);
                            showInfo("🡹");
                            break;
                        case 2:
                            ShowWindow(currentWindow, (int)1);
                            SetWindowPos(currentWindow, IntPtr.Zero, 0 + ScreenSize.Location.X, 0, ScreenSize.Width / 2, ScreenSize.Height, 0x0);
                            showInfo("🡸");
                            break;
                        case 3:
                            ShowWindow(currentWindow, (int)1);
                            SetWindowPos(currentWindow, IntPtr.Zero, ScreenSize.Width / 2 + ScreenSize.Location.X, 0, ScreenSize.Width / 2, ScreenSize.Height, 0x0);
                            showInfo("🡺");
                            break;
                        case 4:
                            ShowWindow(currentWindow, (int)3);
                            showInfo("⮅");
                            break;
                        case 5:
                            ShowWindow(currentWindow, (int)1);
                            SetWindowPos(currentWindow, IntPtr.Zero,  ScreenSize.Location.X , 0, ScreenSize.Width, ScreenSize.Height, 0x0);
                            showInfo("⮇");
                            break;
                        case 99:
                            showInfo("🚫");
                            exit = true;
                            break;
                    }
                   
                }
            }
        }

        private void popup_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            waitTimer.Interval = 1000;
            waitTimer.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.Opacity > 0)
            {
                this.Opacity -= 0.1;
            }
            else if (this.Opacity == 0) {
                this.Hide();
                fadeOutTimer.Stop();
                if (exit) {
                    Application.Exit();
                }
            }
        }

        private void waitTimer_Tick(object sender, EventArgs e)
        {
            if (delay > 0) {
                delay--;
            }
            else
            {
                fadeOutTimer.Interval = 1;
                fadeOutTimer.Start();
                waitTimer.Stop();
            }
        }

        private void showInfo(string text) {
            this.label1.Text = text;
            this.Opacity = 1;
            this.Show();
            fadeOutTimer.Interval = 1;
            fadeOutTimer.Start();
        }
    }
}
