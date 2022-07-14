using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using Common;
using ControlzEx.Theming;
using MahApps.Metro.Controls;

namespace StickyNotes.View
{
    /// <summary>
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AboutWindow : MetroWindow
    {

        public string Version { get; set; }
        public AboutWindow()
        {
            InitializeComponent();
            ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncWithAppMode;
            ThemeManager.Current.SyncTheme();
            this.DataContext = this;
            var ver= JsonHelper.ReadVersionFromFile(ComUtil.GetCurrentExecDirectory(),StickyNotes.Properties.Resources.VersionFileName);
            Version = $"{ver.StickyNotesVersion.MajorVersionNumber}.{ver.StickyNotesVersion.MinorVersionNumber}.{ver.StickyNotesVersion.RevisionNumebr}";
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = sender as Hyperlink;
            Process.Start(new ProcessStartInfo(link.NavigateUri.AbsoluteUri));
        }

    }
}
