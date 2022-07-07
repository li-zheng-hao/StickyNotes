using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Common.Lang
{
    public class LangExtension:MarkupExtension
    {
        private object _value;

        public object Key
        {
            get => _value;
            set => _value = value;
        }



        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var val = LangHelper.Instance.Lang.GetType().GetProperty((string)Key).GetValue(LangHelper.Instance.Lang, null);
            return val;
        }
    }
}
