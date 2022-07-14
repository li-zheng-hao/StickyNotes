using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{

    public class Config
    {
        private static string GetCurrentProjectPath
        {

            get
            {
                return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location).Replace(@"\bin\Debug", "");//获取具体路径
            }
        }
        public static string ConnectionString = @"DataSource=" + GetCurrentProjectPath + @"\data.sqlite";
    }
  
}
