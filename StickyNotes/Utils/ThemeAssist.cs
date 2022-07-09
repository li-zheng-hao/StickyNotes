using ControlzEx.Theming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StickyNotes
{

   
  
    /// <summary>
    /// 主题颜色管理类
    /// </summary>
    public static class ThemeAssist
    {
        public static readonly List<string> BaseTheme = new List<string>()
        {
            "Light",
            "Dark"
        };
        public static readonly List<string> Themes = new List<string>()
        {
            "冷灰","田园褐","石材灰","乌云黑","甘蓝"
        };
        /// <summary>
        /// 改变主题颜色
        /// </summary>
        /// <param name="themeName"></param>
        public static void ChangeTheme(string themeName)
        {

            ProgramData.Instance.CurrentTheme=themeName;

            //ThemeManager.Current.ChangeTheme(Application.Current, ProgramData.Instance.BaseTheme+"."+themeName);
            var mergedDictionaries = Application.Current.Resources.MergedDictionaries;

            foreach (var merged in mergedDictionaries)
            {
                var exist=Themes.Any(it => merged.Source.ToString().Contains(it));
                if (exist)
                {
                    mergedDictionaries.Remove(merged);
                    break;
                }
            }
            try
            {
                mergedDictionaries.Add(new ResourceDictionary { Source = new Uri($"pack://application:,,,/StickyNotes;component/Style/Themes/{themeName.ToString()}.xaml") });

            }
            catch (Exception)
            {

                mergedDictionaries.Add(new ResourceDictionary { Source = new Uri($"pack://application:,,,/StickyNotes;component/Style/Themes/{Themes.First()}.xaml") });

            }
        }

        public static void ChangeBaseTheme(string baseTheme)
        {
            ProgramData.Instance.BaseTheme=baseTheme;

            ThemeManager.Current.ChangeTheme(Application.Current, baseTheme+"."+ProgramData.Instance.CurrentTheme);

        }

    }

}

