using StickyNotes.View;
using System.Collections.Generic;

namespace StickyNotes
{
    public class WindowsManager
    {
        public List<MainWindow> Windows { get; set; }

        public static WindowsManager Instance = null;

        public static ListWindow ListWindow{ get; set; }
        public static AboutWindow AboutWindow { get; set; }

        static WindowsManager()
        {
            Instance = new WindowsManager();
            Instance.Windows = new List<MainWindow>();
        }
    }
}
