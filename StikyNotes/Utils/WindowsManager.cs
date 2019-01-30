using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StikyNotes
{
    public class WindowsManager
    {
        public List<MainWindow> Windows { get; set; }

        public static WindowsManager Instance = null;

        static WindowsManager()
        {
            Instance=new WindowsManager();
            Instance.Windows=new List<MainWindow>();
        }
    }
}
