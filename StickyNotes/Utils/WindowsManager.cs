using System.Collections.Generic;

namespace StickyNotes
{
    public class WindowsManager
    {
        public List<MainWindow> Windows { get; set; }

        public static WindowsManager Instance = null;
        static WindowsManager()
        {
            Instance = new WindowsManager();
            Instance.Windows = new List<MainWindow>();
        }
    }
}
