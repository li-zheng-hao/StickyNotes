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
using System.Diagnostics;
using System.IO;
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
        public RelayCommand CloseCommand { get; }
        #region 进度条相关属性

        public int ProgressValue { get; set; } = 0;
        public string ProgressLabel { get; set; } = "0%";
        public Visibility UpdateProgressVisible { get; set; } = Visibility.Collapsed;

        /// <summary>
        /// 只有从服务器获取到更新的文件地址后才允许开始下载更新
        /// </summary>
        public bool CanDownloadFlag { get; set; } = false;
        /// <summary>
        /// 更新补丁文件下载地址
        /// </summary>
        public string DownloadFileUrl { get;private set; }
        /// <summary>
        /// 更新文件路径
        /// </summary>
        public string UpdatePatchFilePath { get; private set; }
        public DownloadFileHelper DownloadFileHelper { get;private set; }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public LangHelper LangHelper { get; set; } = LangHelper.Instance;


        public ObservableCollection<SoftwareUpdate> SoftwareInfoList { get; set; } = new ObservableCollection<SoftwareUpdate>();

        public MainWindowViewModel(Window window)
        {
            this.window = window;
            this.window.Loaded += Window_Loaded;
            ClickUpdateCommand= new RelayCommand(ClickUpdateMethod);
            CloseCommand = new RelayCommand(CloseWindowMethod);
        }

        private void CloseWindowMethod()
        {
            this.window.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CheckUpdate(majorVersionNumber, minorVersionNumber, revisionNumebr);
            
        }

        public void SetVersion(int majorVersionNumber,int minorVersionNumber,int revisionNumebr)
        {
            this.majorVersionNumber = majorVersionNumber;
            this.minorVersionNumber = minorVersionNumber;
            this.revisionNumebr = revisionNumebr;
        }
     

        private void ClickUpdateMethod()
        {

            Process[] proc=Process.GetProcessesByName("StickyNotes");
            if (proc.Length > 0)
            {
                proc[0].Kill();
            } 
            var fileName = Common.FileHelper.GetFileName(this.DownloadFileUrl);
            var fileDir = Environment.CurrentDirectory;
            UpdatePatchFilePath = Path.Combine(fileDir, fileName);
            this.DownloadFileHelper = new DownloadFileHelper(this.DownloadFileUrl, UpdatePatchFilePath);
            this.DownloadFileHelper.ProgressChanged += DownloadFileHelper_ProgressChanged;
            this.DownloadFileHelper.ProcessCompleted += DownloadFileHelper_ProcessCompleted;
            this.DownloadFileHelper.DownLoadStart();
            this.UpdateProgressVisible = Visibility.Visible;


        }
        /// <summary>
        /// 下载完成
        /// </summary>
        private void DownloadFileHelper_ProcessCompleted()
        {
            try
            {
                this.ProgressValue = 100;
                this.ProgressLabel = "100%";
                // 直接覆盖解压
                
                Common.FileHelper.Decompress(UpdatePatchFilePath,ComUtil.GetParentDirectory(Environment.CurrentDirectory));
                if (File.Exists(this.UpdatePatchFilePath))
                    File.Delete(this.UpdatePatchFilePath);
                this.UpdatePatchFilePath = string.Empty;
                 var dir=Common.ComUtil.GetParentDirectory(Environment.CurrentDirectory);
                var version = JsonHelper.ReadVersionFromFile(dir,UpdateApp.Properties.Resources.VersionFileName);
                var software=SoftwareInfoList.FirstOrDefault();
                version.StickyNotesVersion.MajorVersionNumber = software.major_version_number;
                version.StickyNotesVersion.MinorVersionNumber = software.minor_version_number;
                version.StickyNotesVersion.RevisionNumebr = software.revision_number;
                JsonHelper.WriteVersionToFile(version,dir, UpdateApp.Properties.Resources.VersionFileName);
                var parentPath=ComUtil.GetParentDirectory(Environment.CurrentDirectory);
                Process.Start(Path.Combine(parentPath, "StickyNotes.exe"));
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                
            }
           

        }
        /// <summary>
        /// 下载进度更新
        /// </summary>
        /// <param name="totalNum"></param>
        /// <param name="num"></param>
        /// <param name="proc"></param>
        /// <param name="speed"></param>
        /// <param name="remainTime"></param>
        /// <param name="outMsg"></param>
        /// <param name="fileName"></param>
        private void DownloadFileHelper_ProgressChanged(string totalNum, string num, int proc, string speed, string remainTime, string outMsg, string fileName)
        {
            this.ProgressValue = proc;
            this.ProgressLabel =proc.ToString() + "%";
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
            if (res!=null&&res.success)
            {
                List<SoftwareUpdate> software = HttpHelper.DynamicToObject<List<SoftwareUpdate>>(res.data);
                software.ForEach(it => this.SoftwareInfoList.Add(it));
                this.CanDownloadFlag = true;
                this.DownloadFileUrl = software.First().patch_file_url;
            }
            else
            {
                
                (window as MainWindow).ShowModalMessageExternal("提示", $"检查更新失败,请稍后再试");
            }
        }

    }
}
