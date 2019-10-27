using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace StikyNotes.Utils
{
    public static class Logger
    {
        public static ILog Log(string LoggerName="")
        {
            if (LoggerName.Equals(""))
            {
                return LogManager.GetLogger((MethodBase.GetCurrentMethod().DeclaringType));

            }
            return LogManager.GetLogger(LoggerName);
        }
    }

}

