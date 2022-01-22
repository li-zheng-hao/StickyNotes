using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using StickyNotes.Utils;

namespace StickyNotes.ViewModel
{
    public class ListWindowViewModel:ViewModelBase
    {
        public RelayCommand<WindowsData> DeleteWindowCommand { get; set; }
        public RelayCommand<WindowsData> ChangeWindowVisibilityCommand { get; set; }
        public RelayCommand<TextChangedEventArgs> FilterTextChangedCommand { get; set; }
        /// <summary>
        /// 搜索框内字符串
        /// </summary>
        public string FilterText { get; set; }
        /// <summary>
        /// 用来显示的列表，过滤和新增时添加
        /// </summary>
        public ObservableCollection<WindowsData> DisplayWindows{ get; set; }=new ObservableCollection<WindowsData>();

        public RelayCommand NewWindowCommand { get; set; }
        public ProgramData ProgramData { get; set; }
        public ListWindowViewModel()
        {
            this.ProgramData = ProgramData.Instance;
            DeleteWindowCommand = new RelayCommand<WindowsData>(DeleteWindowClick);
            ChangeWindowVisibilityCommand = new RelayCommand<WindowsData>(ChangeWindowVisibilityMethod);
            NewWindowCommand = new RelayCommand(NewWindowMethod);
            FilterTextChangedCommand = new RelayCommand<TextChangedEventArgs>(FilterTextChangedMethod);
            ProgramData.Datas.ToList().ForEach(data => DisplayWindows.Add(data));
            ProgramData.HideWindowData.ToList().ForEach(data=>DisplayWindows.Add(data));

            ProgramData.Datas.CollectionChanged += Datas_CollectionChanged;
            ProgramData.HideWindowData.CollectionChanged += Datas_CollectionChanged;
        }

        private void Datas_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(string.IsNullOrEmpty(FilterText))
            {
                this.DisplayWindows.Clear();
                ProgramData.Datas.ToList().ForEach(data => DisplayWindows.Add(data));
                ProgramData.HideWindowData.ToList().ForEach(data => DisplayWindows.Add(data));

            }
        }

        private void FilterTextChangedMethod(TextChangedEventArgs e) 
        {
            var text=(e.Source as System.Windows.Controls.TextBox).Text;
            this.DisplayWindows.Clear();
            this.ProgramData.Datas?.ToList()?.Where(it=>it.RichTextBoxContent.Contains(text))?.ToList()?.ForEach(data => DisplayWindows.Add(data));
            this.ProgramData.HideWindowData?.ToList()?.Where(it=>it.RichTextBoxContent.Contains(text))?.ToList()?.ForEach(data => DisplayWindows.Add(data));

        }

        private void NewWindowMethod()
        {
            MainWindow win = new MainWindow();
            win.viewModel.Datas = new WindowsData();
            win.Show();
            ProgramData.Instance.Datas.Add(win.viewModel.Datas);
            WindowsManager.Instance.Windows.Add(win);
        }

        private void ChangeWindowVisibilityMethod(WindowsData windowsData)
        {
            if(ProgramData.Instance.Datas.Contains(windowsData))
            {
                //通知主窗体关闭
                Messenger.Default.Send<WindowsData>(windowsData, "CloseWindow");
            }
            else
            {
                var MainWindow = new MainWindow();
                MainWindow.viewModel.Datas = windowsData;
                windowsData.IsShowed = true;
                MainWindow.viewModel.RestoreData(MainWindow.Document, MainWindow.viewModel.Datas.WindowID);
                MainWindow.Show();
                WindowsManager.Instance.Windows.Add(MainWindow);
                ProgramData.Instance.Datas.Add(windowsData);
                ProgramData.Instance.HideWindowData.Remove(windowsData);
            }
           
        }

       
        private void DeleteWindowClick(WindowsData windowsData)
        {
            try
            {
                if (ProgramData.Instance.HideWindowData.Contains(windowsData))
                {
                    ChangeWindowVisibilityMethod(windowsData);
                }
                ProgramData.Instance.Datas.Remove(windowsData);
                //通知主窗体删除这个数据 关闭这个便签窗体
                Messenger.Default.Send<WindowsData>(windowsData, "DeleteWindow");
            }
            catch (Exception ex)
            {
                Logger.Log().Error("删除窗体时出错"+ex.Message);
            }
           
        }

    }
}
