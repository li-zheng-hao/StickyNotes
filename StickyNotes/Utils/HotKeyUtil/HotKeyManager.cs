using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Interop;

namespace StickyNotes.Utils.HotKeyUtil
{
    /// <summary>
    /// Represents a service to manage hotkeys for the current application.
    /// </summary>
    public class HotkeyManager
    {
        /// <summary>
        /// A dedicated object instance.
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="HotkeyManager"/> class and implements a Win32 window.
        /// </summary>
        protected HotkeyManager()
        {
            if (Handle == IntPtr.Zero)
            {
                // Creates an invisible Win32 window that we can use to handle WM_HOTKEY messages.
                HwndSourceParameters parameters = new HwndSourceParameters("HotkeyUtility")
                {
                    WindowStyle = 0,
                };
                HwndSource source = new HwndSource(parameters);
                source.AddHook(WndProc);
                Handle = source.Handle;
            }
        }

        /// <summary>
        /// Gets or sets the handle to the associated Win32 window of this <see cref="HotkeyManager"/>.
        /// </summary>
        private static IntPtr Handle { get; set; }

        /// <summary>
        /// Gets or sets the single, global instance of <see cref="HotkeyManager"/>.
        /// </summary>
        private static HotkeyManager Instance { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="HotKey"/> for this <see cref="HotkeyManager"/>.
        /// </summary>
        private Dictionary<ushort, HotKey> Keys { get; set; } = new Dictionary<ushort, HotKey>();

        /// <summary>
        /// Returns a single, global instance of <see cref="HotkeyManager"/>.
        /// </summary>
        /// <returns>A single, global instance of <see cref="HotkeyManager"/>.</returns>
        public static HotkeyManager GetHotkeyManager()
        {
            if (Instance == null)
            {
                lock (_lock)
                {
                    if (Instance == null)
                    {
                        Instance = new HotkeyManager();
                    }
                }
            }

            return Instance;
        }

        /// <summary>
        /// Attempts to add a <see cref="HotKey"/> to a dictionary and register it.
        /// </summary>
        /// <param name="hotkey">The <see cref="HotKey"/> to add and register.</param>
        /// <returns><see langword="true"/> if the specified <see cref="HotKey"/> was successfully added and registered; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="hotkey"/> is <see langword="null"/>.</exception>
        public bool TryAddHotkey(HotKey hotkey)
        {
            if (hotkey is null)
            {
                throw new ArgumentNullException(nameof(hotkey));
            }

            if (Keys.ContainsKey(hotkey.Id))
                return false;
            var res=RegisterHotkey(hotkey);
            if (res == true)
            {
                Keys.Add(hotkey.Id, hotkey);
            }
            else
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Attempts to remove a <see cref="HotKey"/> from a dictionary and unregister it.
        /// </summary>
        /// <param name="hotkey">The <see cref="HotKey"/> to remove and unregister.</param>
        /// <returns><see langword="true"/> if the specified <see cref="HotKey"/> was successfully removed and unregistered; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="hotkey"/> is <see langword="null"/>.</exception>
        public bool TryRemoveHotkey(HotKey hotkey)
        {
            if (hotkey is null)
            {
                throw new ArgumentNullException(nameof(hotkey));
            }
            if(!Keys.ContainsKey(hotkey.Id))
                return false;
            return Keys.Remove(hotkey.Id) && UnregisterHotkey(Handle, hotkey.Id);
        }

        /// <summary>
        /// Attempts to locate an existing <see cref="HotKey"/> through its current key and modifiers, unregister it, and reregister it with a new key and modifiers.
        /// </summary>
        /// <param name="oldKey">The key to match.</param>
        /// <param name="oldModifiers">The modifiers to match.</param>
        /// <param name="newKey">The key to replace the current key.</param>
        /// <param name="newModifiers">The modifiers to replace the current modifiers.</param>
        /// <returns><see langword="true"/> if the specified <see cref="Key"/> and specified <see cref="ModifierKeys"/> were successfully matched and replaced; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="newKey"/> and <paramref name="newModifiers"/> are respectively equal to <see cref="Key.None"/> and <see cref="ModifierKeys.None"/>.</exception>
        public bool TryReplaceHotkey(Key oldKey, ModifierKeys oldModifiers, Key newKey = Key.None, ModifierKeys newModifiers = ModifierKeys.None)
        {
            if (newKey == Key.None && newModifiers == ModifierKeys.None)
            {
                throw new ArgumentException($"{newKey} and {newModifiers} cannot both be None");
            }

            bool success = false;
            foreach (HotKey hotkey in GetHotkeys())
            {
                if (hotkey.Key == oldKey && hotkey.Modifiers == oldModifiers)
                {
                    if (success = UnregisterHotkey(Handle, hotkey.Id))
                    {
                        hotkey.Key = newKey;
                        hotkey.Modifiers = newModifiers;
                        success = RegisterHotkey(hotkey);
                    }

                    break;
                }
            }

            return success;
        }

        /// <summary>
        /// Attempts to locate an existing <see cref="HotKey"/> through its Id property, unregister it, and reregister it with a new key and modifiers.
        /// </summary>
        /// <param name="id">The Id of the targeted <see cref="HotKey"/>.</param>
        /// <param name="newKey">The key to replace the current key.</param>
        /// <param name="newModifiers">The modifiers to replace the current modifiers.</param>
        /// <returns><see langword="true"/> if the specified id was matched and the new key and modifiers were successfully registered; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="id"/> is equal to <see langword="default"/> or <paramref name="newKey"/> and <paramref name="newModifiers"/> are respectively equal to <see cref="Key.None"/> and <see cref="ModifierKeys.None"/>.</exception>
        public bool TryReplaceHotkey(ushort id, Key newKey = Key.None, ModifierKeys newModifiers = ModifierKeys.None)
        {
            if (id == default)
            {
                throw new ArgumentException($"Hotkeys cannot have an ID of {id}", nameof(id));
            }

            if (newKey == Key.None && newModifiers == ModifierKeys.None)
            {
                throw new ArgumentException($"{newKey} and {newModifiers} cannot both be None");
            }

            // Checks in order:
            // 1. That the id of the hotkey exists to begin with.
            // 2. That we aren't trying to needlessly register the same exact hotkey combination.
            // 3. And that we are able to successfully unregister the previous hotkey.
            //
            //
            //
            // I think it's important to expand on what I mean in point 2. Let's say a user has two
            // VisualHotkey controls in their XAML. One VisualHotkey's Combination property is using
            // a MultiBinding with a converter and binding to two strings: the first string is called
            // 'Key' and the second string is called 'Modifiers'. The converter ultimately converts
            // both strings to a KeyBinding by parsing Key.Space from 'Key' and parsing ModifierKeys.Alt
            // from 'Modifiers'. The user's second VisualHotkey's Combination property is simply hard-coded
            // as "Control + Space".
            //
            // Since one of the main features of HotkeyUtility is that users can change hotkey bindings
            // at runtime, there's a high chance the user will want to change one or both of those strings.
            // Let's now say the user changes 'Key' to the string equivalent of Key.D and 'Modifiers' to
            // the string equivalent of 'Control'. No exception is thrown but behind the scenes, the way the
            // Combination property updates due to the MultiBinding is that:
            //
            //      - The Combination is first binded to a KeyBinding of Key.D + ModifierKeys.Alt
            //          -- The previous binding (Key.Space + ModifierKeys.Alt) gets unregistered
            //          -- Key.D + ModifierKeys.Alt becomes the new hotkey binding
            //      - The Combination finally changes to a KeyBinding of Key.D + ModifierKeys.Control
            //          -- The previous binding (Key.D + ModifierKeys.Alt) gets unregistered
            //          -- Key.D + ModifierKeys.Control becomes the new hotkey binding
            //
            // See the problem? There's a completely unneccesary registering and unregistering when *both*
            // 'Key' and 'Modifiers' change. The user only wanted "Control + D", not "Alt + D". Even
            // though no exceptions are thrown here, this presents a much greater problem when the user
            // switches 'Key' and 'Modififers' back to Key.Space and ModifierKeys.Alt. Let's see how it
            // plays out:
            //
            //      - The Combination is first binded to a KeyBinding of Key.Space + ModifierKeys.Control
            //          -- The previous binding (Key.D + ModifierKeys.Control) gets unregistered
            //          -- Key.Space + ModifierKeys.Control becomes the new hotkey binding... except it
            //             doesn't because an exception gets thrown
            //
            // Remember what the user's second VisualHotkey's Combination was? It was "Control + Space"
            // which means that the hotkey is already registered. So, when we try to register Key.Space +
            // ModifierKeys.Control, it fails because the RegisterHotKey function produces an
            // ERROR_HOTKEY_ALREADY_REGISTERED error and an ApplicationException gets subsequently thrown.
            if (Keys.ContainsKey(id) && GetHotkeys().Where(it=>it.Key== newKey&&it.Modifiers==newModifiers) is null && UnregisterHotkey(Handle, id))
            {
                HotKey oldhotkey = Keys[id];
                oldhotkey.Key = newKey;
                oldhotkey.Modifiers = newModifiers;
                return RegisterHotkey(oldhotkey);
            }

            return false;
        }

        /// <summary>
        /// Returns a <see cref="Dictionary{TKey, TValue}.ValueCollection"/> of hotkeys.
        /// </summary>
        /// <returns>A <see cref="Dictionary{TKey, TValue}.ValueCollection"/> of <see cref="HotKey"/></returns>
        public Dictionary<ushort, HotKey>.ValueCollection GetHotkeys()
        {
            return Keys.Values;
        }

        /// <summary>
        /// Registers a <see cref="HotKey"/>.
        /// </summary>
        /// <param name="hotkey">The <see cref="HotKey"/> to register.</param>
        /// <returns><see langword="true"/> if the specified <see cref="HotKey"/> was successfully registered; otherwise, <see langword="false"/></returns>
        /// <exception cref="ApplicationException">The specified <see cref="HotKey"/> is already registered.</exception>
        private static bool RegisterHotkey(HotKey hotkey)
        {
            bool success = NativeMethods.RegisterHotKey(Handle, hotkey.Id, (uint)hotkey.Modifiers, (uint)KeyInterop.VirtualKeyFromKey(hotkey.Key));
            if (!success)
            {
                int error = Marshal.GetLastWin32Error();
                if (error == NativeMethods.ERROR_HOTKEY_ALREADY_REGISTERED)
                {
                    throw new ApplicationException($"The keystrokes specified for the hotkey (Key: {hotkey.Key} | Modifiers: {hotkey.Modifiers}) have already been registered by another hotkey");
                }
            }

            return success;
        }

        /// <summary>
        /// Unregisters a hotkey.
        /// </summary>
        /// <param name="hWnd">The handle to a window.</param>
        /// <param name="id">The Id associated with a <see cref="HotKey"/>.</param>
        /// <returns><see langword="true"/> if the hotkey was successfully unregistered; otherwise, <see langword="false"/>.</returns>
        private static bool UnregisterHotkey(IntPtr hWnd, ushort id)
        {
            return NativeMethods.UnregisterHotKey(hWnd, id);
        }

        /// <summary>
        /// A callback function that processes hotkey messages associated with the thread that registered the hotkeys. 
        /// </summary>
        /// <param name="hwnd">A handle to the window.</param>
        /// <param name="msg">The message.</param>
        /// <param name="wParam">Additional message information.</param>
        /// <param name="lParam">Additional message information.</param>
        /// <param name="handled">Indicates whether events resulting should be marked handled.</param>
        /// <returns>The return value is the result of the message processing, and depends on the message sent.</returns>
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == NativeMethods.WM_HOTKEY && Keys.TryGetValue((ushort)wParam.ToInt32(), out HotKey hotkey))
            {
                hotkey.OnPressed(new HotkeyEventArgs() { Type=hotkey.HotKeyType});
                handled = true;
            }

            return IntPtr.Zero;
        }
    }
}
