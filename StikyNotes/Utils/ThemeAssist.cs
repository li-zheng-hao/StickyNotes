using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StikyNotes
{

    public enum Themes
    {
        Blue,
        Gray,
        Orange
    }
    /// <summary>
    /// 主题颜色管理类
    /// </summary>
    public static class ThemeAssist
    {


        /// <summary>
        /// 改变主题颜色
        /// </summary>
        /// <param name="themeName"></param>
        public static void ChangeTheme(Themes themeName)
        {
            var mergedDictionaries = Application.Current.Resources.MergedDictionaries;

            foreach (var merged in mergedDictionaries)
            {
                if (merged.Source.ToString().Contains(nameof(Themes.Orange)) 
                    || merged.Source.ToString().Contains(nameof(Themes.Gray))
                    || merged.Source.ToString().Contains(nameof(Themes.Blue)))
                {
                    mergedDictionaries.Remove(merged);
                    break;
                }
            }
            mergedDictionaries.Add(new ResourceDictionary { Source = new Uri($"pack://application:,,,/StikyNotes;component/Style/Themes/{themeName.ToString()}.xaml") });
        }

    }

}

