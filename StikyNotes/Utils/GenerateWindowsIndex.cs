using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StikyNotes
{
    /// <summary>
    /// 产生窗体索引
    /// </summary>
    public static class GenerateWindowsIndex
    {
        private static int Num=0;
        //todo 这里要根据日期来生成窗体的编号
        public static int Generate()
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            rand.Next();
            return Num;
        }
    }
}
