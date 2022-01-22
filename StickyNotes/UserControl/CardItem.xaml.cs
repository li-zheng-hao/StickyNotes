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
           
            RoutedEventArgs args =new RoutedEventArgs(DeleteNoteEvent,this);
            this.RaiseEvent(args);
        }



        public bool IsShowOnScreen
        {
            get { return (bool)GetValue(IsShowOnScreenProperty); }
            set { SetValue(IsShowOnScreenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsShowOnScreen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsShowOnScreenProperty =
            DependencyProperty.Register("IsShowOnScreen", typeof(bool), typeof(CardItem), new PropertyMetadata(true));


        /// <summary>
        /// 声明删除事件
        /// 参数:要注册的路由事件名称，路由事件的路由策略，事件处理程序的委托类型(可自定义)，路由事件的所有者类类型
        /// </summary>
        public static readonly RoutedEvent DeleteNoteEvent = EventManager.RegisterRoutedEvent("DeleteNoteEvent", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventArgs<Object>), typeof(System.Windows.Controls.UserControl));
        /// <summary>
        /// 处理各种路由事件的方法 
        /// </summary>
        public event RoutedEventHandler DeleteNoteClick
        {
            //将路由事件添加路由事件处理程序
            add { AddHandler(DeleteNoteEvent, value); }
            //从路由事件处理程序中移除路由事件
            remove { RemoveHandler(DeleteNoteEvent, value); }
        }
    }
}
