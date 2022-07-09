using Common;
using Common.Lang;
using DB;
using MahApps.Metro.Controls;
using StickyNotes.Utils;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;
namespace StickyNotes
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {

        #region 主程序
        System.Threading.Mutex mutex;

        public bool IsInited { get; set; } = true;

        public TimerUtil TimerUtil;

        /// <summary>
        /// 程序启动
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            #region 是否只能运行一个APP

            bool ret;
            mutex = new System.Threading.Mutex(true, "StickyNotesAPP", out ret);

            if (!ret)
            {
                MessageBox.Show("程序已经运行了");
                Environment.Exit(0);
            }
            #endregion


            Logger.Log().Info("程序启动");
            /// 将全局异常保存到文件目录下
            Current.DispatcherUnhandledException += App_OnDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            //Messenger.Default.Register<SaveMessage>(this, SaveDataMessage);
            // 初始化数据库
            DBInit.InitDB();
            // 删除3小时前的旧数据
            new ProgramDataService().DeleteByDate(DateTime.Now.AddHours(3));
            var systemtray = SystemTray.Instance;
            var programData = DataHelper.RestoreData<ProgramData>();
            if (programData == null)
            {
                LanguageManager.ChangeLanguage(Language.Chinese);

            }
            else
            {
                LanguageManager.ChangeLanguage(programData.Language);

            }

            if (programData != null)
            {
                var windowsDatas = programData.Datas;
                ProgramData.Instance.IsWindowTopMost = programData.IsWindowTopMost;
                ProgramData.Instance.IsStartUpWithSystem = programData.IsStartUpWithSystem;
                ProgramData.Instance.CurrentTheme = programData.CurrentTheme;
                ProgramData.Instance.ShowAllHotKey = programData.ShowAllHotKey;
                ProgramData.Instance.IsAutoCheckUpdate = programData.IsAutoCheckUpdate;
                ProgramData.Instance.HideWindowData= programData.HideWindowData;
                ProgramData.Instance.Language = programData.Language;
                ThemeAssist.ChangeTheme(programData.CurrentTheme);
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
            TimerUtil = new TimerUtil(SaveDataAction);
            HttpHelper.BaseUrl = StickyNotes.Properties.Resources.ServerUrl;
            
            new Task(CheckUpdate).Start();

        }
        /// <summary>
        /// 检查程序是否要更新
        /// </summary>
        private void CheckUpdate()
        {
            ProgramData p = ProgramData.Instance;
            //if (p.IsAutoCheckUpdate == false)
            //    return;
            try
            {
                var updateHelper=new UpdateHelper();
                updateHelper.UpdateToolCompleted += () => {
                   var res=updateHelper.CheckSelfNeedUpdate();
                   if(res)
                   {
                        updateHelper.OpenUpdateTool();
                   }
                };
               
                updateHelper.UpdateUpdateTool();
            }
            catch (Exception ex)
            {
                Logger.Log().Debug($"无法更新 {ex.Message}");
            }
        }


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
        /// 程序退出事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            Logger.Log().Info("程序退出");
            DataHelper.SaveData(ProgramData.Instance);
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
        //private void SaveDataMessage(SaveMessage message)
        //{
        //    if (!IsInited)
        //    {
        //        XMLHelper.SaveObjAsXml(ProgramData.Instance, ConstData.SaveSettingDataName);
        //    }
        //}

        public void SaveDataAction()
        {
            if (!IsInited)
            {
                DataHelper.SaveData(ProgramData.Instance);
            }
        }
        
        /// <summary>
        /// 打开一个空的窗体
        /// </summary>
        private void OpenNewWindow()
        {
            var MainWindow = new MainWindow();
            MainWindow.viewModel.Datas = new WindowsData();
            MainWindow.Show();
            ProgramData.Instance.Datas.Add(MainWindow.viewModel.Datas);
            WindowsManager.Instance.Windows.Add(MainWindow);
        }
        /// <summary>
        /// 打开之前曾经存在过的窗体
        /// </summary>
        /// <param name="data"></param>
        private void OpenNewWindow(WindowsData data)
        {
            var MainWindow = new MainWindow();
            MainWindow.viewModel.Datas = data;
            MainWindow.viewModel.RestoreData(MainWindow.Document, MainWindow.viewModel.Datas.WindowID);
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
                MessageBox.Show(e.Exception.Message);

            }
            catch (Exception ex)
            {
                Logger.Log().Error(ex.StackTrace);
                Logger.Log().Error(ex.Message);
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
                    MessageBox.Show(exception.Message);
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
                    }
                }
                catch (Exception exxxx)
                {
                    Logger.Log().Error(exxxx.StackTrace);
                    Logger.Log().Error(exxxx.Message);

                }
            }
        }

        #endregion

    }
}
