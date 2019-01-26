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
    public class WindowManager
    {
        public List<MainWindow> Windows { get; set; }

        #region 单例

        public static WindowManager Instance = null;

        static WindowManager()
        {
            Instance=new WindowManager();
        }
        #endregion
        private WindowManager()
        {

        }


    }
}
