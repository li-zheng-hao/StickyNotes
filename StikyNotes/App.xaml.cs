using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Messaging;
using StikyNotes.Utils;
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

        #region 已经写好
        System.Threading.Mutex mutex;

        public bool IsInited { get; set; } = true;

        public TimerUtil TimerUtil;

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
            Logger.Log().Info("程序启动");

            /// 将全局异常保存到文件目录下
            Current.DispatcherUnhandledException += App_OnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Messenger.Default.Register<SaveMessage>(this, SaveDataMessage);
            var systemtray = SystemTray.Instance;
           
            var programData = XMLHelper.DecodeXML<ProgramData>(ConstData.SaveSettingDataName);
            if (programData != null)
            {
                var windowsDatas = programData.Datas;
                ProgramData.Instance.IsWindowTopMost = programData.IsWindowTopMost;
                ProgramData.Instance.IsStartUpWithSystem = programData.IsStartUpWithSystem;
                ProgramData.Instance.CurrenTheme= programData.CurrenTheme;
                ProgramData.Instance.ShowAllHotKey = programData.ShowAllHotKey;
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
            TimerUtil=new TimerUtil(SaveDataAction);

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
            Logger.Log().Info("程序退出");
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

        [System.Obsolete("方法已经弃用，改成使用定时器，每过一段时间自动保存")]
        /// <summary>
        /// 接收到了保存数据的消息
        /// </summary>
        /// <param name="message"></param>
        private void SaveDataMessage(SaveMessage message)
        {
//            if (!IsInited)
//            {
//                XMLHelper.SaveObjAsXml(ProgramData.Instance, ConstData.SaveSettingDataName);
//            }
        }

        public void SaveDataAction()
        {
            if (!IsInited)
            {
                XMLHelper.SaveObjAsXml(ProgramData.Instance,ConstData.SaveSettingDataName );
                BackupDataAction();

            }
        }
        public void BackupDataAction()
        {
            if (File.Exists(ConstData.BackUpDataName))
            {
                FileInfo newestData = new FileInfo(ConstData.SaveSettingDataName);
                FileInfo backupData = new FileInfo(ConstData.BackUpDataName);
                TimeSpan ts1 = new TimeSpan(newestData.CreationTime.Ticks);
                TimeSpan ts2 = new TimeSpan(backupData.CreationTime.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                if (ts.Hours >= 1)
                {
                    XMLHelper.SaveObjAsXml(ProgramData.Instance, ConstData.BackUpDataName);
                }
            }
            else
            {
                XMLHelper.SaveObjAsXml(ProgramData.Instance, ConstData.BackUpDataName);
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


        // <summary>
        /// UI线程抛出全局异常事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                Logger.Log().Error(e.Exception.StackTrace);
                Logger.Log().Error(e.Exception.Message);
                MessageBox.Show("应用程序发生不可恢复的异常，将要退出！");
                Application.Current.Shutdown();

            }
            catch (Exception ex)
            {
                Logger.Log().Error(ex.StackTrace);
                Logger.Log().Error(ex.Message);
                MessageBox.Show("应用程序发生不可恢复的异常，将要退出！");
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// 非UI线程抛出全局异常事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var exception = e.ExceptionObject as Exception;
                if (exception != null)
                {
                    Logger.Log().Error(exception.StackTrace);
                    Logger.Log().Error(exception.Message);
                    MessageBox.Show("应用程序发生不可恢复的异常，将要退出！");
                    Application.Current.Shutdown();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    var exception = e.ExceptionObject as Exception;
                    if (exception != null)
                    {
                        Logger.Log().Error(exception.StackTrace);
                        Logger.Log().Error(exception.Message);
                        MessageBox.Show("应用程序发生不可恢复的异常，将要退出！");
                        Application.Current.Shutdown();


                    }
                }
                catch (Exception exxxx)
                {
                    Logger.Log().Error(exxxx.StackTrace);
                    Logger.Log().Error(exxxx.Message);
                    MessageBox.Show("应用程序发生不可恢复的异常，将要退出！");
                    Application.Current.Shutdown();

                }
            }
        }

        #endregion

    }
}
