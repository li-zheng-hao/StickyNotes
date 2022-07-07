using Common;
using Common.Lang;
using Contract;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UpdateApp
{
   
    public class MainWindowViewModel: ViewModelBase
    {
        private Window window;
        private int majorVersionNumber;
        private int minorVersionNumber;
        private int revisionNumebr;
        public RelayCommand ClickUpdateCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public LangHelper LangHelper { get; set; } = LangHelper.Instance;

        public Visibility UpdateProgressVisible { get; set; } = Visibility.Collapsed;

        public ObservableCollection<SoftwareUpdate> SoftwareInfoList { get; set; } = new ObservableCollection<SoftwareUpdate>();

        public MainWindowViewModel(Window window)
        {
            this.window = window;
            this.window.Activated += Window_Activated;
            ClickUpdateCommand= new RelayCommand(ClickUpdateMethod);
        }

        public void SetVersion(int majorVersionNumber,int minorVersionNumber,int revisionNumebr)
        {
            this.majorVersionNumber = majorVersionNumber;
            this.minorVersionNumber = minorVersionNumber;
            this.revisionNumebr = revisionNumebr;
        }
        private void Window_Activated(object sender, EventArgs e)
        {
            CheckUpdate(majorVersionNumber, minorVersionNumber, revisionNumebr);
        }

        private void ClickUpdateMethod()
        {
            
        }
        /// <summary>
        /// 获取比当前版本更高的更新信息
        /// </summary>
        /// <param name="majorVersionNumber"></param>
        /// <param name="minorVersionNumber"></param>
        /// <param name="revisionNumebr"></param>
        private async void CheckUpdate(int majorVersionNumber, int minorVersionNumber, int revisionNumebr)
        {
            var res = HttpHelper.HttpGet("api/Software/GetLastedVersionByVersion",
               new string[] { "softwarename", "majorVersionNumber", "minorVersionNumber", "revisionNumebr" },
               new object[] { "stickynotes", majorVersionNumber, minorVersionNumber, revisionNumebr });
            if (res.success)
            {
                List<SoftwareUpdate> software = HttpHelper.DynamicToObject<List<SoftwareUpdate>>(res.data);
                software.ForEach(it => this.SoftwareInfoList.Add(it));
                software.ForEach(it => this.SoftwareInfoList.Add(it));
                software.ForEach(it => this.SoftwareInfoList.Add(it));
                software.ForEach(it => this.SoftwareInfoList.Add(it));
                software.ForEach(it => this.SoftwareInfoList.Add(it));
                software.ForEach(it => this.SoftwareInfoList.Add(it));
                await (window as MainWindow).ShowMessageAsync("提示", "检查到新版本，是否需要更新?");
            }
            else
            {
                if (string.IsNullOrEmpty(res.message))
                {
                    (window as MainWindow).ShowModalMessageExternal("提示", $"检查更新失败,请稍后再试");
                }
                else
                {
                    (window as MainWindow).ShowModalMessageExternal("提示", $"检查更新失败,失败原因:{res.message}");
                }
            }
        }

    }
}
