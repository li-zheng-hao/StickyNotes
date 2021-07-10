using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;

namespace StickyNotes.Utils.HotKeyUtil
{
    /// <summary>
    /// 快捷键设置管理器
    /// </summary>
    public class HotKeySettingsManager
    {
        private static HotKeySettingsManager m_Instance;
        /// <summary>
        /// 单例实例
        /// </summary>
        public static HotKeySettingsManager Instance
        {
            get { return m_Instance ?? (m_Instance = new HotKeySettingsManager()); }
        }

        /// <summary>
        /// 加载默认快捷键
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<HotKeyModel> LoadDefaultHotKey()
        {
            var hotKeyList = new ObservableCollection<HotKeyModel>();
            hotKeyList.Add(ProgramData.Instance.ShowAllHotKey);
            return hotKeyList;
        }

        /// <summary>
        /// 通知注册系统快捷键委托
        /// </summary>
        /// <param name="hotKeyModelList"></param>
        public delegate bool RegisterGlobalHotKeyHandler(ObservableCollection<HotKeyModel> hotKeyModelList);
        public event RegisterGlobalHotKeyHandler RegisterGlobalHotKeyEvent;
        public bool RegisterGlobalHotKey(ObservableCollection<HotKeyModel> hotKeyModelList)
        {
            if (RegisterGlobalHotKeyEvent != null)
            {
                return RegisterGlobalHotKeyEvent(hotKeyModelList);
            }
            return false;
        }

        /// <summary>
        /// 由于多个窗体只需要注册一次快捷键，因此如果有一个窗体绑定了就不需要再注册
        /// </summary>
        public bool IsShowAllWindowHotKeyRegistered { get; set; } = false;

        /// <summary>
        /// 由于多个窗体只需要注册一次快捷键，但是要更改快捷键的时候也需要进行重新注册
        /// </summary>
        public bool IsShowAllWindowHotKeyNeedChanged { get; set; } = false;
    }
}
