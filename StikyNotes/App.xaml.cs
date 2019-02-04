using System;
using System.Diagnostics;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using ContextMenu = System.Windows.Controls.ContextMenu;
using MessageBox = System.Windows.MessageBox;

namespace StikyNotes
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {

        #region 已经写好
        System.Threading.Mutex mutex;

        public bool IsInited { get; set; } = true;

        /// <summary>
        /// 程序启动
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            bool ret;
            mutex = new System.Threading.Mutex(true, "StikyNotesAPP", out ret);

            if (!ret)
            {
                MessageBox.Show("程序已经运行了");
                Environment.Exit(0);
            }

            //            base.OnStartup(e);

            Messenger.Default.Register<SaveMessage>(this, SaveDataMessage);
            var systemtray = SystemTray.Instance;
           
            var programData = XMLHelper.DecodeXML<ProgramData>(ConstData.SaveSettingDataName);
            if (programData != null)
            {
                var windowsDatas = programData.Datas;
                ProgramData.Instance.IsWindowTopMost = programData.IsWindowTopMost;
                ProgramData.Instance.IsStartUpWithSystem = programData.IsStartUpWithSystem;
                ProgramData.Instance.CurrenTheme= programData.CurrenTheme;
                ThemeAssist.ChangeTheme(programData.CurrenTheme);
                //有创建过的窗口
                if (windowsDatas.Count > 0)
                {
                    for (int i = 0; i < windowsDatas.Count; i++)
                    {
                       OpenNewWindow(windowsDatas[i]);
                    }
                }
                else//以前的窗口都被删掉了
                {
                   OpenNewWindow();

                }
            }
            //没有创建过的窗口
            else
            {
                OpenNewWindow();
            }

            IsInited = false;
        }

       





        //        protected override void OnDeactivated(EventArgs e)
        //        {
        //            base.OnDeactivated(e);
        //            System.Windows.Controls.ContextMenu menu = this.FindResource("NotifyIconMenu") as ContextMenu;
        //            if (menu.IsOpen == true)
        //            {
        //                menu.IsOpen = false;
        //            }
        //        }


        /// <summary>
        /// 新建窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenNewWindow();
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
            XMLHelper.SaveObjAsXml(ProgramData.Instance, ConstData.SaveSettingDataName);
            SystemTray.Instance.DisposeNotifyIcon();
            base.OnExit(e);
        }

        /// <summary>
        /// 程序开始启动的地方
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
           OnStartup(null);
        }

        /// <summary>
        /// 接收到了保存数据的消息
        /// </summary>
        /// <param name="message"></param>
        private void SaveDataMessage(SaveMessage message)
        {
            if (!IsInited)
            {
                XMLHelper.SaveObjAsXml(ProgramData.Instance, ConstData.SaveSettingDataName);
            }
        }

        /// <summary>
        /// 打开一个空的窗体
        /// </summary>
        private void OpenNewWindow()
        {
            var MainWindow = new MainWindow();
            var vm = new MainViewModel();
            MainWindow.DataContext = vm;
            vm.Datas = new WindowsData();
            MainWindow.Show();
            ProgramData.Instance.Datas.Add(vm.Datas);
            WindowsManager.Instance.Windows.Add(MainWindow);
        }

        private void OpenNewWindow(WindowsData data)
        {
            var MainWindow = new MainWindow();
            var vm = new MainViewModel();
            vm.Datas = data;
            MainWindow.DataContext = vm;
            MainWindow.Show();
            WindowsManager.Instance.Windows.Add(MainWindow);
            ProgramData.Instance.Datas.Add(data);
        }
        #endregion

    }
}
