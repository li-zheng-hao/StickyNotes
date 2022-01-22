using ControlzEx.Theming;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls;
using StickyNotes.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace StickyNotes.View
{
    /// <summary>
    /// ListWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ListWindow :MetroWindow
    {
      

        public ListWindow()
        {
            InitializeComponent();
            ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncWithAppMode;
            ThemeManager.Current.SyncTheme();
            ListWindowViewModel vm = new ListWindowViewModel();
            this.DataContext=vm;

        }


        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

     
    }
}
