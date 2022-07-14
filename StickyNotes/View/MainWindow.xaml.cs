using ControlzEx.Theming;
using MahApps.Metro.Controls;
using StickyNotes.Utils;
using StickyNotes.Utils.HotKeyUtil;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace StickyNotes
{

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainViewModel viewModel;
        //构造函数
        public MainWindow()
        {

            InitializeComponent();
            ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncWithAppMode;
            ThemeManager.Current.SyncTheme();
            viewModel = new MainViewModel();
            this.DataContext = viewModel;
            viewModel.ThisWindow = this;
            WindowHide windowHide = new WindowHide(this);
            WindowHideManager.GetInstance().windowHideList.Add(windowHide);


       
        }
        // 切换删除线
        private void StrikeCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TextRange textRange = new TextRange((e.Source as RichTextBox).Selection.Start, (e.Source as RichTextBox).Selection.End);
            var currentTextDecoration = textRange.GetPropertyValue(Inline.TextDecorationsProperty);
            TextDecorationCollection newTextDecoration;

            if (currentTextDecoration != DependencyProperty.UnsetValue)
                newTextDecoration = ((TextDecorationCollection)currentTextDecoration == TextDecorations.Strikethrough) ? new TextDecorationCollection() : TextDecorations.Strikethrough;
            else
                newTextDecoration = TextDecorations.Strikethrough;
            textRange.ApplyPropertyValue(Inline.TextDecorationsProperty, newTextDecoration);

            e.Handled = true;
        }

        private void StrikeCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void RichTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Tab )
                return;

            var richTextBox = sender as RichTextBox;
            if (richTextBox == null) return;

            if (richTextBox.Selection.Text != string.Empty)
                richTextBox.Selection.Text = string.Empty;

            var caretPosition = richTextBox.CaretPosition.GetPositionAtOffset(0,
                                  LogicalDirection.Forward);

            richTextBox.CaretPosition.InsertTextInRun("  ");
            richTextBox.CaretPosition = caretPosition;
            e.Handled = true;
        }
    }
}
