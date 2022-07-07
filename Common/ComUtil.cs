using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
