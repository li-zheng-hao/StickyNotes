using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Lang
{
    public class LangCN : LangBase
    {
        public override string StartUpArgsError { get; set; } = "启动参数错误";

        public override string UpdateWindowTitle { get; set; } = "更新内容";
        public override string ConfirmUpdate { get; set; } = "开始更新";
        public override string CloseUpdate { get; set; } = "关闭";
        public override string UpdateWindowLeftTopTitle { get; set; } = "检查更新";
    }
}
