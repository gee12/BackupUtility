using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace BackupLibrary
{
    public class Display
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public const int SW_HIDE = 0;
        public const int SW_SHOWNORMAL = 1;
        public const int SW_MAXIMIZE = 3;
        public const int SW_SHOW = 5;
        public const int SW_MINIMIZE = 6;

        public static ProcessWindowStyle WindowStyle = ProcessWindowStyle.Hidden;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="style"></param>
        public static void SetWindowStyle(ProcessWindowStyle style)
        {
            WindowStyle = style;
            //
            int showCode = SW_HIDE;
            switch (style)
            {
                case ProcessWindowStyle.Hidden: showCode = SW_HIDE; break;
                case ProcessWindowStyle.Normal: showCode = SW_SHOWNORMAL; break;
                case ProcessWindowStyle.Maximized: showCode = SW_MAXIMIZE; break;
                case ProcessWindowStyle.Minimized: showCode = SW_MINIMIZE; break;
            }
            var handle = GetConsoleWindow();
            ShowWindow(handle, showCode);
        }
    }
}
