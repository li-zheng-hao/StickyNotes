using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB
{

    public class Config
    {
        private static string GetCurrentProjectPath
        {

            get
            {
               var str= ComUtil.GetCurrentExecDirectory();
                return str;
            }
        }
        public static string ConnectionString = @"DataSource=" + GetCurrentProjectPath + @"\data.sqlite";
    }
  
}
