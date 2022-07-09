using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Lang
{
    public class LangEN : LangBase
    {
        public override string StartUpArgsError { get; set; } = "Start up args error";


        public override string UpdateWindowTitle { get; set; } = "Update Info";
        public override string ConfirmUpdate { get; set; } = "Begin Update";
        public override string CloseUpdate { get; set; } = "Close";
        public override string UpdateWindowLeftTopTitle { get; set; } = "Check Update";
    }
}
