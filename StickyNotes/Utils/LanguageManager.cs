using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StickyNotes.Utils
{
    public enum Language{
        Chinese,
        English
    }
    public class LanguageManager
    {
        private const string _resourceStrFomart = "pack://application:,,,/Style/Languages/Lan-{0}.xaml";
        /// <summary>
        /// 获取当前语言下的对应标签
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Translate(string key)
        {
            return Application.Current.FindResource(key).ToString();
        }

        public static void ChangeLanguage(Language lan)
        {
            switch (lan)
            {
                case Language.Chinese:
                    RemoveCurLan();
                    AddResource(string.Format(_resourceStrFomart, "cn"));
                    break;
                case Language.English:
                default:
                    RemoveCurLan();
                    AddResource(string.Format(_resourceStrFomart, "en"));
                    break;

            }
        }

        private static void AddResource(string uri)
        {
            var dictionaries=Application.Current.Resources.MergedDictionaries;
            foreach (var item in dictionaries)
            {
                if (item?.Source?.OriginalString == uri)
                    return;
            }
            ResourceDictionary resourceDictionary = new ResourceDictionary()
            {
                Source = new Uri(uri,UriKind.Absolute)
            };
            dictionaries.Add(resourceDictionary);

        }
        private static void RemoveCurLan()
        {
            var dictionaries = Application.Current.Resources.MergedDictionaries;
            foreach (var item in dictionaries)
            {
                if (item == null || item.Source == null || item.Source.OriginalString == null)
                    continue;
                if ((bool)(item?.Source?.OriginalString?.Contains("Style/Languages")))
                {
                    dictionaries.Remove(item);
                    break;
                }
            }
        }
    }
}
