using ControlzEx.Theming;
using MahApps.Metro.Controls;

namespace StickyNotes
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : MetroWindow
    {
        public SettingViewModel settingViewModel;
        public SettingWindow()
        {
            InitializeComponent();
            ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncWithAppMode;
            ThemeManager.Current.SyncTheme();
            settingViewModel = new SettingViewModel();
            this.DataContext = settingViewModel;
        }


    }
}
