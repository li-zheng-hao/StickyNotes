using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using StickyNotes.Utils;
using StickyNotes.Utils.HotKeyUtil;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ComboBoxItem = System.Windows.Controls.ComboBoxItem;

namespace StickyNotes
{
    public class SettingViewModel : ViewModelBase
    {
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

        public RelayCommand<bool> IsTopMostChangedCommand { get; set; }

        public RelayCommand<bool> IsStartUpWithSystemChangedCommand { get; set; }

        public RelayCommand<SelectionChangedEventArgs> SelectionChangedCommand { get; set; }
        public RelayCommand<SelectionChangedEventArgs> LanguageChangeCommand { get; set; }
        public RelayCommand<KeyEventArgs> ShowAllHotKeyChangedCommand { get; set; }

        #region 窗体数据

        public bool IsTopMost { get; set; }
        public string HotKeyStr { get; set; }
        #endregion
        /// <summary>
        /// 当输入Ctrl+A，X，C，Z，V等按键，调用此command
        /// </summary>
        public RelayCommand<string> ShowAllTextUsedCommand { get; set; }

        public SettingViewModel()
        {
            Datas = ProgramData.Instance;
            IsTopMost = Datas.IsWindowTopMost;
            ShowAllHotKey = ProgramData.Instance.ShowAllHotKey;
            HotKeyStr = ShowAllHotKey.ToString();
            IsTopMostChangedCommand = new RelayCommand<bool>(IsTopMostChangedMethod);
            IsStartUpWithSystemChangedCommand = new RelayCommand<bool>(IsStartUpWithSystemChangedMethod);
            SelectionChangedCommand = new RelayCommand<SelectionChangedEventArgs>(SelectionChangedMethod);
            ShowAllHotKeyChangedCommand = new RelayCommand<KeyEventArgs>(ShowAllShortCutChangedMethod);
            ShowAllTextUsedCommand = new RelayCommand<string>(ShowAllTextUsedMethod);
            LanguageChangeCommand = new RelayCommand<SelectionChangedEventArgs>(LanguageChangeMethod);
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
        private void ShowAllShortCutChangedMethod(KeyEventArgs e)
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
                var res=HotkeyManager.GetHotkeyManager().TryAddHotkey(newHotKey);
                if (res == false)
                {
                    MessageBox.Show("快捷键注册失败，可能系统或其它软件存在冲突");
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
        /// 选择主题发生改变
        /// </summary>
        /// <param name="obj"></param>
        private void SelectionChangedMethod(SelectionChangedEventArgs e)
        {
            var content = e.AddedItems[0] as string;

            ThemeAssist.ChangeTheme(content);
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

            //Datas.IsStartUpWithSystem = !param;
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

    }
}
