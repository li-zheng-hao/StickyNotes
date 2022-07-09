using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class DownloadFileHelper
    {
        const int bytebuff = 1024;
        const int ReadWriteTimeOut = 2 * 1000;//超时等待时间
        const int TimeOutWait = 5 * 1000;//超时等待时间
        const int MaxTryTime = 12;

        private double totalSize, curReadSize, speed;
        private int proc, remainTime;
        private int totalTime = 0;

        private bool downLoadWorking = false;

        private string StrFileName = "";
        private string StrUrl = "";

        private string outMsg = "";

        private string fileName = string.Empty;
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="savePath">完整的路径包含文件名</param>
        public DownloadFileHelper(string url, string savePath)
        {
            this.StrUrl = url;
            this.StrFileName = savePath;
            this.fileName = Path.GetFileName(savePath);
        }
        /// <summary>
        /// 下载进度更新
        /// </summary>
        /// <param name="totalNum">下载文件总大小</param>
        /// <param name="num">已下载文件大小</param>
        /// <param name="progress">下载进度百分比</param>
        /// <param name="speed">下载速度</param>
        /// <param name="remainTime">剩余下载时间</param>
        public delegate void delDownFileHandler(string totalNum, string num, int progress, string speed, string remainTime, string outMsg, string fileName);
        public event delDownFileHandler ProgressChanged;

        // 文件下载完成
        public event Action ProcessCompleted;

        public System.Windows.Forms.Timer timer;

        public void Init()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 50;
            timer.Tick -= TickEventHandler;
            timer.Tick += TickEventHandler;
            timer.Enabled = true;
            downLoadWorking = true;
        }
        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private string GetSize(double size)
        {
            String[] units = new String[] { "B", "KB", "MB", "GB", "TB", "PB" };
            double mod = 1024.0;
            int i = 0;
            while (size >= mod)
            {
                size /= mod;
                i++;
            }
            return Math.Round(size) + units[i];


        }
        /// <summary>
        /// 获取时间
        /// </summary>
        /// <param name="second"></param>
        /// <returns></returns>
        private string GetTime(int second)
        {
            return new DateTime(1970, 01, 01, 00, 00, 00).AddSeconds(second).ToString("HH:mm:ss");
        }

        /// <summary>
        /// 开始下载文件（同步）  支持断点续传
        /// </summary>
        private void DownloadFile()
        {
            totalSize = GetFileContentLength(StrUrl);
            System.IO.FileStream fs=null;
            try
            {
                //打开上次下载的文件或新建文件
                long lStartPos = 0;
                
                if (System.IO.File.Exists(StrFileName))
                {
                    fs = System.IO.File.OpenWrite(StrFileName);
                    lStartPos = fs.Length;
                    fs.Seek(lStartPos, System.IO.SeekOrigin.Current);   //移动文件流中的当前指针
                }
                else
                {
                    fs = new System.IO.FileStream(StrFileName, System.IO.FileMode.Create);
                    lStartPos = 0;
                }

                curReadSize = lStartPos;

                if (curReadSize == totalSize)
                {
                    outMsg = "文件已下载！";
                    timer.Enabled = false;
                    timer.Stop();
                    fs?.Close();
                    ProcessCompleted?.Invoke();
                    
                    return;
                }

                //打开网络连接
                try
                {
                    System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(StrUrl);
                    if (lStartPos > 0)
                        request.AddRange((int)lStartPos);    //设置Range值

                    //向服务器请求，获得服务器回应数据流
                    System.IO.Stream ns = request.GetResponse().GetResponseStream();
                    byte[] nbytes = new byte[bytebuff];
                    int nReadSize = 0;
                    proc = 0;

                    do
                    {

                        nReadSize = ns.Read(nbytes, 0, bytebuff);
                        fs.Write(nbytes, 0, nReadSize);

                        //已下载大小
                        curReadSize += nReadSize;
                        //进度百分比
                        proc = (int)((curReadSize / totalSize) * 100);
                        Console.WriteLine(proc + "进度条");
                        //下载速度
                        speed = (curReadSize / totalTime) * 10;
                        //剩余时间
                        remainTime = (int)((totalSize / speed) - (totalTime / 10));

                        if (downLoadWorking == false)
                            break;

                    } while (nReadSize > 0);

                    fs.Close();
                    ns.Close();

                    if (curReadSize == totalSize)
                    {
                        outMsg = "下载完成！";
                        ProcessCompleted?.Invoke();
                        downLoadWorking = false;
                    }
                }
                catch (Exception ex)
                {
                    outMsg = string.Format("下载失败：{0}", ex.ToString());
                }
         
            }finally
            {
                fs?.Close();
                timer.Enabled = false;
                timer.Stop();

            }

        }
        /// <summary>
        /// 暂停下载
        /// </summary>
        public void DownLoadPause()
        {
            outMsg = "下载已暂停";
            downLoadWorking = false;
            timer.Stop();
            
        }
        /// <summary>
        /// 继续下载
        /// </summary>
        public void DownLoadContinue()
        {
            outMsg = "正在下载";
            downLoadWorking = true;
            DownLoadStart();
        }

        /// <summary>
        /// 开始下载
        /// </summary>
        public void DownLoadStart()
        {
            Init();
            Task.Run(() =>
            {
                DownloadFile();
            });
        }

        /// <summary>
        /// 定时器方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TickEventHandler(object sender, EventArgs e)
        {
            ProgressChanged?.Invoke(GetSize(totalSize),
                                GetSize(curReadSize),
                                proc,
                                string.Format("{0}/s", GetSize(speed)),
                                GetTime(remainTime),
                                outMsg,
                                fileName
                                );
            if (downLoadWorking == true)
            {
                totalTime++;
            }
        }

        /// <summary>
        /// 获取下载文件长度
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public long GetFileContentLength(string url)
        {
            HttpWebRequest request = null;
            try
            {
                request = (HttpWebRequest)HttpWebRequest.Create(url);
                //request.Timeout = TimeOutWait;
                //request.ReadWriteTimeout = ReadWriteTimeOut;
                //向服务器请求，获得服务器回应数据流
                WebResponse respone = request.GetResponse();
                request.Abort();
                return respone.ContentLength;
            }
            catch (WebException e)
            {
                if (request != null)
                    request.Abort();
                return 0;
            }
        }
    }

}
