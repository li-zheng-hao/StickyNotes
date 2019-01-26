using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StikyNotes
{
    public class MainViewModel
    {
        #region 属性
        private MainWindow mainWindow;
        public MainWindow MainWindow { get => mainWindow; set => mainWindow = value; }
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

        public MainViewModel(MainWindow mainWindow)
        {
            MainWindow=mainWindow;
            Init();
        }

        /// <summary>
        /// 一些初始化步骤
        /// </summary>
        private void Init()
        {
            mainWindow.MouseDown += MainWindow_MouseDown;
            mainWindow.DeleteButton.Click += DeleteButton_Click;
            mainWindow.AddButton.Click += AddButton_Click;


            #region 增大字体命令
            IncreaseFontCommand=new RoutedCommand();
            KeyGesture IncreaseFontCommandGesture = new KeyGesture(
                Key.Up, ModifierKeys.Control | ModifierKeys.Alt);
            CommandBinding IncreaseBinding=new CommandBinding();
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

        #endregion



        #region 命令实现
        private void DecreaseBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            mainWindow.rickTextBox.FontSize -=2;
        }

        private void DecreaseBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !(mainWindow.rickTextBox.FontSize < 6);
        }

        private void IncreaseBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            mainWindow.rickTextBox.FontSize += 2;
        }

        private void IncreaseBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !(mainWindow.rickTextBox.FontSize > 48);
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
            newWin.Show();
            newWin.Activate();
        }
        /// <summary>
        /// 删除当前窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
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

        #endregion

    }
}
