using StickyNotes.Annotations;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Xml.Serialization;
using GalaSoft.MvvmLight.Messaging;

namespace StickyNotes
{
    public class WindowsData : INotifyPropertyChanged
    {
        /// <summary>
        /// 文档创建的ID，以时间戳为准
        /// </summary>
        public string WindowID { get; set; }
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
        /// 文本框是否获得了焦点
        /// </summary>
        public bool IsFocused { get; set; }
        /// <summary>
        /// 窗体宽度
        /// </summary>
        public int WindowsWidth { get; set; }


        /// <summary>
        /// 窗体高度
        /// </summary>
        public int WindowsHeight { get; set; }

        /// <summary>
        /// 文本框内容 用于存储到数据库使用的字符串 显示会乱码
        /// </summary>
        public string RichTextBoxContent { get; set; }

        /// <summary>
        /// 用于在界面上展示的便签内容
        /// </summary>
        public string DisplayRichTextBoxContent { get; set; }
        /// <summary>
        /// 窗体是否置顶
        /// </summary>
        public bool IsCurrentWindowTopMost { get; set; }

        /// <summary>
        /// 当前窗口存储的文件路径
        /// </summary>
        public string DocumentFileName { get; set; }

        /// <summary>
        /// 窗体是否已经显示
        /// </summary>
        public bool IsShowed { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime ModifiedTime { get; set; }
        /// <summary>
        /// 默认初始化数据
        /// </summary>
        public WindowsData()
        {
            WindowID = DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() +
                       DateTime.Now.Day.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" +
                       DateTime.Now.Second.ToString() + "-" +
                       DateTime.Now.Millisecond.ToString();
            DocumentFileName = WindowID;
            WindowsWidth = 300;
            WindowsHeight = 300;
            FontSize = 14;
            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            StartUpPositionLeft = (screenWidth - WindowsWidth) / 2;
            StartUpPositionTop = (screenHeight - WindowsHeight) / 2;
            RichTextBoxContent = string.Empty;
            DisplayRichTextBoxContent = string.Empty;
            IsCurrentWindowTopMost = false;
            IsFocused = true;
            IsShowed = true;
            ModifiedTime = DateTime.Now;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
