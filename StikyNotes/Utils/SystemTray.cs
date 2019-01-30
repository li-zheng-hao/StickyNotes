using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Windows.Forms;
using System.Windows.Navigation;
using Application = System.Windows.Application;

namespace StikyNotes
{
    public class SystemTray
    {
        public static SystemTray Instance;
        /// <summary>
        /// 静态构造函数,在类第一次被创建或者静态成员被调用的时候调用
        /// </summary>
        static SystemTray()
        {
            Instance=new SystemTray();
        }

        public NotifyIcon Ni { get; set; }

        private SystemTray()
        {
            Ni = new System.Windows.Forms.NotifyIcon();
            Stream iconStream = Application.GetResourceStream(new Uri("pack://application:,,,/StikyNotes;component/MyLogo.ico")).Stream;
            Ni.Icon = new System.Drawing.Icon(iconStream);
            Ni.Visible = true;
            Ni.MouseClick += Ni_MouseClick;
        }

        private void Ni_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                System.Windows.Controls.ContextMenu NotifyIconMenu = (System.Windows.Controls.ContextMenu)App.Current.FindResource("NotifyIconMenu");
                NotifyIconMenu.IsOpen = true;
                App.Current.MainWindow?.Activate();
            }

            if (e.Button == MouseButtons.Left)
            {
                var wins=WindowsManager.Instance.Windows;
                foreach (var i in wins)
                {
                    i.Activate();
                }
            }
        }

        /// <summary>
        /// 销毁系统托盘图标的资源
        /// </summary>
        public void DisposeNotifyIcon()
        {
            Ni?.Dispose();
        }
    }
}
