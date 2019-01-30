using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MaterialDesignThemes.Wpf;

namespace StikyNotes
{
    [Serializable]
    public class MainViewModel
    {
        #region 属性
        private MainWindow mainWindow;
        public MainWindow MainWindow { get => mainWindow; set => mainWindow = value; }

        public WindowsData Datas { get; set; }




        #endregion


        #region 命令
        /// <summary>
        /// 增大字体
        /// </summary>
        private RoutedCommand IncreaseFontCommand { get; set; }
        /// <summary>
        /// 减小字体
        /// </summary>
        private RoutedCommand DecreaseFontCommand { get; set; }

        #endregion


        #region 初始化

        public MainViewModel(MainWindow mainWindow, WindowsData data)
        {
            MainWindow = mainWindow;
            MainWindow.Deactivated += MainWindow_Deactivated;
            Datas = data;
            WindowsManager.Instance.Windows.Add(mainWindow);
            Init();
        }

        private void MainWindow_Deactivated(object sender, EventArgs e)
        {
            XMLHelper.SaveObjAsXml(ProgramData.Instance, "StikyNotesData.xml");
        }

        /// <summary>
        /// 一些初始化步骤
        /// </summary>
        private void Init()
        {
            #region 窗体按钮绑定以及数据初始化

            mainWindow.Left = Datas.StartUpPosition.X;
            mainWindow.Top = Datas.StartUpPosition.Y;
            mainWindow.Topmost = ProgramData.Instance.IsWindowTopMost;
            ProgramData.Instance.Datas.Add(this.Datas);
            mainWindow.MouseDown += MainWindow_MouseDown;
            mainWindow.DeleteButton.Click += DeleteButton_Click;
            mainWindow.AddButton.Click += AddButton_Click;
            mainWindow.Closed += MainWindow_Closed;
            mainWindow.SettingButton.Click += SettingButton_Click;
            mainWindow.AboutButton.Click += AboutButton_Click;

            #endregion

            #region 增大字体命令
            IncreaseFontCommand = new RoutedCommand();
            KeyGesture IncreaseFontCommandGesture = new KeyGesture(
                Key.Up, ModifierKeys.Control | ModifierKeys.Alt);
            CommandBinding IncreaseBinding = new CommandBinding();
            IncreaseBinding.Command = IncreaseFontCommand;
            IncreaseBinding.CanExecute += IncreaseBinding_CanExecute;
            IncreaseBinding.Executed += IncreaseBinding_Executed;
            IncreaseFontCommand.InputGestures.Add(IncreaseFontCommandGesture);
            mainWindow.CommandBindings.Add(IncreaseBinding);

            #endregion

            #region 减小字体
            DecreaseFontCommand = new RoutedCommand();
            KeyGesture DecreaseFontCommandGesture = new KeyGesture(
                Key.Down, ModifierKeys.Control | ModifierKeys.Alt);
            CommandBinding DecreaseBinding = new CommandBinding();
            DecreaseBinding.Command = DecreaseFontCommand;
            DecreaseBinding.CanExecute += DecreaseBinding_CanExecute; ;
            DecreaseBinding.Executed += DecreaseBinding_Executed;
            DecreaseFontCommand.InputGestures.Add(DecreaseFontCommandGesture);

            mainWindow.CommandBindings.Add(DecreaseBinding);
            #endregion

        }

        private void Datas_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            XMLHelper.SaveObjAsXml(ProgramData.Instance, "StikyNotesData.xml");
        }


        #endregion


        #region 命令实现
        private void DecreaseBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (mainWindow.MainTextBox.FontSize > 8)
                mainWindow.MainTextBox.FontSize -= 2;
        }

        private void DecreaseBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void IncreaseBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (mainWindow.MainTextBox.FontSize < 24)
                mainWindow.MainTextBox.FontSize += 2;
        }

        private void IncreaseBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        #endregion


        #region 窗体事件
        /// <summary>
        /// 添加新窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var newWin = new MainWindow();
            newWin.DataContext = new MainViewModel(newWin, new WindowsData());
            WindowsManager.Instance.Windows.Add(newWin);
            newWin.Show();
            newWin.Activate();
        }
        /// <summary>
        /// 删除当前窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private  void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProgramData.Instance.Datas.Contains(this.Datas))
                ProgramData.Instance.Datas.Remove(this.Datas);
            mainWindow.Close();
        }
        /// <summary>
        /// 拖动窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.Source.Equals(mainWindow.Menu) || e.Source.Equals(mainWindow.SoftWareName))
            {
                mainWindow.DragMove();
            }
        }

        /// <summary>
        /// 窗体关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            WindowsManager.Instance.Windows.Remove(mainWindow);
            Datas.StartUpPosition = new Point(mainWindow.Left, mainWindow.Top);
            if(WindowsManager.Instance.Windows.Count==0)
                Application.Current.Shutdown();
        }

        /// <summary>
        /// 关于按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow win = new AboutWindow();
            win.Show();
        }

        /// <summary>
        /// 设置按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow win = new SettingWindow();
            win.Show();
        }


        #endregion

    }
}
