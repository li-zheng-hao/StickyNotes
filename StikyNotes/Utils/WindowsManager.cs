using System.Collections.Generic;

namespace StikyNotes
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
