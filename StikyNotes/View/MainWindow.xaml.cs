using StikyNotes.Utils;
using System.Windows;

namespace StikyNotes
{

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel viewModel;
        //构造函数
        public MainWindow()
        {

            InitializeComponent();
            viewModel = new MainViewModel();
            this.DataContext = viewModel;
            WindowHide windowHide = new WindowHide(this);
            WindowHideManager.GetInstance().windowHideList.Add(windowHide);
        }


    }
}
