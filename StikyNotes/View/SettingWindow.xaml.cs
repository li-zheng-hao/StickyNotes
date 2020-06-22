using MahApps.Metro.Controls;

namespace StikyNotes
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
            settingViewModel = new SettingViewModel();
            this.DataContext = settingViewModel;
        }


    }
}
