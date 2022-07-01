using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StickyNotes.Utils.HotKeyUtil
{
    [Serializable]
    public class HotKey
    {

        // convert hotkey to string
        public override string ToString()
        {
            var str = new StringBuilder();

            if (Modifiers.HasFlag(ModifierKeys.Control))
                str.Append("Ctrl + ");
            if (Modifiers.HasFlag(ModifierKeys.Shift))
                str.Append("Shift + ");
            if (Modifiers.HasFlag(ModifierKeys.Alt))
                str.Append("Alt + ");
            if (Modifiers.HasFlag(ModifierKeys.Windows))
                str.Append("Win + ");

            str.Append(Key);

            return str.ToString();
        }
       
        public HotKey()
        {

        }
        public HotKeyType HotKeyType { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="HotKey"/> class.
        /// </summary>
        /// <param name="key">The key to be associated with the hotkey.</param>
        /// <param name="modifiers">The modifiers to be associated with the hotkey.</param>
        /// <param name="handler">The event handler to invoke when the hotkey is pressed.</param>
        /// <param name="id">An optional ID for the hotkey.</param>
        public HotKey(Key key, ModifierKeys modifiers, EventHandler<HotkeyEventArgs> handler,HotKeyType type,ushort id = default)
        {
            Key = key;
            Modifiers = modifiers;
            Pressed += handler;
            Id = id == default ? ++UniqueId : id;
            HotKeyType = type;
        }

        /// <summary>
        /// Occurs when the hotkey is pressed.
        /// </summary>
        public event EventHandler<HotkeyEventArgs> Pressed;

        /// <summary>
        /// Gets or sets the ID of this <see cref="HotKey"/>.
        /// </summary>
        public ushort Id { get; set; }

        /// <summary>
        /// Gets or sets the associated key of this <see cref="HotKey"/>.
        /// </summary>
        public Key Key { get; set; }

        /// <summary>
        /// Gets or sets the associated modifiers of this <see cref="HotKey"/>.
        /// </summary>
        public ModifierKeys Modifiers { get; set; }

        /// <summary>
        /// Gets or sets the current unique ID.
        /// </summary>
        private static ushort UniqueId { get; set; }

        /// <summary>
        /// An event reporting that this <see cref="HotKey"/> was pressed.
        /// </summary>
        /// <param name="e">Event data that contains information about this pressed <see cref="HotKey"/>.</param>
        public void OnPressed(HotkeyEventArgs e)
        {
            EventHandler<HotkeyEventArgs> handler = Pressed;
            handler?.Invoke(this, e);
        }
    }
}
