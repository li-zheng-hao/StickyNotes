
using Common;
using Contract;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StickyNotes.Utils
{
    /// <summary>
    /// 用于检查更新工具的版本是否需要更新
    /// </summary>
    public class UpdateHelper
    {
        public static string ServerUrl = StickyNotes.Properties.Resources.ServerUrl;

        public static string DownloadFileUrl { get; private set; }
        public static string UpdatePatchFilePath { get; private set; }
        public DownloadFileHelper DownloadFileHelper { get; private set; }
        /// <summary>
        /// 更新工具是否检查更新完成
        /// </summary>
        public static bool UpdateToolUpdated { get; set; } = false;

        public event Action UpdateToolCompleted;
        /// <summary>
        /// 更新 更新工具
        /// </summary>
        /// <param name="majorVersionNumber"></param>
        /// <param name="minorVersionNumber"></param>
        /// <param name="revisionNumebr"></param>
        public void UpdateUpdateTool()
        {
            Common.Version version = JsonHelper.ReadVersionFromFile(StickyNotes.Properties.Resources.VersionFileName);
            var res = HttpHelper.HttpGet("api/Software/GetLastedVersionByVersion",
               new string[] { "softwarename", "majorVersionNumber", "minorVersionNumber", "revisionNumebr" },
               new object[] { "updateapp", version.UpdateAppVersion.MajorVersionNumber, version.UpdateAppVersion.MinorVersionNumber, version.UpdateAppVersion.RevisionNumebr });
            if (res.success)
            {
                List<SoftwareUpdate> software = HttpHelper.DynamicToObject<List<SoftwareUpdate>>(res.data);
                DownloadFileUrl = software.First().patch_file_url;
                var fileName = Common.FileHelper.GetFileName(DownloadFileUrl);
                var fileDir = Environment.CurrentDirectory;
                UpdatePatchFilePath = Path.Combine(fileDir, fileName);
                DownloadFileHelper = new DownloadFileHelper(DownloadFileUrl, UpdatePatchFilePath);
                DownloadFileHelper.ProgressChanged += DownloadFileHelper_ProgressChanged;
                DownloadFileHelper.ProcessCompleted += DownloadFileHelper_ProcessCompleted;
                DownloadFileHelper.DownLoadStart();
            }
            else
            {
                UpdateToolUpdated = true;
            }
        }

        private  void DownloadFileHelper_ProcessCompleted()
        {
            Common.FileHelper.Decompress(UpdatePatchFilePath, Environment.CurrentDirectory);
            if (File.Exists(UpdatePatchFilePath))
                File.Delete(UpdatePatchFilePath);
            UpdateToolUpdated = true;
            UpdateToolCompleted?.Invoke();
        }

        private  void DownloadFileHelper_ProgressChanged(string totalNum, string num, int progress, string speed, string remainTime, string outMsg, string fileName)
        {
            
        }
        /// <summary>
        /// 检查自己的版本是否需要更新
        /// </summary>
        /// <returns></returns>
        public  bool CheckSelfNeedUpdate()
        {
            Common.Version version = JsonHelper.ReadVersionFromFile(StickyNotes.Properties.Resources.VersionFileName);
            HttpHelper.BaseUrl = ServerUrl;
            var res=HttpHelper.HttpGet("api/Software/GetLastedVersion", new string[] { "softwarename" }, new object[] { "stickynotes" });
            if (res.success)
            {
                List<SoftwareUpdate> software = HttpHelper.DynamicToObject<List<SoftwareUpdate>>(res.data);
                var lastedSoftwareUpdate = software.FirstOrDefault();
                long remoteVersion = lastedSoftwareUpdate.major_version_number * 1000000 + lastedSoftwareUpdate.minor_version_number * 1000 + lastedSoftwareUpdate.revision_number;
                long localVersion = version.StickyNotesVersion.MajorVersionNumber * 1000000 + version.StickyNotesVersion.MinorVersionNumber * 1000 +
                    version.StickyNotesVersion.RevisionNumebr;
                return remoteVersion > localVersion ? true : false;

            }
            return false;
        }
        /// <summary>
        /// 打开更新工具，然后关闭自己
        /// </summary>
        public  void OpenUpdateTool()
        {
            try
            {
                if (UpdateToolUpdated == false) 
                    return;
                Process.Start(Path.Combine(Environment.CurrentDirectory, "UpdateApp.exe"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Logger.Log().Error(ex.Message);
            }
         
        }
    }
}
