using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace StickyNotes.Utils
{
    /// <summary>
    /// 每过一段时间去执行一段代码
    /// </summary>
    public class TimerUtil
    {
        private Timer timer;
        public Action action;
        public TimerUtil(Action action,int interval= 5000)
        {
            timer=new Timer(interval);
            this.action = action;
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            action?.Invoke();
        }
    }
}
