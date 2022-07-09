using Common.Lang;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{

    public class LangHelper:INotifyPropertyChanged
    {
        // singleton
        public static LangHelper Instance { get; set; }
        static LangHelper()
        {
            Instance = new LangHelper();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void SetLang(Language language)
        {
            Instance.Language = language;
            if (language == Language.English)
            {
                Instance.Lang = new LangEN();
            }
            else
            {
                Instance.Lang = new LangCN();
            }
        }
        public Language Language { get; set; }
        private LangHelper() { }
        
        public LangBase Lang { get; set; }

    }
}
