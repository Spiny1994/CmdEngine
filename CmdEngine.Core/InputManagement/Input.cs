using System.Runtime.InteropServices;
using System.Text;

namespace CmdEngine.Core.InputManagement;

public static class Input
{
    private static List<Key> _pressedKeys = new List<Key>();
    private static List<Key> _pressedKeysThisFrame = new List<Key>();
    private static List<Key> _liftedKeysThisFrame = new List<Key>();

    /// <summary>
    /// Is a specific key pressed this frame?
    /// </summary>
    public static bool GetKeyDown(Key key)
    {
        if (InputFetcher.IsKeyDown(key) && !_pressedKeys.Contains(key))
        {
            _pressedKeys.Add(key);
            _pressedKeysThisFrame.Add(key);
            return true;
        }

        if (_pressedKeysThisFrame.Contains(key))
            return true;

        return false;
    }

    /// <summary>
    /// Is a specific key not pressed anymore this frame?
    /// </summary>
    public static bool GetKeyUp(Key key)
    {
        if (!InputFetcher.IsKeyDown(key) && _pressedKeys.Contains(key))
        {
            _pressedKeys.Remove(key);
            _liftedKeysThisFrame.Add(key);
            return true;
        }

        if (_liftedKeysThisFrame.Contains(key))
            return true;

        return false;
    }

    /// <summary>
    /// Is a specific key held down?
    /// </summary>
    public static bool GetKey(Key key)
    {
        if (_pressedKeys.Contains(key))
            return true;

        return GetKeyDown(key);
    }

    /// <summary>
    /// Marks the end of the frame for the input to clear keys pressed and lifted.
    /// </summary>
    public static void MarkEndOfFrame()
    {
        var keysStillPressed = new List<Key>();

        // Clears the pressed keys if noone listens for it.
        foreach (var key in _pressedKeys)
            if (InputFetcher.IsKeyDown(key))
                keysStillPressed.Add(key);

        _pressedKeys.Clear();
        _pressedKeys.AddRange(keysStillPressed);

        _pressedKeysThisFrame.Clear();
        _liftedKeysThisFrame.Clear();
    }

    internal static class InputFetcher
    {
        [DllImport("user32.dll")]
        static extern short GetKeyState(Key nVirtKey);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private const int KEY_PRESSED = 0x8000;

        public static bool IsKeyDown(Key key)
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0 && Buff.ToString() == Console.Title && (GetKeyState(key) & KEY_PRESSED) != 0)
                return true;
            else
                return false;
        }
    }
}
