using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class Versions
    {
        [SqlSugar.SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        /// <summary>
        /// 大版本
        /// </summary>
        public int MajorVersion { get; set; }
        /// <summary>
        /// 中版本
        /// </summary>
        public int MinorVersion { get; set; }
        /// <summary>
        /// 小版本
        /// </summary>
        public int BuildVersion { get; set; }

        // get version
        public string Version
        {
            get
            {
                return string.Format("{0}.{1}.{2}", MajorVersion, MinorVersion, BuildVersion);
            }
        }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime PublishTime { get; set; }
    }
}
