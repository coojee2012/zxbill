using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;


namespace Bill
{
    
    class ConsoleWin32Helper
    {

        /**
        static ConsoleWin32Helper()
        {
            _NotifyIcon.Icon = new Icon(@"G:\BruceLi Test\ConsoleAppTest\ConsoleApps\Tray\small.ico");
            _NotifyIcon.Visible = false;
            _NotifyIcon.Text = "tray";

            ContextMenu menu = new ContextMenu();
            MenuItem item = new MenuItem();
            item.Text = "右键菜单，还没有添加事件";
            item.Index = 0;

            menu.MenuItems.Add(item);
            _NotifyIcon.ContextMenu = menu;

            _NotifyIcon.MouseDoubleClick += new MouseEventHandler(_NotifyIcon_MouseDoubleClick);

        }

        static void _NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Console.WriteLine("托盘被双击.");
        }

        #region 禁用关闭按钮
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, IntPtr bRevert);

        [DllImport("user32.dll", EntryPoint = "RemoveMenu")]
        static extern IntPtr RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        /// <summary>
        /// 禁用关闭按钮
        /// </summary>
        /// <param name="consoleName">控制台名字</param>
        public static void DisableCloseButton(string title)
        {
            //线程睡眠，确保closebtn中能够正常FindWindow，否则有时会Find失败。。
            Thread.Sleep(100);

            IntPtr windowHandle = FindWindow(null, title);
            IntPtr closeMenu = GetSystemMenu(windowHandle, IntPtr.Zero);
            uint SC_CLOSE = 0xF060;
            RemoveMenu(closeMenu, SC_CLOSE, 0x0);
        }
        public static bool IsExistsConsole(string title)
        {
            IntPtr windowHandle = FindWindow(null, title);
            if (windowHandle.Equals(IntPtr.Zero)) return false;

            return true;
        }
        #endregion

        #region 托盘图标
        static NotifyIcon _NotifyIcon = new NotifyIcon();
        public static void ShowNotifyIcon()
        {
            _NotifyIcon.Visible = true;
            _NotifyIcon.ShowBalloonTip(3000, "", "我是托盘图标，用右键点击我试试，还可以双击看看。", ToolTipIcon.None);
        }
        public static void HideNotifyIcon()
        {
            _NotifyIcon.Visible = false;
        }

        #endregion
         * **/
    }

}
