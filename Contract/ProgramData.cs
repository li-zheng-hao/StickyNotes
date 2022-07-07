
using Common.Lang;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Contract
{
    public class ProgramData
    {
        /// <summary>
        /// 系统语言
        /// </summary>
        public Language Language { get; set; }
        /// <summary>
        /// 窗体是否置顶
        /// </summary>
        public bool IsWindowTopMost { get; set; }
        /// <summary>
        /// 程序是否自动检查更新
        /// </summary>
        public bool IsAutoCheckUpdate { get; set; }

        /// <summary>
        /// 日间或夜间
        /// </summary>
        public string BaseTheme { get; set; }
        /// <summary>
        /// 窗体主题颜色
        /// </summary>
        public string CurrentTheme { get; set; }

        /// <summary>
        /// 是否开机自启动
        /// </summary>
        public bool IsStartUpWithSystem { get; set; }

        public bool IsWindowVisible { get; set; } = true;



       
    }
}
