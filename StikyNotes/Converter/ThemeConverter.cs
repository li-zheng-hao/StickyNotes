using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace StikyNotes
{
    public class ThemeConverter:IValueConverter
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
            int result=(int)value;
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
            var index = (Themes)value;
            return index;

        }
    }
}
