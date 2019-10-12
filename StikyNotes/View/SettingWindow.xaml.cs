using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using StikyNotes.Utils.HotKeyUtil;

namespace StikyNotes
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : MetroWindow
    {
        public SettingWindow()
        {
            InitializeComponent();
        }

       


        //        private void UIElement_OnKeyUp(object sender, KeyEventArgs e)
        //        {
        //            if (e.Key == Key.LeftAlt || e.Key == Key.RightAlt)
        //            {
        //                return;
        //            }
        //
        //            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
        //            {
        //                return;
        //            }
        //
        //            if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
        //            {
        //                return;
        //            }
        //
        //            bool useCtrl = (e.KeyboardDevice.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;
        //            bool useAlt = (e.KeyboardDevice.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt;
        //            bool useShift = (e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift;
        //            //清除当前快捷键
        //            //todo
        //            //检测输入的快捷键是否可用
        //            //todo
        //            //将更新的快捷键输入文本框
        //            this.ShowAllTB.Text = "";
        //            this.ShowAllTB.Text += useCtrl ? "Ctrl+" : "";
        //            this.ShowAllTB.Text += useShift ? "Shift+" : "";
        //            this.ShowAllTB.Text += useAlt ? "Alt+" : "";
        //            this.ShowAllTB.Text += e.Key.ToString();
        //            EKey useKey = EKey.Q;
        //            foreach (int v in Enum.GetValues(typeof(EKey)))
        //            {
        //                string keyName = Enum.GetName(typeof(EKey), v);
        //                if (e.Key.ToString() == keyName)
        //                {
        //                    useKey = (EKey) v;
        //                }
        //            }
        //            var hotKeyList = new ObservableCollection<HotKeyModel>
        //            {
        //                new HotKeyModel() { IsSelectCtrl = useCtrl, IsSelectAlt = useAlt, IsSelectShift = useShift, IsUsable = true, Name = EHotKeySetting.ShowAllWindow.ToString(), SelectKey = useKey }
        //            };
        //
        //            if (!HotKeySettingsManager.Instance.RegisterGlobalHotKey(hotKeyList))
        //                MessageBox.Show("快捷键注册失败，可能与其他软件存在冲突");
        //        
        //        }
        
    }
}