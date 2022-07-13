using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using StickyNotes.Utils.HotKeyUtil;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using StickyNotes.Utils;
using StickyNotes.View;
using StickyNotes.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls.Dialogs;
using MaterialDesignThemes.Wpf;
using System.Threading;
using System.Threading.Tasks;
using StickyNotes.UserControl;
using System.Text;

namespace StickyNotes
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

        public RelayCommand<object> DropDownMenuClickCommand { get; set; }

        public List<string> Commands { get;set; } =new List<string>() { LanguageManager.Translate("menuList"), LanguageManager.Translate("menuAbout") };
        /// <summary>
        /// 窗体数据
        /// </summary>
        public WindowsData Datas { get; set; }

        /// <summary>
        /// 定时器检测是否位于窗体边缘
        /// </summary>
        DispatcherTimer timer;

        public bool IsDeleteWindowShowed { get; set; } = false;

        public ProgramData ProgramData { get; set; }
        #region 命令
        public RelayCommand NewWindowCommand { get; private set; }
        public RelayCommand OpenSettingCommand { get; private set; }
        public RelayCommand OpenAboutCommand { get; private set; }
        public RelayCommand AddFontSizeCommand { get; private set; }
        public RelayCommand ReduceFontSizeCommand { get; private set; }
        public RelayCommand<object> MoveWindowCommand { get; private set; }
        public RelayCommand ShowDeleteWindowCommand { get; private set; }

        public RelayCommand<object> DeleteWindowCommand { get; private set; }

        public RelayCommand OnContentRenderedCommand { get; private set; }
        public RelayCommand<MainWindow> OnSourceInitializedCommand { get; private set; }
        public RelayCommand<object> ChangeIsFocusedPropertyCommand { get; set; }

        public RelayCommand<WindowsData> CloseWindowButNotDeleteDataCommand { get; private set; }
        public RelayCommand OpenListCommand { get; set; }
        public CustomDialog CustomDeleteDialog { get; private set; }
        #endregion

        #region 快捷键数据
        /// <summary>
        /// 当前窗口句柄
        /// </summary>
        private IntPtr m_Hwnd = new IntPtr();
        /// <summary>
        /// 记录快捷键注册项的唯一标识符
        /// </summary>
        //private Dictionary<EHotKeySetting, int> m_HotKeySettings = new Dictionary<EHotKeySetting, int>();
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
            ShowDeleteWindowCommand = new RelayCommand(ShowDeleteWindowMethod);
            DeleteWindowCommand=new RelayCommand<object>(DeleteWindowMethod);
            MoveWindowCommand = new RelayCommand<object>(MoveWindowMethod);
            AddFontSizeCommand = new RelayCommand(AddFontSizeMethod);
            ReduceFontSizeCommand = new RelayCommand(ReduceFontSizeMethod);
            OnContentRenderedCommand = new RelayCommand(OnContentRenderedMethod);
            ChangeIsFocusedPropertyCommand = new RelayCommand<object>(ChangeIsFocusedPropertyMethod);
            OpenListCommand=new RelayCommand(OpenListMethod);
            CloseWindowButNotDeleteDataCommand = new RelayCommand<WindowsData>(CloseWindowButNotDeleteDataMethod);
            DropDownMenuClickCommand = new RelayCommand<object>(DropDownMenuClickMethod);
            ProgramData = ProgramData.Instance;

            Messenger.Default.Register<WindowsData>(this, "DeleteWindow",DeleteWindowActionInListView);
            Messenger.Default.Register<WindowsData>(this, "CloseWindow", CloseWindowButNotDeleteDataMethod);
            Messenger.Default.Register<WindowsData>(this, "OpenNewExistWindow", OpenNewExistWindowMethod);

        }

        private void DropDownMenuClickMethod(object obj)
        {
            string command=obj.ToString();
            if(command==Commands[0])
            {
                OpenListCommand.Execute(null);
            }else if(command==Commands[1])
            {
                OpenAboutCommand.Execute(null);

            }
                
               
        }

        private void OpenNewExistWindowMethod(WindowsData data)
        {
        }

        private void DeleteWindowActionInListView(WindowsData windowsData)
        {
            if(windowsData == this.Datas)
            {
                // 说明要删除的就是自己这个窗体
                foreach (Window item in Application.Current.Windows)
                {
                    if (item.DataContext == this)
                    {
                        WindowsManager.Instance.Windows.Remove((MainWindow)item);
                        item.Close();
                    }
                }
            }
        }
        private void CloseWindowButNotDeleteDataMethod(WindowsData data)
        {

            // 关闭但是不删除
            //if(WindowsManager.Instance.Windows.Count==1)
            //{
            //    return;
            //}
            foreach (Window item in Application.Current.Windows)
            {
                var main = item as MainWindow;
                if (main != null)
                {
                    var db = main.DataContext as MainViewModel;
                    if (db.Datas.WindowID == this.Datas.WindowID&&this.Datas.WindowID==data.WindowID)
                    {
                        WindowsManager.Instance.Windows.Remove((MainWindow)item);
                        ProgramData.Instance.Datas.Remove(data);
                        ProgramData.Instance.HideWindowData.Add(data);
                        data.IsShowed = false;
                        item.Close();
                    }
                }
            }

        }

        /// <summary>
        /// 打开所有列表窗口
        /// </summary>
        private void OpenListMethod()
        {
            if (WindowsManager.ListWindow == null||WindowsManager.ListWindow.IsActive==false)
            {
                WindowsManager.ListWindow = new ListWindow();
                WindowsManager.ListWindow.Show();
            }
            else
            {
                WindowsManager.ListWindow.Activate();
            }


        }

        private void DeleteWindowMethod(object window)
        {
            var win=window as MainWindow;
           
            // 删除窗体
            IsDeleteWindowShowed = false;
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
            var range= new TextRange(document.ContentStart, document.ContentEnd);
            using (MemoryStream ms=new MemoryStream())
            {
                range.Save(ms, DataFormats.Rtf);
                ms.Seek(0, SeekOrigin.Begin);
                var sr=new StreamReader(ms);
                var dataString = sr.ReadToEnd();
                Datas.RichTextBoxContent = dataString;
                Datas.DisplayRichTextBoxContent = range.Text;
            }
        }

        public void RestoreData(FlowDocument document, string fileName)
        {
            if (string.IsNullOrWhiteSpace(Datas.RichTextBoxContent))
                return;
            TextRange range;
            range = new TextRange(document.ContentStart,
                document.ContentEnd);
            var ms=GetMemoryStreamFromString(Datas.RichTextBoxContent);
            range.Load(ms, DataFormats.Rtf);
            Datas.DisplayRichTextBoxContent = range.Text;
            
        }
        private MemoryStream GetMemoryStreamFromString(string s)
        {
            if (s == null || s.Length == 0)
                return null;
            MemoryStream m = new MemoryStream();
            StreamWriter sw = new StreamWriter(m);
            sw.Write(s);
            sw.Flush();
            return m;
        }

        private void OnContentRenderedMethod()
        {
            // 注册热键
            InitHotKey();
        }

        ///// <summary>
        ///// 通知注册系统快捷键事件处理函数
        ///// </summary>
        ///// <param name="hotKeyModelList"></param>
        ///// <returns></returns>
        //private bool Instance_RegisterGlobalHotKeyEvent(ObservableCollection<HotKey> hotKeyModelList)
        //{
        //    return InitHotKey(hotKeyModelList);
        //}

        /// <summary>
        /// 初始化注册快捷键
        /// </summary>
        /// <param name="hotKeyModelList">待注册热键的项</param>
        /// <returns>true:保存快捷键的值；false:弹出设置窗体</returns>
        private bool InitHotKey()
        {
           
                try
                {
                ProgramData.ShowAllHotKey.Pressed -= HotKeyHandler.HandlePress;
                ProgramData.ShowAllHotKey.Pressed += HotKeyHandler.HandlePress;
                    HotkeyManager.GetHotkeyManager().TryAddHotkey(ProgramData.ShowAllHotKey);
                }
                catch (Exception)
                {
                    MessageBox.Show($"注册快捷键{ProgramData.ShowAllHotKey.ToString()}失败");
                }
                   
                return true;
        }


        private void HideWindowDetect(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// 打开相关窗口
        /// </summary>
        private void OpenAboutMethod()
        {
            if (WindowsManager.AboutWindow == null)
            {
                WindowsManager.AboutWindow = new AboutWindow();
                WindowsManager.AboutWindow.Show();
            }
            else
            {
                WindowsManager.AboutWindow.Show();
            }
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
        private async void OpenSettingMethod()
        {
           
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
        async void ShowDeleteWindowMethod()
        {
            IsDeleteWindowShowed = true;
            MainWindow win=null;
            var datas = Datas;
            foreach(Window item in Application.Current.Windows)
            {
                if(item.DataContext==this)
                {
                    win = item as MainWindow;
                    break;
                }
            }
            var dialog = win.Resources["customWarningDialog"] as CustomWarningDialog;
            CustomDeleteDialog = new CustomDialog(win) { Content = dialog, Title = LanguageManager.Translate("main-warning") };
            CustomDeleteDialog.DialogContentMargin = new GridLength(20);
            //CustomDeleteDialog.DialogContentWidth = new GridLength(100);
            dialog.CloseButtonClicked -= CancelDelete;
            dialog.CloseButtonClicked += CancelDelete;
            dialog.ConfirmButtonClicked -= DeleteWindow;
            dialog.ConfirmButtonClicked += DeleteWindow;
            await win.ShowMetroDialogAsync(CustomDeleteDialog);
        }
        

        public MainWindow GetCurrentWindow()
        {
            MainWindow win = null;
            var datas = Datas;
            foreach (Window item in Application.Current.Windows)
            {
                if (item.DataContext == this)
                {
                    win = item as MainWindow;
                    break;
                }
            }
            return win;
        }
        private async void DeleteWindow(object sender, RoutedEventArgs e)
        {
            MainWindow win = GetCurrentWindow();
            foreach (Window item in Application.Current.Windows)
            {
                if (item.DataContext == this)
                {
                    win = item as MainWindow;
                    break;
                }
            }
            await win.HideMetroDialogAsync(CustomDeleteDialog);
            DeleteWindowMethod(win);
        }

        private void CancelDelete(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow win = null;
                var datas = Datas;
                foreach (Window item in Application.Current.Windows)
                {
                    if (item.DataContext == this)
                    {
                        win = item as MainWindow;
                        break;
                    }
                }
                win.HideMetroDialogAsync(CustomDeleteDialog);
            }
            catch (Exception ex)
            {
                Logger.Log().Error(ex.Message);
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

    namespace Commands
    {
        public class CStrikeCommand
        {
            public static RoutedCommand StrikeCommand = new RoutedCommand("CStrikeCommand", typeof(CStrikeCommand));
        }
    }
}
