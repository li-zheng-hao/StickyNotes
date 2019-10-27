using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Threading;
using System.Timers;
using System.Windows.Input;

namespace StikyNotes.Utils
{
    class Win32
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out POINT pt);
    }
    public class WindowHide
    {
        public bool IsHide = false;
        private System.Windows.Forms.Timer timer;
        public MainWindow win;
        
        public WindowHide(MainWindow win)
        {
            this.win = win;
            timer = new System.Windows.Forms.Timer(); ;
            timer.Interval = 250;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        /// <summary>
        /// 判断定时器是否停止
        /// </summary>
        /// <returns></returns>
        public bool IsStop()
        {
            return !timer.Enabled;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!win.IsLoaded)
                {
                    timer.Stop();
                    timer.Enabled = false;
                }
                Win32.POINT point = new Win32.POINT();
                Win32.GetCursorPos(out point);
                System.Windows.Point mousePositionInApp = Mouse.GetPosition(win);
                System.Windows.Point mousePositionInScreenCoordinates = win.PointToScreen(mousePositionInApp);
                Console.WriteLine("Screen:" + mousePositionInScreenCoordinates);
                //Console.WriteLine(win.Width+"====="+win.ActualWidth);
                //Console.WriteLine(win.Height);
                //Point point = PointToScreen(System.Windows.Input.Mouse.GetPosition(this));//获取鼠标相对桌面的位置
                //Console.WriteLine(win.Top);
                //Console.WriteLine(point.X + "----" + point.Y+"宽度从"+win.Left+"到"+(win.Left+win.ActualWidth));
                //窗体已经在上边缘了
                if (this.win.Top <= 1)
                {
                    //窗体已经隐藏了
                    if (IsHide)
                    {
                        //鼠标在窗体上边缘了
                        if (point.Y <= 2)
                        {
                            Console.WriteLine("显示");
                            DoubleAnimation animation = new DoubleAnimation();
                            animation.From = 0;
                            animation.To = 1;
                            animation.Duration = new Duration(TimeSpan.FromSeconds(0.2));
                            animation.Completed += (se, es) =>
                            {
                                this.win.Visibility = Visibility.Visible;
                                this.win.Activate();
                                IsHide = false;
                            };
                            win.BeginAnimation(MainWindow.OpacityProperty, animation);
                        }
                    }
                    //窗体没有隐藏
                    else
                    {
                        //鼠标离开了
                        if (point.Y > 50)
                        {
                            Console.WriteLine("隐藏");
                            //在使用
                            if (mousePositionInScreenCoordinates.X > 0.1 && mousePositionInScreenCoordinates.Y > 0.1)
                                return;
                            IsHide = true;
                            DoubleAnimation animation = new DoubleAnimation();
                            animation.From = 1;
                            animation.To = 0;
                            animation.Duration = new Duration(TimeSpan.FromSeconds(0.2));
                            animation.Completed += (se, es) =>
                            {
                                this.win.Visibility = Visibility.Hidden;
                                IsHide = true;
                            };
                            win.BeginAnimation(MainWindow.OpacityProperty, animation);

                        }


                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Log("WindowHide.cs").Error("隐藏窗体模块出现异常 "+ex.Message);
            }
           
        }

        /// <summary>
        /// 暂停多久时间后再继续执行
        /// </summary>
        /// <param name="time">毫秒</param>
        public void Stop()
        {
            timer.Enabled = false;
        }

        public void Start()
        {
            timer.Enabled = true;
        }
        
    }

    public class WindowHideManager
    {
        private static WindowHideManager instance=new WindowHideManager();

        public static WindowHideManager GetInstance()
        {
            return instance;
        }

        private WindowHideManager()
        {
            windowHideList=new List<WindowHide>();

        }
        public List<WindowHide> windowHideList;


        private System.Windows.Forms.Timer timer;
        /// <summary>
        /// 暂停所有隐藏动作，在一定时间后重新进行
        /// </summary>
        /// <param name="time"></param>
        public void StopAllHideAction(int time)
        {
            windowHideList.RemoveAll(n=>n.IsStop());

            foreach (var winHide in WindowHideManager.GetInstance().windowHideList)
            {
                winHide.Stop();

                winHide.win.Activate();
                //                            win.Opacity = 1.0;
                DoubleAnimation animation = new DoubleAnimation();
                animation.From = 0.99;
                animation.To = 1;
                animation.Duration = new Duration(TimeSpan.FromSeconds(0.05));
                animation.Completed += (se, es) =>
                {
                    winHide.win.Visibility = Visibility.Visible;
                    winHide.win.Activate();
                    winHide.IsHide = false;
                };
                winHide.win.BeginAnimation(MainWindow.OpacityProperty, animation);

            }
            timer = new System.Windows.Forms.Timer();
            timer.Interval = time;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            foreach (var windowHide in windowHideList)
            {
                windowHide.Start();
            }

            timer.Stop();
        }

       
    }


}
