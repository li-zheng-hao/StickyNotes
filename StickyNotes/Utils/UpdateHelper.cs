using GeneralUpdate.ClientCore;
using GeneralUpdate.ClientCore.Strategys;
using GeneralUpdate.Core.Update;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StickyNotes.Utils
{
    /// <summary>
    /// 用于检查更新工具的版本是否需要更新
    /// </summary>
    public class UpdateHelper
    {
        public static string ServerUrl = StickyNotes.Properties.Resources.ServerUrl;
        public static void Update()
        {
            Task.Run(async () =>
            {
                var generalClientBootstrap = new GeneralClientBootstrap();
                generalClientBootstrap.MutiDownloadProgressChanged += OnMutiDownloadProgressChanged;
                generalClientBootstrap.MutiDownloadStatistics += OnMutiDownloadStatistics;
                generalClientBootstrap.MutiDownloadCompleted += OnMutiDownloadCompleted;
                generalClientBootstrap.MutiAllDownloadCompleted += OnMutiAllDownloadCompleted;
                generalClientBootstrap.MutiDownloadError += OnMutiDownloadError;
                generalClientBootstrap.Exception += OnException;
                generalClientBootstrap.Config(ServerUrl).
                Option(UpdateOption.DownloadTimeOut, 60).
                Option(UpdateOption.Encoding, Encoding.Default).
                Option(UpdateOption.Format, "zip").
                Strategy<ClientStrategy>();
                await generalClientBootstrap.LaunchTaskAsync();
            });
        }

        private static void OnException(object sender, ExceptionEventArgs e)
        {
            Debug.WriteLine(e.Exception.Message);
        }

        private static void OnMutiDownloadError(object sender, MutiDownloadErrorEventArgs e)
        {
            Debug.WriteLine($"{ e.Version.ToString()} error!");
        }

        private static void OnMutiAllDownloadCompleted(object sender, MutiAllDownloadCompletedEventArgs e)
        {
            //e.FailedVersions; 如果出现下载失败则会把下载错误的版本、错误原因统计到该集合当中。
            Debug.WriteLine($"Is all download completed {e.IsAllDownloadCompleted}.");
        }

        private static void OnMutiDownloadCompleted(object sender, MutiDownloadCompletedEventArgs e)
        {
            Debug.WriteLine($"{ e.Version.ToString() } download completed.");
        }

        private static void OnMutiDownloadStatistics(object sender, MutiDownloadStatisticsEventArgs e)
        {
            //e.Remaining 剩余下载时间
            //e.Speed 下载速度
            //e.Version 当前下载的版本信息
        }

        private static void OnMutiDownloadProgressChanged(object sender, MutiDownloadProgressChangedEventArgs e)
        {
            //e.TotalBytesToReceive 当前更新包需要下载的总大小
            //e.ProgressValue 当前进度值
            //e.ProgressPercentage 当前进度的百分比
            //e.Version 当前下载的版本信息
            //e.Type 当前正在执行的操作 1.ProgressType.Check 检查版本信息中 2.ProgressType.Donwload 正在下载当前版本 3. ProgressType.Updatefile 更新当前版本 4. ProgressType.Done更新完成 5.ProgressType.Fail 更新失败
            //e.BytesReceived 已下载大小
        }
    }
}
