using StikyNotes.Utils.HotKeyUtil.GlobalHotKeyDemo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StikyNotes.Utils.HotKeyUtil
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
            hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.全屏.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = false, IsSelectShift = false, SelectKey = EKey.Q });
            //hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.截图.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.Z });
            //hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.播放.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.Space });
            //hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.前进.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.D });
            //hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.后退.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.A });
            //hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.保存.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.B });
            //hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.打开.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.X });
            //hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.新建.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.H });
            //hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.删除.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.G });
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

    }
}
