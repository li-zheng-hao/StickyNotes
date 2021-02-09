using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using StikyNotes.Utils.HotKeyUtil;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Threading;

namespace StikyNotes
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// 窗体数据
        /// </summary>
        public WindowsData Datas { get; set; }

        /// <summary>
        /// 定时器检测是否位于窗体边缘
        /// </summary>
        DispatcherTimer timer;

        public ProgramData ProgramData { get; set; }
        #region 命令
        public RelayCommand NewWindowCommand { get; private set; }
        public RelayCommand OpenSettingCommand { get; private set; }
        public RelayCommand OpenAboutCommand { get; private set; }
        public RelayCommand AddFontSizeCommand { get; private set; }
        public RelayCommand ReduceFontSizeCommand { get; private set; }
        public RelayCommand<object> MoveWindowCommand { get; private set; }
        public RelayCommand<MainWindow> DeletePaWindowCommand { get; private set; }
        public RelayCommand OnContentRenderedCommand { get; private set; }
        public RelayCommand<MainWindow> OnSourceInitializedCommand { get; private set; }
        public RelayCommand<object> ChangeIsFocusedPropertyCommand { get; set; }
        #endregion

        #region 快捷键数据
        /// <summary>
        /// 当前窗口句柄
        /// </summary>
        private IntPtr m_Hwnd = new IntPtr();
        /// <summary>
        /// 记录快捷键注册项的唯一标识符
        /// </summary>
        private Dictionary<EHotKeySetting, int> m_HotKeySettings = new Dictionary<EHotKeySetting, int>();
        #endregion



        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += HideWindowDetect;
            timer.Start();

            NewWindowCommand = new RelayCommand(NewWindowMethod);
            OpenSettingCommand = new RelayCommand(OpenSettingMethod);
            OpenAboutCommand = new RelayCommand(OpenAboutMethod);
            DeletePaWindowCommand = new RelayCommand<MainWindow>(DeleteWindowMethod);
            MoveWindowCommand = new RelayCommand<object>(MoveWindowMethod);
            AddFontSizeCommand = new RelayCommand(AddFontSizeMethod);
            ReduceFontSizeCommand = new RelayCommand(ReduceFontSizeMethod);
            OnContentRenderedCommand = new RelayCommand(OnContentRenderedMethod);
            OnSourceInitializedCommand = new RelayCommand<MainWindow>(OnSourceInitializedMethod);
            ChangeIsFocusedPropertyCommand = new RelayCommand<object>(ChangeIsFocusedPropertyMethod);
            ProgramData = ProgramData.Instance;
        }
        /// <summary>
        /// 修改IsFocused属性
        /// </summary>
        /// <param name="obj"></param>
        private void ChangeIsFocusedPropertyMethod(object obj)
        {
            if (obj is string)
            {
                Datas.IsFocused = true;
            }
            else
            {
                Console.WriteLine("存储数据" + Datas.WindowID);
                Datas.IsFocused = false;
                var document = obj as FlowDocument;
                SaveDocument(document, Datas.DocumentFileName);

            }
        }
        /// <summary>
        /// 将数据文件进行保存
        /// </summary>
        /// <param name="datasDocumentFilePath"></param>
        private void SaveDocument(FlowDocument document, string datasDocumentFilePath)
        {
            TextRange range;
            FileStream fileStream;

            range = new TextRange(document.ContentStart,
                document.ContentEnd);
            //获取当前文件夹路径
            string currPath = System.Windows.Forms.Application.StartupPath;
            //检查是否存在文件夹
            string subPath = currPath + "/Datas/";
            if (false == System.IO.Directory.Exists(subPath))
            {
                //创建Datas文件夹
                System.IO.Directory.CreateDirectory(subPath);
            }
            fileStream = new FileStream(subPath + datasDocumentFilePath, FileMode.Create);
            range.Save(fileStream, DataFormats.XamlPackage);

            fileStream.Close();
        }

        public void RestoreData(FlowDocument document, string fileName)
        {
            TextRange range;
            FileStream fileStream;

            range = new TextRange(document.ContentStart,
                document.ContentEnd);

            string currPath = System.Windows.Forms.Application.StartupPath;
            //Environment.CurrentDirectory;
            string subPath = currPath + "/Datas/";
            fileStream = new FileStream(subPath + fileName, FileMode.Open);
            range.Load(fileStream, DataFormats.XamlPackage);
            fileStream.Close();
        }
        private void OnSourceInitializedMethod(MainWindow window)
        {
            HotKeySettingsManager.Instance.RegisterGlobalHotKeyEvent += Instance_RegisterGlobalHotKeyEvent;

            // 获取窗体句柄
            m_Hwnd = new WindowInteropHelper(window).Handle;
            HwndSource hWndSource = HwndSource.FromHwnd(m_Hwnd);
            // 添加处理程序
            if (hWndSource != null) hWndSource.AddHook(WndProc);
        }

        /// <summary>
        /// 窗体回调函数，接收所有窗体消息的事件处理函数
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="msg">消息</param>
        /// <param name="wideParam">附加参数1</param>
        /// <param name="longParam">附加参数2</param>
        /// <param name="handled">是否处理</param>
        /// <returns>返回句柄</returns>
        private IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wideParam, IntPtr longParam, ref bool handled)
        {
            switch (msg)
            {
                case HotKeyManager.WM_HOTKEY:
                    int sid = wideParam.ToInt32();
                    // 按下了显示所有窗体的快捷键,执行相应逻辑
                    if (sid == m_HotKeySettings[EHotKeySetting.ShowAllWindow])
                    {
                        ActivateOrHideAllNotTopMostWindow();
                        // WindowHideManager.GetInstance().StopAllHideAction(2000);
                    }
                    handled = true;
                    break;
            }
            return IntPtr.Zero;
        }
        /// <summary>
        /// 显示所有窗体或者隐藏所有窗体
        /// </summary>
        private void ActivateOrHideAllNotTopMostWindow()
        {
            foreach (var window in WindowsManager.Instance.Windows)
            {
                if (window.viewModel.Datas.IsCurrentWindowTopMost == false)
                {
                    if (ProgramData.Instance.IsWindowVisible)
                        window.Visibility = Visibility.Hidden;
                    else
                    {
                        Console.WriteLine("A");
                        window.Visibility = Visibility.Visible;
                        window.Activate();
                    }

                }
            }

            ProgramData.Instance.IsWindowVisible = !ProgramData.Instance.IsWindowVisible;

        }


        private void OnContentRenderedMethod()
        {
            // 注册热键
            InitHotKey();
        }

        /// <summary>
        /// 通知注册系统快捷键事件处理函数
        /// </summary>
        /// <param name="hotKeyModelList"></param>
        /// <returns></returns>
        private bool Instance_RegisterGlobalHotKeyEvent(ObservableCollection<HotKeyModel> hotKeyModelList)
        {
            return InitHotKey(hotKeyModelList);
        }

        /// <summary>
        /// 初始化注册快捷键
        /// </summary>
        /// <param name="hotKeyModelList">待注册热键的项</param>
        /// <returns>true:保存快捷键的值；false:弹出设置窗体</returns>
        private bool InitHotKey(ObservableCollection<HotKeyModel> hotKeyModelList = null)
        {
            if (HotKeySettingsManager.Instance.IsShowAllWindowHotKeyRegistered == false ||
                HotKeySettingsManager.Instance.IsShowAllWindowHotKeyNeedChanged == true)
            {
                HotKeySettingsManager.Instance.IsShowAllWindowHotKeyRegistered = true;
                HotKeySettingsManager.Instance.IsShowAllWindowHotKeyNeedChanged = false;
                var list = hotKeyModelList ?? HotKeySettingsManager.Instance.LoadDefaultHotKey();
                // 注册全局快捷键
                string failList = HotKeyHelper.RegisterGlobalHotKey(list, m_Hwnd, out m_HotKeySettings);
                if (string.IsNullOrEmpty(failList))
                    return true;
                System.Windows.MessageBox.Show(string.Format("无法注册下列快捷键\n\r{0}", failList), "提示", MessageBoxButton.OK);
                return false;
            }
            else
            {
                return true;
            }

        }



        private void HideWindowDetect(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// 打开相关窗口
        /// </summary>
        private void OpenAboutMethod()
        {
            var win = new AboutWindow();
            win.Show();
        }

        /// <summary>
        /// 减少字体
        /// </summary>
        private void ReduceFontSizeMethod()
        {
            if (Datas.FontSize > 8)
            {
                Datas.FontSize -= 2;
            }
        }
        /// <summary>
        /// 放大字体
        /// </summary>
        private void AddFontSizeMethod()
        {
            if (Datas.FontSize < 32)
            {
                Datas.FontSize += 2;
            }
        }

        /// <summary>
        /// 打开设置窗口
        /// </summary>
        private void OpenSettingMethod()
        {
            var win = new SettingWindow();
            win.Show();
        }

        /// <summary>
        /// 移动窗体
        /// </summary>
        /// <param name="e">当前的Window</param>
        private void MoveWindowMethod(object e)
        {
            var win = e as MainWindow;
            win.ResizeMode = ResizeMode.NoResize;
            win.DragMove();
            win.ResizeMode = ResizeMode.CanResize;

            //            var newPos=win.PointFromScreen(new Point(0, 0));
            //            Datas.StartUpPosition = newPos;
        }

        /// <summary>
        /// 新建窗体
        /// </summary>
        void NewWindowMethod()
        {
            MainWindow win = new MainWindow();
            win.viewModel.Datas = new WindowsData();
            win.Show();
            ProgramData.Instance.Datas.Add(win.viewModel.Datas);
            WindowsManager.Instance.Windows.Add(win);

        }

        /// <summary>
        /// 删除窗体
        /// </summary>
        void DeleteWindowMethod(MainWindow obj)
        {
            var win = obj as MainWindow;
            string documentFileName = string.Empty;
            if (WindowsManager.Instance.Windows.Contains(win))
            {
                WindowsManager.Instance.Windows.Remove(win);
                ProgramData.Instance.Datas.Remove(Datas);
                documentFileName = Datas.DocumentFileName;
            }
            win.Close();
            if (documentFileName != string.Empty)
            {
                RemoveDocumentFile(documentFileName);
            }
        }
        /// <summary>
        /// 删除已经不需要的文档数据
        /// </summary>
        /// <param name="fileName"></param>
        private void RemoveDocumentFile(string fileName)
        {

            string currPath = Environment.CurrentDirectory;
            string subPath = currPath + "/Datas/";
            if (System.IO.File.Exists(subPath + fileName))
            {
                System.IO.File.Delete(subPath + fileName);
            }
        }
    }
}
