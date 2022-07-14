
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
        private Common.Version version { get; set; }

        public static string DownloadFileUrl { get; private set; }
        public static string UpdatePatchFilePath { get; private set; }
        public DownloadFileHelper DownloadFileHelper { get; private set; }
        /// <summary>
        /// 更新工具是否检查更新完成
        /// </summary>
        public static bool UpdateToolUpdated { get; set; } = false;

        public event Action UpdateToolCompleted;
        public SoftwareUpdate LatestUpdateToolVersion { get;private set; }
        /// <summary>
        /// 更新 更新工具
        /// </summary>
        /// <param name="majorVersionNumber"></param>
        /// <param name="minorVersionNumber"></param>
        /// <param name="revisionNumebr"></param>
        public void UpdateUpdateTool()
        {
            Common.Version version = JsonHelper.ReadVersionFromFile(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), StickyNotes.Properties.Resources.VersionFileName);
            var res = HttpHelper.HttpGet("api/Software/GetLastedVersion",
               new string[] { "softwarename", "majorVersionNumber", "minorVersionNumber", "revisionNumebr" },
               new object[] { "updateapp", version.UpdateAppVersion.MajorVersionNumber, version.UpdateAppVersion.MinorVersionNumber, version.UpdateAppVersion.RevisionNumebr });
            if (res!=null&&res.success)
            {
                SoftwareUpdate software = HttpHelper.DynamicToObject<SoftwareUpdate>(res.data);
                long latestVersion = software.major_version_number * 1000000 + software.minor_version_number * 1000 + software.revision_number;
                long localVersion = version.UpdateAppVersion.MajorVersionNumber * 1000000 + version.UpdateAppVersion.MinorVersionNumber * 1000 + version.UpdateAppVersion.RevisionNumebr;
                if (latestVersion <= localVersion)
                {
                    UpdateToolCompleted?.Invoke();
                    return;
                }
                LatestUpdateToolVersion = software;
                DownloadFileUrl = software.patch_file_url;
                var fileName = Common.FileHelper.GetFileName(DownloadFileUrl);
                var fileDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                UpdatePatchFilePath = Path.Combine(fileDir, fileName);
                DownloadFileHelper = new DownloadFileHelper(DownloadFileUrl, UpdatePatchFilePath);
                DownloadFileHelper.ProgressChanged += DownloadFileHelper_ProgressChanged;
                DownloadFileHelper.ProcessCompleted += DownloadFileHelper_ProcessCompleted;
                DownloadFileHelper.DownLoadStart();
            }
            else
            {
                UpdateToolUpdated = true;
                UpdateToolCompleted?.Invoke();
            }
        }

        private  void DownloadFileHelper_ProcessCompleted()
        {
            var curDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            Common.FileHelper.Decompress(UpdatePatchFilePath, Path.Combine(curDir, "update"));
            if (File.Exists(UpdatePatchFilePath))
                File.Delete(UpdatePatchFilePath);
            UpdateToolUpdated = true;
            var version=JsonHelper.ReadVersionFromFile(curDir, StickyNotes.Properties.Resources.VersionFileName);
            version.UpdateAppVersion.MajorVersionNumber = LatestUpdateToolVersion.major_version_number;
            version.UpdateAppVersion.MinorVersionNumber = LatestUpdateToolVersion.minor_version_number;
            version.UpdateAppVersion.RevisionNumebr = LatestUpdateToolVersion.revision_number;
            JsonHelper.WriteVersionToFile(version, curDir, StickyNotes.Properties.Resources.VersionFileName);
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
            version = JsonHelper.ReadVersionFromFile(ComUtil.GetCurrentExecDirectory(),StickyNotes.Properties.Resources.VersionFileName);
            HttpHelper.BaseUrl = ServerUrl;
            var res=HttpHelper.HttpGet("api/Software/GetLastedVersion", new string[] { "softwarename" }, new object[] { "stickynotes" });
            if (res!=null&&res.success)
            {
                SoftwareUpdate software = HttpHelper.DynamicToObject<SoftwareUpdate>(res.data);
                var lastedSoftwareUpdate = software;
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
                var path = Path.Combine(ComUtil.GetCurrentExecDirectory(), "update\\UpdateApp.exe");
                string args = version.StickyNotesVersion.MajorVersionNumber.ToString() + " " + version.StickyNotesVersion.MinorVersionNumber.ToString() + " " + version.StickyNotesVersion.RevisionNumebr.ToString();
                Process.Start(path,args);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Logger.Log().Error(ex.Message);
            }
         
        }
    }
}
