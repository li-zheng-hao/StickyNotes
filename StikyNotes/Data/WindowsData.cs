using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Xml.Serialization;
using MessageBox = System.Windows.MessageBox;

namespace StikyNotes
{
    [XmlRoot]
    public class WindowsData:PropertyChangedBase
    {
        [XmlElement("WindowsIndex")]
        /// <summary>
        /// Windows窗体索引
        /// </summary>
        public int WindowsIndex{get;set; }

        [XmlElement("FontSize")]
        private double fontSize=14;
        /// <summary>
        /// 窗体字体
        /// </summary>
        public double FontSize
        {
            get { return fontSize;}
            set
            {
                fontSize=value;
            }
        }


        private Point startUpPosition;
        [XmlElement("LeftTopWinPosition")]
        public Point StartUpPosition
        {
            get { return startUpPosition; }
            set
            {
                startUpPosition = value;
               
                OnPropertyChanged("StartUpPosition");
            }
        }
        

        /// <summary>
        /// 窗体宽度
        /// </summary>
        private int windowsWidth;
        public int WindowsWidth
        {
            get { return windowsWidth; }
            set { windowsWidth = value;OnPropertyChanged("WindowsWidth"); }
        }

        private int windowsHeight;
        public int WindowsHeight
        {
            get { return windowsHeight; }
            set
            {
                windowsHeight = value;
                OnPropertyChanged("WindowsHeight");
            }
        }


        private string richTextBoxContent;

        [XmlElement("UserInputNotes")]
        public string RichTextBoxContent
        {
            get { return richTextBoxContent; }
            set
            {
                richTextBoxContent = value;
                OnPropertyChanged("RichTextBoxContent");
            }
        }
        public WindowsData()
        {
//            MessageBox.Show("新的窗体数据加载了");
            WindowsIndex = GenerateWindowsIndex.Generate();
            WindowsWidth = 350;
            WindowsHeight = 450;
            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            StartUpPosition =new Point((screenWidth - WindowsWidth)/2,(screenHeight - WindowsHeight)/2);
        }
    }
}
