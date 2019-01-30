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
        public Point StartUpPosition { get; set; }

        /// <summary>
        /// 窗体宽度
        /// </summary>
        public int WindowsWidth { get; set; }

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
            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            StartUpPosition =new Point((screenWidth - WindowsWidth)/2,(screenHeight - WindowsHeight)/2);
            RichTextBoxContent = "测试是否读取到后台数据";
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
