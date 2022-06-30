using Microsoft.VisualStudio.TestTools.UnitTesting;
using StickyNotes.Utils.HotKeyUtil;
using System;
using System.Windows.Input;
namespace TestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Hotkey hotkey=new Hotkey(Key.A,ModifierKeys.Shift)
            //HotkeyManager.GetHotkeyManager().TryAddHotkey()
        }
    }
}
