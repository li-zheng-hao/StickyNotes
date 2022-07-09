using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StickyNotes.Utils.HotKeyUtil
{
    public class HotKeyHandler
    {
        public static  void HandlePress(object sender, HotkeyEventArgs e)
        {
            switch (e.Type)
            {
                case HotKeyType.ShowOrHideAllWindow:
                    ActivateOrHideAllNotTopMostWindow();
                    break;
            }

        }
        /// <summary>
        /// 显示所有窗体或者隐藏所有窗体
        /// </summary>
        private static void ActivateOrHideAllNotTopMostWindow()
        {
            foreach (var window in WindowsManager.Instance.Windows)
            {
                if (window.viewModel.Datas.IsCurrentWindowTopMost == false)
                {
                    if (ProgramData.Instance.IsWindowVisible)
                        window.Visibility = Visibility.Hidden;
                    else
                    {
                        window.Visibility = Visibility.Visible;
                        window.Activate();
                    }

                }
            }

            ProgramData.Instance.IsWindowVisible = !ProgramData.Instance.IsWindowVisible;

        }
    }
}
