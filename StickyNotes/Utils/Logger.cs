using log4net;
using System.Reflection;

namespace StickyNotes.Utils
{
    public static class Logger
    {
        public static ILog Log(string LoggerName = "")
        {
            if (LoggerName.Equals(""))
            {
                return LogManager.GetLogger((MethodBase.GetCurrentMethod().DeclaringType));
            }
            return LogManager.GetLogger(LoggerName);
        }
    }

}
