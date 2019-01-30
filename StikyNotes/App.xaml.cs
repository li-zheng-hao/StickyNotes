using System;
using System.Windows;
using ContextMenu = System.Windows.Controls.ContextMenu;
using MessageBox = System.Windows.MessageBox;

namespace StikyNotes
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        System.Threading.Mutex mutex;
        protected override void OnStartup(StartupEventArgs e)
        {
            bool ret;
            mutex = new System.Threading.Mutex(true, "StikyNotesAPP", out ret);

            if (!ret)
            {
                MessageBox.Show("程序已经运行了");
                Environment.Exit(0);
            }

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
            var MainWindow = new MainWindow();
            MainWindow.DataContext = new MainViewModel(MainWindow, new WindowsData());
            WindowsManager.Instance.Windows.Add(MainWindow);
            MainWindow.Show();
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
            SystemTray.Instance.DisposeNotifyIcon();
            base.OnExit(e);
        }



        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var programData = XMLHelper.DecodeXML<ProgramData>("StikyNotesData.xml");
            if (programData != null)
            {
                var windowsDatas = programData.Datas;
                ProgramData.Instance.IsWindowTopMost = programData.IsWindowTopMost;
                ProgramData.Instance.IsStartUpWithSystem = programData.IsStartUpWithSystem;
                MainWindow MainWindow;
                //有创建过的窗口
                if (windowsDatas.Count > 0)
                {
                    for (int i = 0; i < windowsDatas.Count; i++)
                    {
                        MainWindow = new MainWindow();
                        MainWindow.DataContext = new MainViewModel(MainWindow, windowsDatas[i]);
                        MainWindow.Show();
                    }
                }
                else//以前的窗口都被删掉了
                {
                    MainWindow = new MainWindow();
                    MainWindow.DataContext = new MainViewModel(MainWindow, new WindowsData());
                    MainWindow.Show();
                }
            }
            //没有创建过的窗口
            else
            {
                var MainWindow = new MainWindow();
                MainWindow.DataContext = new MainViewModel(MainWindow, new WindowsData());
                MainWindow.Show();
            }
        }
    }
}
