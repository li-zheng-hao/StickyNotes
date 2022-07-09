using Common.Lang;
using GalaSoft.MvvmLight.Messaging;
using StickyNotes.Annotations;
using StickyNotes.Utils;
using StickyNotes.Utils.HotKeyUtil;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace StickyNotes
{
    [Serializable]
    public class ProgramData : INotifyPropertyChanged
    {
        public ObservableCollection<WindowsData> Datas { get; set; }
        public ObservableCollection<WindowsData> HideWindowData { get; set; }

        /// <summary>
        /// 系统语言
        /// </summary>
        public Language Language { get; set; }
        /// <summary>
        /// 窗体是否置顶
        /// </summary>
        public bool IsWindowTopMost { get; set; }
        /// <summary>
        /// 程序是否自动检查更新
        /// </summary>
        public bool IsAutoCheckUpdate { get; set; }

        private static ProgramData instance = new ProgramData();

        public static ProgramData Instance
        {
            get { return instance; }
        }
        /// <summary>
        /// 显示所有窗体的快捷键
        /// </summary>
        public HotKey ShowAllHotKey { get; set; }


        /// <summary>
        /// 日间或夜间
        /// </summary>
        public string BaseTheme { get; set; }
        /// <summary>
        /// 窗体主题颜色
        /// </summary>
        public string CurrentTheme { get; set; }

        /// <summary>
        /// 是否开机自启动
        /// </summary>
        public bool IsStartUpWithSystem { get; set; }

        public bool IsWindowVisible { get; set; } = true;


        private ProgramData()
        {
            //            Instance = new ProgramData();
            Datas = new ObservableCollection<WindowsData>();
            HideWindowData = new ObservableCollection<WindowsData>();
            IsWindowTopMost = false;
            IsStartUpWithSystem = false;
            IsAutoCheckUpdate = false;
            Datas.CollectionChanged += Datas_CollectionChanged;
            BaseTheme = "Light";
            Language = Language.Chinese;
            CurrentTheme = "冷灰";
            ShowAllHotKey = new HotKey(Key.Q, ModifierKeys.Control,HotKeyHandler.HandlePress,HotKeyType.ShowOrHideAllWindow);
        }

        

        private void Datas_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Messenger.Default.Send<SaveMessage>(new SaveMessage());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            Messenger.Default.Send<SaveMessage>(new SaveMessage());
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
