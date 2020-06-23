using MahApps.Metro.Controls;
using StikyNotes.Utils;
using System;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Documents;

namespace StikyNotes
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AboutWindow : MetroWindow
    {
        public static bool isFirstTimeOpen = true;
        public string version { get; set; } = "v3.2.1";
        public AboutWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = sender as Hyperlink;
            Process.Start(new ProcessStartInfo(link.NavigateUri.AbsoluteUri));
        }

        private void Loaded(object sender, RoutedEventArgs e)
        {
            if (isFirstTimeOpen)
            {
                Thread thread = new Thread(new ThreadStart(this.CheckUpdate));
                thread.Start();
                isFirstTimeOpen = !isFirstTimeOpen;
            }
        }

        private void CheckUpdate()
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //加上这一句
                System.Net.WebClient client = new WebClient();
                byte[] page = client.DownloadData("https://github.com/li-zheng-hao/StikyNotes/releases/");
                string content = System.Text.Encoding.UTF8.GetString(page);
                string regex = @"v[0-9]\.[0-9]\.[0-9]";
                Regex re = new Regex(regex);
                MatchCollection matches = re.Matches(content);
                System.Collections.IEnumerator enu = matches.GetEnumerator();
                bool needUpdate = false;
                while (enu.MoveNext() && enu.Current != null)
                {
                    Match match = (Match)(enu.Current);

                    //Console.Write(match.Value + "\r\n");
                    string result = match.Value;
                    Console.WriteLine(match.Value);
                    if (Convert.ToInt32(match.Value[1]) > Convert.ToInt32(version[1]))
                    {
                        needUpdate = true;
                    }
                    else if (Convert.ToInt32(match.Value[1]) == Convert.ToInt32(version[1]) && Convert.ToInt32(match.Value[3]) > Convert.ToInt32(version[3]))
                    {
                        needUpdate = true;
                    }
                    else if (Convert.ToInt32(match.Value[1]) == Convert.ToInt32(version[1]) && Convert.ToInt32(match.Value[3]) == Convert.ToInt32(version[3]) && Convert.ToInt32(match.Value[5]) > Convert.ToInt32(version[5]))
                    {
                        needUpdate = true;

                    }

                    if (needUpdate)
                    {
                        new Thread(() =>
                        {
                            this.Invoke(new Action(() =>
                            {
                                MessageBox.Show("有新版本，建议去github更新");
                            }));
                        }).Start();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log().Debug("无法连接项目github官网");
            }

        }
    }
}
