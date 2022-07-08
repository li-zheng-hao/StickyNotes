using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace Common
{
    public class WindowHelper
    {
        public void TryToAttach(IntPtr ownerHandle,Window window)
        {
            if (ownerHandle == IntPtr.Zero)
            {
                return;
            }
            try
            {
                var helper = new WindowInteropHelper(window) { Owner = ownerHandle };
            }
            catch (Exception e)
            {
            }
        }
    }
}
