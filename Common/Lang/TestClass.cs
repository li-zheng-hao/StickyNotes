using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Lang
{
    public class TestClass:INotifyPropertyChanged
        
    {
        public string Value { get; set; } = "origin";

        public event PropertyChangedEventHandler PropertyChanged;
    }
    
}
