using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace StickyNotes.Utils
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
        public bool IsWindowDestroyed = false;
        public bool IsStoped = false;
        public WindowHide(MainWindow win)
        {
            this.win = win;
            timer = new System.Windows.Forms.Timer(); ;
            timer.Interval = 250;
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        /// <summary>
        /// 判断窗体是否销毁了
        /// </summary>
        /// <returns></returns>
        public bool IsStop()
        {
            return IsWindowDestroyed;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!win.IsLoaded)
                {
                    timer.Stop();
                    IsWindowDestroyed = true;
                    timer.Enabled = false;
                    return;
                }
                Win32.POINT point = new Win32.POINT();
                Win32.GetCursorPos(out point);
                System.Windows.Point mousePositionInApp = Mouse.GetPosition(win);
                System.Windows.Point mousePositionInScreenCoordinates = win.PointToScreen(mousePositionInApp);
                // Console.WriteLine("-------------------");
                // Console.WriteLine("鼠标 " + point.X + ":" + point.Y);
                //
                // Console.WriteLine("窗体 " + this.win.Left);

                point = CheckDPI(point);
                ////Console.WriteLine("Screen:" + mousePositionInScreenCoordinates);
                //Console.WriteLine(win.Width+"====="+win.ActualWidth);
                //Console.WriteLine(win.Height);
                //Console.WriteLine(win.Top);
                //Console.WriteLine(point.X + "----" + point.Y+"宽度从"+win.Left+"到"+(win.Left+win.ActualWidth));
                //窗体已经在上边缘了
                if (this.win.Top <= 1)
                {
                    //窗体已经隐藏了
                    if (IsHide)
                    {
                        //鼠标在窗体上边缘了
                        if (point.Y <= 2 &&
                            point.X > this.win.Left &&
                            point.X < (this.win.Left + this.win.Width))
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
            catch (Exception ex)
            {
                Logger.Log("WindowHide.cs").Error("隐藏窗体模块出现异常 " + ex.Message);
                timer.Stop();
            }

        }
        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        public enum DeviceCap
        {
            VERTRES = 10,
            DESKTOPVERTRES = 117,
        }


        private float getScalingFactor()
        {
            Graphics g = Graphics.FromHwnd(IntPtr.Zero);
            IntPtr desktop = g.GetHdc();
            int LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
            int PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);

            float ScreenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;

            return ScreenScalingFactor; // 1.25 = 125%
        }
        private Win32.POINT CheckDPI(Win32.POINT point)
        {
            try
            {
                Matrix m = PresentationSource.FromVisual(Application.Current.MainWindow).CompositionTarget.TransformToDevice;
                double dx = m.M11; // notice it's divided by 96 already
                double dy = m.M22; // notice it's divided by 96 already
                point.X = (int)(point.X / dx);
                point.Y = (int)(point.Y / dy);
                // Console.WriteLine("鼠标修改后 " + point.X + ":" + point.Y);
                return point;
            }
            catch (Exception)
            {
                throw;
            }
         
        }

        /// <summary>
        /// 暂停多久时间后再继续执行
        /// </summary>
        /// <param name="time">毫秒</param>
        public void Stop()
        {
            timer.Stop();
            IsStoped = true;
        }

        public void Start()
        {
            IsStoped = false;
            timer.Start();
        }

    }

    public class WindowHideManager
    {
        private static WindowHideManager instance = new WindowHideManager();

        public static WindowHideManager GetInstance()
        {
            return instance;
        }

        private WindowHideManager()
        {
            windowHideList = new List<WindowHide>();

        }
        public List<WindowHide> windowHideList;


        private System.Windows.Forms.Timer timer;
        /// <summary>
        /// 暂停所有隐藏动作，在一定时间后重新进行
        /// </summary>
        /// <param name="time"></param>
        public void StopAllHideAction(int time)
        {
            windowHideList.RemoveAll(n => n.IsWindowDestroyed);
            foreach (var winHide in WindowHideManager.GetInstance().windowHideList)
            {
                if (winHide.IsStoped == false)
                {
                    winHide.Stop();
                }

                winHide.win.Activate();
                //                            win.Opacity = 1.0;
                DoubleAnimation animation = new DoubleAnimation();
                animation.From = 0.99;
                animation.To = 1;
                animation.Duration = new Duration(TimeSpan.FromSeconds(0.01));
                animation.Completed += (se, es) =>
                {
                    winHide.win.Visibility = Visibility.Visible;
                    winHide.win.Activate();
                    winHide.IsHide = false;
                };
                winHide.win.BeginAnimation(MainWindow.OpacityProperty, animation);

            }
            if (timer == null)
            {
                timer = new System.Windows.Forms.Timer();
                timer.Interval = time;
                timer.Tick += Timer_Tick;
                timer.Start();
            }
            else
            {

                timer.Start();
            }

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            foreach (var windowHide in windowHideList)
            {
                windowHide.Start();
            }

            timer.Stop();
            timer.Enabled = false;
        }


    }


}
