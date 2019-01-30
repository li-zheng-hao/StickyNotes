using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace StikyNotes
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// 窗体数据
        /// </summary>
        public WindowsData Datas { get; set; }

        #region 命令
        public RelayCommand NewWindowCommand { get; private set; }
        public RelayCommand<MainWindow> DeletePaWindowCommand { get; private set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            Datas=new WindowsData();
            ProgramData.Instance.Datas.Add(Datas);
            NewWindowCommand=new RelayCommand(NewWindowMethod);
            DeletePaWindowCommand = new RelayCommand<MainWindow>(DeleteWindowMethod);
            
        }

        /// <summary>
        /// 新建窗体
        /// </summary>
        void NewWindowMethod()
        {
            MainWindow win=new MainWindow();
            win.Show();
            WindowsManager.Instance.Windows.Add(win);

        }

        /// <summary>
        /// 删除窗体
        /// </summary>
        void DeleteWindowMethod(MainWindow obj)
        {
            var win=obj as MainWindow;
            if(WindowsManager.Instance.Windows.Contains(win))
            {
                WindowsManager.Instance.Windows.Remove(win);
                ProgramData.Instance.Datas.Remove(Datas);
            }
            win.Close();
        }

    }
}