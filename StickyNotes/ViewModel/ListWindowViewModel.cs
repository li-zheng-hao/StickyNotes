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
        public ProgramData ProgramData { get; set; }
        public ListWindowViewModel()
        {
            this.ProgramData = ProgramData.Instance;
            DeleteWindowCommand = new RelayCommand<WindowsData>(DeleteWindowClick);

        }

        public void DeleteWindow()
        {
            //ProgramData.Instance.Datas.Remove(windowsdata);
            // 通知主窗体删除这个数据 关闭这个便签窗体
            //Messenger.Default.Send<dynamic>(windowsdata, "DeleteWindow");
        }
        private void DeleteWindowClick(WindowsData windowsData)
        {
            ProgramData.Instance.Datas.Remove(windowsData);
            //通知主窗体删除这个数据 关闭这个便签窗体
            Messenger.Default.Send<WindowsData>(windowsData, "DeleteWindow");
        }

    }
}
