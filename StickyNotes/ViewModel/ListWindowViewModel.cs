using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Common.Lang;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using StickyNotes.Utils;
using StickyNotes.Utils.HotKeyUtil;

namespace StickyNotes.ViewModel
{
    public class ListWindowViewModel:ViewModelBase
    {
        public RelayCommand<WindowsData> DeleteWindowCommand { get; set; }
        public RelayCommand<WindowsData> ChangeWindowVisibilityCommand { get; set; }
        public RelayCommand<string> FilterTextChangedCommand { get; set; }

        public RelayCommand OpenSettingViewCommand { get; set; }
        /// <summary>
        /// 搜索框内字符串
        /// </summary>
        public string FilterText { get; set; }
        /// <summary>
        /// 用来显示的列表，过滤和新增时添加
        /// </summary>
        public ObservableCollection<WindowsData> DisplayWindows{ get; set; }=new ObservableCollection<WindowsData>();
        /// <summary>
        /// 使用此类进行过滤、排序等操作
        /// CollectionView是基类视图，实现了ICollectionView接口，不支持排序，使用其派生类ListCollectionView等进行排序
        /// </summary>
        public CollectionViewSource CollectionViewSource { get; set; }
        public RelayCommand NewWindowCommand { get; set; }
        public ProgramData ProgramData { get; set; }
        public RelayCommand<Window> MoveWindowCommand { get; private set; }
        public ProgramData Datas { get; set; }
        public List<string> Themes { get; set; } = new List<string>()
        {
            "Red", "Green", "Blue", "Purple", "Orange", "Lime", "Emerald", "Teal", "Cyan", "Cobalt", "Indigo", "Violet", "Pink", "Magenta", "Crimson", "Amber", "Yellow", "Brown", "Olive", "Steel", "Mauve", "Taupe", "Sienna"
        };
        public List<string> Languages { get; set; } = new List<string>()
        {
            "Chinese",
            "English",
        };
        public HotKey ShowAllHotKey { get; set; }
        public bool IsTopMost { get; set; }
        public string HotKeyStr { get; set; }

        public RelayCommand<bool> IsTopMostChangedCommand { get; set; }

        public RelayCommand<bool> IsStartUpWithSystemChangedCommand { get; set; }

        public RelayCommand<string> ThemeChangeCommand { get; set; }
        public RelayCommand<SelectionChangedEventArgs> LanguageChangeCommand { get; set; }
        public RelayCommand<KeyEventArgs> ShowAllHotKeyChangedCommand { get; set; }
        /// <summary>
        /// 控制是显示列表还是显示设置
        /// </summary>
        public bool ShowListView { get; set; } = true;
        public bool ShowSettingView { get; set; } = false;
        public RelayCommand<string> ShowAllTextUsedCommand { get; set; }

        public ListWindowViewModel()
        {
            this.ProgramData = ProgramData.Instance;
            MoveWindowCommand = new RelayCommand<Window>(MoveWindowMethod);
            DeleteWindowCommand = new RelayCommand<WindowsData>(DeleteWindowClick);
            ChangeWindowVisibilityCommand = new RelayCommand<WindowsData>(ChangeWindowVisibilityMethod);
            NewWindowCommand = new RelayCommand(NewWindowMethod);
            OpenSettingViewCommand = new RelayCommand(OpenSettingViewMethod);
            FilterTextChangedCommand = new RelayCommand<string>(FilterTextChangedMethod);
            ProgramData.Datas.ToList().ForEach(data => DisplayWindows.Add(data));
            ProgramData.HideWindowData.ToList().ForEach(data=>DisplayWindows.Add(data));

            ProgramData.Datas.CollectionChanged += Datas_CollectionChanged;
            ProgramData.HideWindowData.CollectionChanged += Datas_CollectionChanged;
            Datas = ProgramData.Instance;
            IsTopMost = Datas.IsWindowTopMost;
            ShowAllHotKey = ProgramData.Instance.ShowAllHotKey;
            HotKeyStr = ShowAllHotKey.ToString();
            IsTopMostChangedCommand = new RelayCommand<bool>(IsTopMostChangedMethod);
            IsStartUpWithSystemChangedCommand = new RelayCommand<bool>(IsStartUpWithSystemChangedMethod);
            ThemeChangeCommand = new RelayCommand<string>(ThemeChangeMethod);
            ShowAllHotKeyChangedCommand = new RelayCommand<System.Windows.Input.KeyEventArgs>(ShowAllShortCutChangedMethod);
            ShowAllTextUsedCommand = new RelayCommand<string>(ShowAllTextUsedMethod);
            LanguageChangeCommand = new RelayCommand<SelectionChangedEventArgs>(LanguageChangeMethod);

            //var dist=DisplayWindows.Distinct().ToList();
            CollectionViewSource = new CollectionViewSource() { Source= DisplayWindows } ;

            //ListCollectionView.Filter = new Predicate<object>(it =>
            //{
            //    if (string.IsNullOrEmpty(FilterText))
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return ((WindowsData)it).RichTextBoxContent.Contains(FilterText);
            //    }
            //});
            CollectionViewSource.View.SortDescriptions.Add((new SortDescription(nameof(WindowsData.ModifiedTime), ListSortDirection.Descending)));
        }


        private void ThemeChangeMethod(string themeName)
        {
            ThemeAssist.ChangeTheme(themeName);
        }

        private void LanguageChangeMethod(SelectionChangedEventArgs arg)
        {
            int index = Languages.IndexOf(arg.AddedItems[0].ToString());
            LanguageManager.ChangeLanguage((Language)index);
            ProgramData.Instance.Language = (Language)index;
        }

        private void ShowAllTextUsedMethod(string key)
        {
            ////使用了ctrl
            //bool useCtrl = true;
            //bool useAlt = false;
            //bool useShift = false;
            //EKey? useKey = null;
            //if (key == "X")
            //{
            //    useKey = EKey.X;
            //}
            //else if (key == "A")
            //{
            //    useKey = EKey.A;
            //}
            //else if (key == "C")
            //{
            //    useKey = EKey.C;
            //}
            //else if (key == "V")
            //{
            //    useKey = EKey.V;
            //}
            //else if (key == "Z")
            //{
            //    useKey = EKey.Z;
            //}
            //var oldHotKey = ShowAllHotKey;
            //var newModel = ShowAllHotKey;
            //newModel.IsSelectAlt = useAlt;
            //newModel.IsSelectCtrl = useCtrl;
            //newModel.IsSelectShift = useShift;
            //if (useKey != null) newModel.SelectKey = (EKey)useKey;

            //if (!useCtrl && !useAlt && !useShift)
            //    return;
            //var hotKeyList = new ObservableCollection<HotKey>
            //{
            //    ShowAllHotKey
            //};
            //HotKeySettingsManager.Instance.IsShowAllWindowHotKeyNeedChanged = true;
            //if (!HotKeySettingsManager.Instance.RegisterGlobalHotKey(hotKeyList))
            //{
            //    //todo 
            //    MessageBox.Show("快捷键注册失败，可能系统或其它软件存在冲突");
            //    ShowAllHotKey = oldHotKey;
            //}
            //else
            //{
            //    ShowAllHotKey = newModel;
            //    ProgramData.Instance.ShowAllHotKey = ShowAllHotKey;
            //    this.HotKeyStr = ShowAllHotKey.ToString();
            //    //this.SettingWin.ShowAllTB.Text = ShowAllHotKey.ToString();
            //}

            //return;

        }
        private void ShowAllShortCutChangedMethod(System.Windows.Input.KeyEventArgs e)
        {
            // Don't let the event pass further
            // because we don't want standard textbox shortcuts working
            e.Handled = true;

            // Get modifiers and key data
            var modifiers = Keyboard.Modifiers;
            var key = e.Key;
            // When Alt is pressed, SystemKey is used instead
            if (key == Key.System)
            {
                key = e.SystemKey;
            }

            // Pressing delete, backspace or escape without modifiers clears the current value
            if (modifiers == ModifierKeys.None &&
                (key == Key.Delete || key == Key.Back || key == Key.Escape))
            {
                return;
            }

            //If no actual key was pressed - return
            if (key == Key.LeftCtrl ||
                key == Key.RightCtrl ||
                key == Key.LeftAlt ||
                key == Key.RightAlt ||
                key == Key.LeftShift ||
                key == Key.RightShift ||
                key == Key.LWin ||
                key == Key.RWin ||
                key == Key.Clear ||
                key == Key.OemClear ||
                key == Key.Apps)
            {
                return;
            }


            if (key.ToString().Contains("ImeProcessed"))
            {
                key = e.ImeProcessedKey;
            }


            var newHotKey = new HotKey(e.Key, modifiers, HotKeyHandler.HandlePress, HotKeyType.ShowOrHideAllWindow);
            var oldKey = HotkeyManager.GetHotkeyManager().GetHotkeys().ToList().Where(it => it.HotKeyType == HotKeyType.ShowOrHideAllWindow).FirstOrDefault();
            if (oldKey == null)
            {
                HotkeyManager.GetHotkeyManager().TryAddHotkey(newHotKey);
            }
            else
            {
                HotkeyManager.GetHotkeyManager().TryRemoveHotkey(oldKey);
                var res = HotkeyManager.GetHotkeyManager().TryAddHotkey(newHotKey);
                if (res == false)
                {
                    System.Windows.MessageBox.Show("快捷键注册失败，可能系统或其它软件存在冲突");
                }
                else
                {
                    ShowAllHotKey = newHotKey;
                    ProgramData.Instance.ShowAllHotKey = ShowAllHotKey;
                    HotKeyStr = ShowAllHotKey.ToString();
                }
            }


            return;
        }

        /// <summary>
        /// 是否开机启动
        /// </summary>
        /// <param name="param">当前选项是否勾选</param>
        private void IsStartUpWithSystemChangedMethod(bool param)
        {
            if (param == true)
            {
                Microsoft.Win32.RegistryKey key =
                    Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run",
                        true);

                Assembly curAssembly = Assembly.GetExecutingAssembly();
                key.SetValue(curAssembly.GetName().Name, curAssembly.Location);
            }
            else
            {
                Microsoft.Win32.RegistryKey key =
                    Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run",
                        true);
                Assembly curAssembly = Assembly.GetExecutingAssembly();
                if (key.GetValue(curAssembly.GetName().Name) != null)
                {
                    key.DeleteValue(curAssembly.GetName().Name);
                }
            }
        }
        /// <summary>
        /// 窗体是否置顶
        /// </summary>
        /// <param name="param"></param>
        private void IsTopMostChangedMethod(bool param)
        {
            foreach (var win in WindowsManager.Instance.Windows)
            {
                win.Topmost = param;
                win.Activate();
            }
            //this.IsTopMost = !param;


            Datas.IsWindowTopMost = IsTopMost;

        }
        private void MoveWindowMethod(Window window)
        {
            window.DragMove();
        }

        /// <summary>
        /// 切换列表和设置界面
        /// </summary>
        private void OpenSettingViewMethod()
        {
            this.ShowListView = !this.ShowListView;
            this.ShowSettingView = !this.ShowSettingView;
        }

        private void Datas_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(string.IsNullOrEmpty(FilterText))
            {
                this.DisplayWindows.Clear();
                ProgramData.Datas.ToList().ForEach(data => DisplayWindows.Add(data));
                ProgramData.HideWindowData.ToList().ForEach(data => DisplayWindows.Add(data));

            }
        }

        private void FilterTextChangedMethod(string filterText) 
        {
            if (string.IsNullOrEmpty(filterText))
            {
                CollectionViewSource.View.Filter = null;
                return;
            }
            CollectionViewSource.View.Filter = new Predicate<object>(it =>
            {
                return ((WindowsData)it).DisplayRichTextBoxContent.Contains(filterText);
            });
            // todo
            //var text=(e.Source as System.Windows.Controls.TextBox).Text;
            //this.DisplayWindows.Clear();
            //this.ProgramData.Datas?.ToList()?.Where(it=>it.DisplayRichTextBoxContent.Contains(text))?.ToList()?.ForEach(data => DisplayWindows.Add(data));
            //this.ProgramData.HideWindowData?.ToList()?.Where(it=>it.DisplayRichTextBoxContent.Contains(text))?.ToList()?.ForEach(data => DisplayWindows.Add(data));
        }
        private void NewWindowMethod()
        {
            MainWindow win = new MainWindow();
            win.viewModel.Datas = new WindowsData();
            win.Show();
            ProgramData.Instance.Datas.Add(win.viewModel.Datas);
            WindowsManager.Instance.Windows.Add(win);
           
        }

        private void ChangeWindowVisibilityMethod(WindowsData windowsData)
        {
            if(ProgramData.Instance.Datas.Contains(windowsData))
            {
                //通知主窗体关闭
                Messenger.Default.Send<WindowsData>(windowsData, "CloseWindow");
            }
            else
            {
                var MainWindow = new MainWindow();
                MainWindow.viewModel.Datas = windowsData;
                windowsData.IsShowed = true;
                MainWindow.viewModel.RestoreData(MainWindow.Document, MainWindow.viewModel.Datas.WindowID);
                MainWindow.Show();
                WindowsManager.Instance.Windows.Add(MainWindow);
                ProgramData.Instance.Datas.Add(windowsData);
                ProgramData.Instance.HideWindowData.Remove(windowsData);
            }
        }
       
        private void DeleteWindowClick(WindowsData windowsData)
        {
            try
            {
                if (ProgramData.Instance.HideWindowData.Contains(windowsData))
                {
                    ChangeWindowVisibilityMethod(windowsData);
                }
                ProgramData.Instance.Datas.Remove(windowsData);
                //通知主窗体删除这个数据 关闭这个便签窗体
                Messenger.Default.Send<WindowsData>(windowsData, "DeleteWindow");
            }
            catch (Exception ex)
            {
                Logger.Log().Error("删除窗体时出错"+ex.Message);
            }
           
        }

    }
}
