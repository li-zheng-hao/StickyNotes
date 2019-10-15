using StikyNotes.Utils.HotKeyUtil;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace StikyNotes
{
    public class ShowAllHotKeyConverter : IValueConverter
    {
        /// <summary>
        /// 从DataContent中的数据转换到View中的数据
        /// </summary>
        /// <param name="value">DataContent中的数据</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = (HotKeyModel)value;
            return result.ToString();
        }

       

        /// <summary>
        /// 只有在TwoWay的时候才能用上
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //            HotKeyModel res = new HotKeyModel();
            //            res.Name = EHotKeySetting.ShowAllWindow.ToString();
            //            string str = (string)value;
            //            string[] keys = str.Split('+');
            //            if (keys.Contains("Ctrl"))
            //            {
            //                res.IsSelectCtrl = true;
            //            }
            //
            //            if (keys.Contains("Shift"))
            //            {
            //                res.IsSelectShift = true;
            //            }
            //
            //            if (keys.Contains("Alt"))
            //            {
            //                res.IsSelectAlt = true;
            //            }
            //
            //            var useKey = EKey.Q;
            //            foreach (int v in Enum.GetValues(typeof(EKey)))
            //            {
            //                string keyName = Enum.GetName(typeof(EKey), v);
            //                if (keys.Last() == keyName)
            //                {
            //                    useKey = (EKey)v;
            //                }
            //            }
            //
            //            res.SelectKey = useKey;
            //            res.IsUsable = true;
            //            return res;
            return null;

        }

    }
}
