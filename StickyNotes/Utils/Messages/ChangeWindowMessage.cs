using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StickyNotes.Utils.Messages
{
    class ChangeWindowMessage
    {
        
        public WindowsData window;
        public ChangeWindowMessageType msgType;
    }
    /// <summary>
    /// 更改便签类型
    /// </summary>
    public enum ChangeWindowMessageType
    {
        AddWindow,
        DeleteWindow
    }
}
