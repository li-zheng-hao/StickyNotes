using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace StickyNotes.Converter
{
    public class DeleteWindowMessageConverter: IMultiValueConverter
    {
      
            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                return values.Clone();
            }

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                return null;
            }
    }
}
