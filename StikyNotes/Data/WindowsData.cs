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
    public class WindowsData
    {

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


        [XmlElement("LeftTopWinPosition")] public Point StartUpPosition;


        /// <summary>
        /// 窗体宽度
        /// </summary>
        public int WindowsWidth;

        public int WindowsHeight;


        private string richTextBoxContent;

        [XmlElement("UserInputNotes")] 
        public string RichTextBoxContent;
        public WindowsData()
        {
            WindowsWidth = 350;
            WindowsHeight = 450;
            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            StartUpPosition =new Point((screenWidth - WindowsWidth)/2,(screenHeight - WindowsHeight)/2);
        }
    }
}
