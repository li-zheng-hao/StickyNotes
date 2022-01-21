using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace StickyNotes.ViewModel
{
    public class ListWindowViewModel:ViewModelBase
    {
        public RelayCommand<WindowsData> DeleteWindowCommand { get; set; }
        public RelayCommand<WindowsData> ChangeWindowVisibilityCommand { get; set; }

        public RelayCommand NewWindowCommand { get; set; }
        public ProgramData ProgramData { get; set; }
        public ListWindowViewModel()
        {
            this.ProgramData = ProgramData.Instance;
            DeleteWindowCommand = new RelayCommand<WindowsData>(DeleteWindowClick);
            ChangeWindowVisibilityCommand = new RelayCommand<WindowsData>(ChangeWindowVisibilityMethod);
            NewWindowCommand = new RelayCommand(NewWindowMethod);

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
                MainWindow.viewModel.RestoreData(MainWindow.Document, MainWindow.viewModel.Datas.WindowID);
                MainWindow.Show();
                WindowsManager.Instance.Windows.Add(MainWindow);
                ProgramData.Instance.Datas.Add(windowsData);
                ProgramData.Instance.HideWindowData.Remove(windowsData);
            }
           
        }

       
        private void DeleteWindowClick(WindowsData windowsData)
        {
            ProgramData.Instance.Datas.Remove(windowsData);
            //通知主窗体删除这个数据 关闭这个便签窗体
            Messenger.Default.Send<WindowsData>(windowsData, "DeleteWindow");
        }

    }
}
