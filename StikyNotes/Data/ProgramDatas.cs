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

        public static ProgramData Instance=null;

        static ProgramData()
        {
            Instance=new ProgramData();
            Instance.Datas=new List<WindowsData>();
        }
    }
}
