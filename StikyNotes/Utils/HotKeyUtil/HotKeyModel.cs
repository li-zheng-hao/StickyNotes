using System;

namespace StikyNotes.Utils.HotKeyUtil
{
    /// <summary>
    /// 快捷键模型
    /// </summary>
    public class HotKeyModel
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
    }
}
