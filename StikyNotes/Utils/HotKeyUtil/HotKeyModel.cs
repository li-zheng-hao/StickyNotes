using StikyNotes.Annotations;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StikyNotes.Utils.HotKeyUtil
{
    /// <summary>
    /// 快捷键模型
    /// </summary>
    [Serializable]
    public class HotKeyModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 设置项名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 设置项快捷键是否可用
        /// </summary>
        public bool IsUsable { get; set; }

        /// <summary>
        /// 是否勾选Ctrl按键
        /// </summary>
        public bool IsSelectCtrl { get; set; }

        /// <summary>
        /// 是否勾选Shift按键
        /// </summary>
        public bool IsSelectShift { get; set; }


        /// <summary>
        /// 是否勾选Alt按键
        /// </summary>
        public bool IsSelectAlt { get; set; }



        /// <summary>
        /// 选中的按键
        /// </summary>
        public EKey SelectKey { get; set; }



        /// <summary>
        /// 快捷键按键集合
        /// </summary>
        public static Array Keys
        {
            get
            {
                return Enum.GetValues(typeof(EKey));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            var showText = "";
            showText += IsSelectCtrl ? "Ctrl+" : "";
            showText += IsSelectShift ? "Shift+" : "";
            showText += IsSelectAlt ? "Alt+" : "";
            showText += SelectKey.ToString();
            return showText;
        }


        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            Console.WriteLine("HotKeyModel发生改变");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
