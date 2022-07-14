using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Common
{
    public class ComUtil
    {
        /// <summary>
        /// get random guid string
        /// </summary>
        /// <returns></returns>
        public static string GetRandomGuid()
        {
            return Guid.NewGuid().ToString();
        }

        // 获取目录的上一级
        public static string GetParentDirectory(string path)
        {
            return System.IO.Path.GetDirectoryName(path);
        }
        public static string GetCurrentExecDirectory()
        {
            return Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
        }
    }
}
