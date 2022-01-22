using ControlzEx.Theming;
using MahApps.Metro.Controls;
using StickyNotes.Utils;
using System.Windows;

namespace StickyNotes
{

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainViewModel viewModel;
        //构造函数
        public MainWindow()
        {

            InitializeComponent();
            ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncWithAppMode;
            ThemeManager.Current.SyncTheme();
            viewModel = new MainViewModel();
            this.DataContext = viewModel;
            WindowHide windowHide = new WindowHide(this);
            WindowHideManager.GetInstance().windowHideList.Add(windowHide);
        }


    }
}
