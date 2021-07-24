using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace StickyNotes.ViewModel
{
    public class ListWindowViewModel:ViewModelBase
    {
        public ProgramData ProgramData { get; set; }
        public ListWindowViewModel()
        {
            this.ProgramData = ProgramData.Instance;
        }
    }
}
