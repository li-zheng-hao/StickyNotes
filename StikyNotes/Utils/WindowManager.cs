using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StikyNotes
{
    /// <summary>
    /// 用来管理所有打开了的窗体
    /// </summary>
    public class WindowsManager
    {
        public List<MainWindow> Windows { get; set; }

        #region 单例

        public static WindowsManager Instance = null;

        static WindowsManager()
        {
            Instance=new WindowsManager();
        }
        #endregion
        private WindowsManager()
        {
            Windows=new List<MainWindow>();
        }


    }
}
