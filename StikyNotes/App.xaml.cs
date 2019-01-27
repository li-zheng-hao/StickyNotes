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
        /// <summary>
        /// 这里是程序入口，在这里读取所有创建过的窗体
        /// </summary>
//        [STAThread]
//        static void Main()
//        {
//            App app=new App();
//            app.InitializeComponent();
//            var programData = XMLHelper.DecodeXML<ProgramData>("Data.xml");
//            var windowsDatas = programData.Datas;
//            MainWindow MainWindow;
//            if (windowsDatas.Count > 0)
//            {
//                for (int i = 0; i < windowsDatas.Count; i++)
//                {
//                    MainWindow = new MainWindow();
//                    MainWindow.DataContext = new MainViewModel(MainWindow, windowsDatas[i]);
//                    app.Run(MainWindow);
//                }
//            }
//        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var systemtray = SystemTray.Instance;
        }

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
            System.Windows.Controls.ContextMenu menu = this.FindResource("NotifyIconMenu") as ContextMenu;
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
            XMLHelper.SaveObjAsXml(ProgramData.Instance, "Data.xml");
            SystemTray.Instance.DisposeNotifyIcon();

        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var programData = XMLHelper.DecodeXML<ProgramData>("Data.xml");
            var windowsDatas = programData.Datas;
            MainWindow MainWindow;
            if (windowsDatas.Count > 0)
            {
                for (int i = 0; i < windowsDatas.Count; i++)
                {
                    MainWindow = new MainWindow();
                    MainWindow.DataContext = new MainViewModel(MainWindow, windowsDatas[i]);
                    MainWindow.Show();
                }
            }
            else
            {
                MainWindow = new MainWindow();
                MainWindow.DataContext = new MainViewModel(MainWindow, new WindowsData());
                MainWindow.Show();
            }
        }
    }
}
