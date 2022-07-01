using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GeneralUpdate.Core;
using GeneralUpdate.Core.Strategys;
using GeneralUpdate.Core.Update;
using Newtonsoft.Json;
using Spectre.Console;
using StickyNotes.Utils;

/// <summary>
/// 自动更新程序
/// </summary>

namespace AutoUpdateTool
{
    internal class Program
    {
        // 字符串版本转换成对应的整型版本 每个版本位置为4位
        // 例如 v3.3.3 转换为 300030003000再进行比较
        public static int LocalVersion;
        public static int GithubVersion;
        public static string StickyNotesExePath;

        
        private static string Tips1, Tips2, Tips3, Tips4, Tips5, Tips6;
        private static double ProgressVal, ProgressMin, ProgressMax;
        
        
        /// <summary>
        /// </summary>
        /// <param name="args">1、版本 2、StickyNotes.exe路径</param>
        private static void Main(string[] args)
        {
            AnsiConsole.Write(
                        new FigletText("StickyNotes")
                            .LeftAligned()
                            .Color(Color.Red));
            var rule = new Rule("[red]Begin Update[/]");
            AnsiConsole.Write(rule);
            if (args.Length<=0)
            {
                AnsiConsole.Markup("[bold red]Error-[/] Agruments number is 0 !");
                Thread.Sleep(5000);
                return;
            }
            string args1 = args[0];
            

            ProgressMin = 0;
            Task.Run(async () =>
            {
                var bootStrap = new GeneralUpdateBootstrap();
                bootStrap.MutiAllDownloadCompleted += OnMutiAllDownloadCompleted;
                bootStrap.MutiDownloadCompleted += OnMutiDownloadCompleted;
                bootStrap.MutiDownloadError += OnMutiDownloadError;
                bootStrap.MutiDownloadProgressChanged += OnMutiDownloadProgressChanged;
                bootStrap.MutiDownloadStatistics += OnMutiDownloadStatistics;
                bootStrap.Exception += OnException;
                bootStrap.Strategy<DefaultStrategy>().
                Option(UpdateOption.Encoding, Encoding.Default).
                Option(UpdateOption.DownloadTimeOut, 60).
                Option(UpdateOption.Format, "zip").
                RemoteAddressBase64(args1);
                await bootStrap.LaunchTaskAsync();
            });
            //if (args.Length == 2)
            //{
            //    // 当前版本
            //    LocalVersion = ConvertVersion(args[0]);
            //    StickyNotesExePath = args[1];
            //    Console.WriteLine("当前版本" + args[0]);
            //    Console.WriteLine("" + StickyNotesExePath);
            //}
            //else
            //{
            //    LocalVersion = ConvertVersion("v1.1.1");
            //    StickyNotesExePath = "D:\\StikyNotes\\StikyNotes\\bin\\Debug\\StikyNotes.exe";
            //    Console.WriteLine("参数不正确,程序退出");
            //    Thread.Sleep(2000);
            //    return;
            //}

            //var res = WebUtil.HttpGet(RepositoryInfo.RepositoryAddr);
            //var responseResult = res.Result;

            //var model = JsonConvert.DeserializeObject<GithubReleaseModel>(responseResult);
            //GithubVersion = ConvertVersion(model.tag_name);
            //Console.WriteLine("查询到Github最新版本：" + GithubVersion);
            //if (GithubVersion <= LocalVersion) return;
            //// 下载新版本进行替换
            //// 思路：
            //// 用StickyNotes启动本程序
            //// 1. 关闭StickyNotes.exe程序
            //// 2. 然后删除本地的StickyNotes.exe
            //// 3. 启动本地的StickyNotes.exe程序
            //// 4. 退出本脚本
            //Console.WriteLine("发现新版本，开始关闭进程");
            //KillProcess("STICKYNOTES");
            //Console.WriteLine("发现Github上有新版本，正在下载中，长时间未下载完成请科学上网");
            //string downloadUrl = "";
            //foreach (var item in model.assets)
            //{
            //    if (item.name.ToLower().Contains("StickyNotes".ToLower()))
            //    {
            //        downloadUrl = item.browser_download_url;
            //        break;
            //    }
            //}
            //var result = WebUtil.HttpDownLoadFile(downloadUrl, "StickyNotes.exe");
            //try
            //{
            //    if (result.Result)
            //        Console.WriteLine("下载完成......");

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("下载过程中出现异常，可能是网络连接不上");
            //    Console.WriteLine(ex.Message + ex.StackTrace);
            //    Console.WriteLine("按Enter回车退出更新（多次更新不上请将更新程序删掉后再打开StickyNotes）");
            //    Console.ReadLine();
            //}
            //RunProcess(StickyNotesExePath);
        }

        private static void OnException(object sender, ExceptionEventArgs e)
        {
            Tips6 = $"{e.Exception.Message}";
        }

        private static void OnMutiDownloadStatistics(object sender, MutiDownloadStatisticsEventArgs e)
        {
            Tips1 = $" {e.Speed} , {e.Remaining.ToShortTimeString()}";
        }

        private static void OnMutiDownloadProgressChanged(object sender, MutiDownloadProgressChangedEventArgs e)
        {
            switch (e.Type)
            {
                case ProgressType.Check:
                    break;

                case ProgressType.Donwload:
                    ProgressVal = e.BytesReceived;
                    if (ProgressMax != e.TotalBytesToReceive)
                    {
                        ProgressMax = e.TotalBytesToReceive;
                    }
                    Tips2 = $" {Math.Round(e.ProgressValue * 100, 2)}% ， Receivedbyte：{e.BytesReceived}M ，Totalbyte：{e.TotalBytesToReceive}M";
                    break;

                case ProgressType.Updatefile:
                    break;

                case ProgressType.Done:
                    break;

                case ProgressType.Fail:
                    break;

                default:
                    break;
            }
        }

        private static void OnMutiDownloadError(object sender, MutiDownloadErrorEventArgs e)
        {
            AnsiConsole.Markup("[underline red]Hello[/] World!");
            Tips6 = $"{e.Exception.Message}";
        }

        private static void OnMutiDownloadCompleted(object sender, MutiDownloadCompletedEventArgs e)
        {
            //Tips3 = $"{ e.Version.Name } download completed.";
        }

        private static void OnMutiAllDownloadCompleted(object sender, MutiAllDownloadCompletedEventArgs e)
        {
            if (e.IsAllDownloadCompleted)
            {
                Tips4 = "AllDownloadCompleted";
            }
            else
            {
                //foreach (var version in e.FailedVersions)
                //{
                //   Debug.Write($"{ version.Item1.Name }");
                //}
            }
        }
    }
}