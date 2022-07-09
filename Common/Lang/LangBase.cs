using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Lang
{   public abstract class LangBase:INotifyPropertyChanged
    {
        public abstract string StartUpArgsError { get; set; }
        /// <summary>
        /// 更新窗体的标题
        /// </summary>
        public abstract string UpdateWindowTitle { get; set; }
        /// <summary>
        /// 确定更新
        /// </summary>
        public abstract string ConfirmUpdate { get; set; }
        /// <summary>
        /// 关闭更新
        /// </summary>
        public abstract string CloseUpdate { get; set; }
        /// <summary>
        /// 窗体菜单栏标题
        /// </summary>

        public abstract string UpdateWindowLeftTopTitle { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
      
    }
}
