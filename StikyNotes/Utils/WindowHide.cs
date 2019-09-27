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
        private bool IsHide = false;
        private System.Windows.Forms.Timer timer;
        MainWindow win;
        public WindowHide(MainWindow win)
        {
            this.win = win;
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 250;
            timer.Tick += new EventHandler(timer_Tick);

            timer.Start();
        }
        void timer_Tick(object sender, EventArgs e)
        {
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


    }



}
