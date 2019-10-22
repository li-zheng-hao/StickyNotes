using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls;
using StikyNotes.Annotations;
using StikyNotes.Utils.HotKeyUtil;
using ComboBoxItem = System.Windows.Controls.ComboBoxItem;

namespace StikyNotes
{
    public class SettingViewModel 
    {
        public ProgramData Datas { get; set; }

        //public HotKeyModel ShowAllHotKey { get; set; }

        public SettingWindow SettingWin { get; set; }

        public RelayCommand<bool> IsTopMostChangedCommand { get; set; }

        public RelayCommand<bool> IsStartUpWithSystemChangedCommand { get; set; }

        public RelayCommand<SelectionChangedEventArgs> SelectionChangedCommand { get; set; }
        //public RelayCommand<KeyEventArgs> ShowAllHotKeyChangedCommand { get; set; }

        public SettingViewModel(SettingWindow win)
        {
            SettingWin = win;
            Datas = ProgramData.Instance;
            //ShowAllHotKey = ProgramData.Instance.ShowAllHotKey;

            IsTopMostChangedCommand = new RelayCommand<bool>(IsTopMostChangedMethod);
            IsStartUpWithSystemChangedCommand = new RelayCommand<bool>(IsStartUpWithSystemChangedMethod);
            SelectionChangedCommand = new RelayCommand<SelectionChangedEventArgs>(SelectionChangedMethod);
            //ShowAllHotKeyChangedCommand = new RelayCommand<KeyEventArgs>(ShowAllShortCutChangedMethod);
        }

        //private void ShowAllShortCutChangedMethod(KeyEventArgs e)
        //{
        //    if (e.Key == Key.LeftAlt || e.Key == Key.RightAlt)
        //    {
        //        return;
        //    }

        //    if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
        //    {
        //        return;
        //    }

        //    if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
        //    {
        //        return;
        //    }

        //    bool useCtrl = (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
        //    bool useAlt = (e.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt;
        //    bool useShift = (e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
        //    EKey useKey = EKey.Q;
        //    foreach (int v in Enum.GetValues(typeof(EKey)))
        //    {
        //        string keyName = Enum.GetName(typeof(EKey), v);
        //        if (e.Key.ToString() == keyName)
        //        {
        //            useKey = (EKey) v;
        //        }
        //    }

        //    var OldHotKey = ShowAllHotKey;
        //    HotKeyModel newModel = ShowAllHotKey;
        //    newModel.IsSelectAlt = useAlt;
        //    newModel.IsSelectCtrl = useCtrl;
        //    newModel.IsSelectShift = useShift;
        //    newModel.SelectKey = useKey;
        //    ProgramData.Instance.ShowAllHotKey = ShowAllHotKey;

        //    var hotKeyList = new ObservableCollection<HotKeyModel>
        //    {
        //        ShowAllHotKey
        //    };
        //    if (!HotKeySettingsManager.Instance.RegisterGlobalHotKey(hotKeyList))
        //    {
        //        MessageBox.Show("快捷键注册失败，可能与其他软件存在冲突");
        //        ShowAllHotKey = OldHotKey;
        //    }

        //    ShowAllHotKey = newModel;
        //    this.SettingWin.ShowAllTB.Text = ShowAllHotKey.ToString();
        //    //清楚当前快捷键
        //    //todo
        //    //检测输入的快捷键是否可用
        //    //todo
        //    //将更新的快捷键输入文本框




        //}



        /// <summary>
        /// 选择主题发生改变
        /// </summary>
        /// <param name="obj"></param>
        private void SelectionChangedMethod(SelectionChangedEventArgs e)
        {
            var content = e.AddedItems[0] as ComboBoxItem;

            switch (content.Content.ToString())
            {
                case "橘黄色":
                    ThemeAssist.ChangeTheme(Themes.Orange);
                    break;
                case "蓝色":
                    ThemeAssist.ChangeTheme(Themes.Blue);
                    break;
                case "灰色":
                    ThemeAssist.ChangeTheme(Themes.Gray);
                    break;
            }
        }

        /// <summary>
        /// 是否开机启动
        /// </summary>
        /// <param name="param">当前选项是否勾选</param>
        private void IsStartUpWithSystemChangedMethod(bool param)
        {
            if (param == false)
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

            Datas.IsStartUpWithSystem = !param;
        }

        /// <summary>
        /// 窗体是否置顶
        /// </summary>
        /// <param name="param"></param>
        private void IsTopMostChangedMethod(bool param)
        {
            if (param == false)
            {
                foreach (var win in WindowsManager.Instance.Windows)
                {
                    win.Topmost = true;
                }
            }
            else
            {
                foreach (var win in WindowsManager.Instance.Windows)
                {
                    win.Topmost = false;
                }
            }

            Datas.IsWindowTopMost = !param;
        }


        
    }
}