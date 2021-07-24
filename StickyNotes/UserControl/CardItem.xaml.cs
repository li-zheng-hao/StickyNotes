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
using GalaSoft.MvvmLight.Messaging;
using StickyNotes.Utils.Messages;

namespace StickyNotes.UserControl
{
    /// <summary>
    /// CardItem.xaml 的交互逻辑
    /// </summary>
    public partial class CardItem : System.Windows.Controls.UserControl
    {
        public CardItem()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 点击删除当前便签
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteWindowOnClick(object sender, RoutedEventArgs e)
        {
            var windowsdata = this.DataContext as WindowsData;
            ProgramData.Instance.Datas.Remove(windowsdata);
            // 通知主窗体删除这个数据 关闭这个便签窗体
//            Messenger.Default.Send<ChangeWindowMessage>(new ChangeWindowMessage(){window = windowsdata,msgType = ChangeWindowMessageType.DeleteWindow});
        }
    }
}
