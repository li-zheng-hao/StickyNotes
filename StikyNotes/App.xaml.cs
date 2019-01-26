using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using Application = System.Windows.Application;
using ContextMenu = System.Windows.Controls.ContextMenu;
using MessageBox = System.Windows.MessageBox;

namespace StikyNotes
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var systemtray = SystemTray.Instance;
        }

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
            System.Windows.Controls.ContextMenu  menu = this.FindResource("NotifyIconMenu") as ContextMenu;
            if (menu.IsOpen == true)
            {
                menu.IsOpen = false;
            }
            
        }
        

        
        /// <summary>
        /// 新建窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuOpen_Click(object sender, RoutedEventArgs e)
        {
            

        }

        /// <summary>
        /// 窗体退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            SystemTray.Instance.DisposeNotifyIcon();
            App.Current.Shutdown();
        }
        /// <summary>
        /// 窗体退出事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            SystemTray.Instance.DisposeNotifyIcon();

        }
    }
}
