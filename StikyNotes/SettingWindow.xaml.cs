using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using Microsoft.Win32;

namespace StikyNotes
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : MetroWindow
    {
        public SettingWindow()
        {
            InitializeComponent();
            Topmost = true;
            this.DataContext = ProgramData.Instance;
        }

        private void ToggleSwitch_IsCheckedChanged(object sender, EventArgs e)
        {
            var btn = sender as ToggleSwitch;
            if (btn.IsChecked == true)
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
        }

        private void ToggleSwitch_IsCheckedChanged_1(object sender, EventArgs e)
        {
            var btn = sender as ToggleSwitch;
            Microsoft.Win32.RegistryKey key =
                Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run",
                    (bool)btn.IsChecked);
            Assembly curAssembly = Assembly.GetExecutingAssembly();
            key.SetValue(curAssembly.GetName().Name, curAssembly.Location);
        }
    }
}
