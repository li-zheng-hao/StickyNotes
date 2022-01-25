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
            var systemtray = SystemTray.Instance;
            var programData = XMLHelper.DecodeXML<ProgramData>(ConstData.SaveSettingDataName);
            var programData = XMLHelper.DecodeXML<ProgramData>(ConstData.SaveSettingDataName);
            if (programData == null)
            {
                LanguageManager.ChangeLanguage(Language.English);

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
            new Task(CheckUpdate).Start();

        }
        /// <summary>
        /// 检查程序是否要更新
        /// </summary>
        private void CheckUpdate()
        {
            ProgramData p = ProgramData.Instance;
            if (!p.IsAutoCheckUpdate)
                return;
            try
            {
                string version = StickyNotes.Properties.Resources.Version;
                var process = new Process();
                var arguments = version;
                arguments += " " + Assembly.GetExecutingAssembly().Location;
                process.StartInfo.FileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)+"\\AutoUpdateTool.exe"; // "iexplore.exe";   //IE
                process.StartInfo.Arguments = arguments;
                process.Start();
                #region
                //                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //加上这一句
                //                System.Net.WebClient client = new WebClient();
                //                byte[] page = client.DownloadData("https://github.com/li-zheng-hao/StickyNotes/releases/");
                //                string content = System.Text.Encoding.UTF8.GetString(page);
                //                string regex = @"v[0-9]\.[0-9]\.[0-9]";
                //                Regex re = new Regex(regex);
                //                MatchCollection matches = re.Matches(content);
                //                System.Collections.IEnumerator enu = matches.GetEnumerator();
                //                bool needUpdate = false;
                //                while (enu.MoveNext() && enu.Current != null)
                //                {
                //                    Match match = (Match)(enu.Current);

                //                    Console.Write(match.Value + "\r\n");
                //                    string result = match.Value;
                //                    Console.WriteLine(match.Value);
                //                    if (Convert.ToInt32(match.Value[1]) > Convert.ToInt32(version[1]))
                //                    {
                //                        needUpdate = true;
                //                    }
                //                    else if (Convert.ToInt32(match.Value[1]) == Convert.ToInt32(version[1]) && Convert.ToInt32(match.Value[3]) > Convert.ToInt32(version[3]))
                //                    {
                //                        needUpdate = true;
                //                    }
                //                    else if (Convert.ToInt32(match.Value[1]) == Convert.ToInt32(version[1]) && Convert.ToInt32(match.Value[3]) == Convert.ToInt32(version[3]) && Convert.ToInt32(match.Value[5]) > Convert.ToInt32(version[5]))
                //                    {
                //                        needUpdate = true;

                //                    }

                //                    if (needUpdate)
                //                    {
                //                        new Thread(() =>
                //                        {
                //                            this.Invoke(new Action(() =>
                //                            {
                //                                MessageBox.Show("发现新版本，建议去Github更新");
                //                            }));
                //                        }).Start();
                //                        break;
                //                    }
                //                }
                #endregion

            }
            catch (Exception ex)
            {
                Logger.Log().Debug("无法连接项目github官网");
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
                XMLHelper.SaveObjAsXml(ProgramData.Instance, ConstData.SaveSettingDataName);
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
                if (ts.Hours >= 2)
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
                MessageBox.Show("应用程序发生不可恢复的异常，将要退出！");
                Application.Current.Shutdown();

            }
            catch (Exception ex)
            {
                Logger.Log().Error(ex.StackTrace);
                Logger.Log().Error(ex.Message);
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
                        Application.Current.Shutdown();
                    }
                }
                catch (Exception exxxx)
                {
                    Logger.Log().Error(exxxx.StackTrace);
                    Logger.Log().Error(exxxx.Message);
                    Application.Current.Shutdown();

                }
            }
        }

        #endregion

    }
}
