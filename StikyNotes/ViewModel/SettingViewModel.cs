using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls;
using StikyNotes.Utils.HotKeyUtil;
using ComboBoxItem = System.Windows.Controls.ComboBoxItem;

namespace StikyNotes
{
    public class SettingViewModel
    {
        public ProgramData Datas { get; set; }

        public  EKey ShortCut { get; set; }
        public RelayCommand<bool> IsTopMostChangedCommand { get; set; }

        public RelayCommand<bool> IsStartUpWithSystemChangedCommand { get; set; }
        public RelayCommand<SelectionChangedEventArgs> SelectionChangedCommand { get; set; }

        public SettingViewModel()
        {
            ShortCut = new EKey();
            Datas=ProgramData.Instance;
            IsTopMostChangedCommand=new RelayCommand<bool>(IsTopMostChangedMethod);
            IsStartUpWithSystemChangedCommand=new RelayCommand<bool>(IsStartUpWithSystemChangedMethod);
            SelectionChangedCommand=new RelayCommand<SelectionChangedEventArgs>(SelectionChangedMethod);

        }

        /// <summary>
        /// 选择主题发生改变
        /// </summary>
        /// <param name="obj"></param>
        private void SelectionChangedMethod(SelectionChangedEventArgs e)
        {
            var content=e.AddedItems[0] as ComboBoxItem;
            
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
            
            if (param== false)
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
