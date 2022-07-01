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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StickyNotes.UserControl
{
    /// <summary>
    /// Interaction logic for CustomWarningDialog.xaml
    /// </summary>
    public partial class CustomWarningDialog : System.Windows.Controls.UserControl
    {
        public CustomWarningDialog()
        {
            InitializeComponent();
        }
        public event RoutedEventHandler CloseButtonClicked ;
        public event RoutedEventHandler ConfirmButtonClicked ;
        private void ConfirmClick(object sender, RoutedEventArgs e)
        {
            ConfirmButtonClicked?.Invoke(sender, e);
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            CloseButtonClicked?.Invoke(sender, e);
        }
    }
}
