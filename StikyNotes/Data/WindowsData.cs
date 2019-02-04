using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Xml.Serialization;
using GalaSoft.MvvmLight.Messaging;
using StikyNotes.Annotations;
using MessageBox = System.Windows.MessageBox;

namespace StikyNotes
{
    [XmlRoot]
    public class WindowsData:INotifyPropertyChanged
    {

        /// <summary>
        /// 窗体字体
        /// </summary>
        public double FontSize { get; set; }
       
        /// <summary>
        /// 窗体启动左上角位置
        /// </summary>
        public double StartUpPositionTop { get; set; }
        public double StartUpPositionLeft { get; set; }


        /// <summary>
        /// 窗体宽度
        /// </summary>
        public int WindowsWidth { get; set; }
//        {
//            get { return windowsWidth; }
//            set
//            {
//                windowsWidth = value;
//                OnPropertyChanged();
//            }
//
//        }

        /// <summary>
        /// 窗体高度
        /// </summary>
        public int WindowsHeight { get; set; }

        /// <summary>
        /// 文本框内容
        /// </summary>
        public string RichTextBoxContent { get; set; }

        /// <summary>
        /// 默认初始化数据
        /// </summary>
        public WindowsData()
        {
            WindowsWidth = 300;
            WindowsHeight = 300;
            FontSize = 14;
            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            StartUpPositionLeft = (screenWidth - WindowsWidth)/ 2;
            StartUpPositionTop = (screenHeight - WindowsHeight) / 2;
            RichTextBoxContent = string.Empty;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            Messenger.Default.Send<SaveMessage>(new SaveMessage());
        }
    }
}
