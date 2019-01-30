using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StikyNotes
{
    [Serializable]
    public class ProgramData
    { 
        
        public List<WindowsData> Datas { get;set;}

        /// <summary>
        /// 窗体是否置顶
        /// </summary>
        public bool IsWindowTopMost { get; set; }

        public static ProgramData Instance=null;


        /// <summary>
        /// 是否开机自启动
        /// </summary>
        public bool IsStartUpWithSystem { get; set; }

        static ProgramData()
        {
            Instance=new ProgramData();
            Instance.Datas=new List<WindowsData>();
            Instance.IsWindowTopMost = false;
            Instance.IsStartUpWithSystem = false;
        }
    }
}
