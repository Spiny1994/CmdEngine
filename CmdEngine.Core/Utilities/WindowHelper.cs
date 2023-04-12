using CmdEngine.Core.Data;
using System;
using System.Runtime.InteropServices;

namespace CmdEngine.Core.Utilities;

public class WindowHelper
{
    private const int MF_BYCOMMAND = 0x00000000;
    public const int SC_CLOSE = 0xF060;
    public const int SC_MINIMIZE = 0xF020;
    public const int SC_MAXIMIZE = 0xF030;
    public const int SC_SIZE = 0xF000;
    public const int MONITOR_DEFAULTTOPRIMARY = 1;
    public const uint SW_RESTORE = 9;

    [DllImport("user32.dll")]
    public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

    [DllImport("user32.dll")]
    private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    [DllImport("kernel32.dll", ExactSpelling = true)]
    private static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    private static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

    [DllImport("user32.dll")]
    private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

    [StructLayout(LayoutKind.Sequential)]
    private struct MONITORINFO
    {
        public uint cbSize;
        public RECT rcMonitor;
        public RECT rcWork;
        public uint dwFlags;
        public static MONITORINFO Default
        {
            get { var inst = new MONITORINFO(); inst.cbSize = (uint)Marshal.SizeOf(inst); return inst; }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int Left, Top, Right, Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct POINT
    {
        public int x, y;
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

    [StructLayout(LayoutKind.Sequential)]
    private struct WINDOWPLACEMENT
    {
        public uint Length;
        public uint Flags;
        public uint ShowCmd;
        public POINT MinPosition;
        public POINT MaxPosition;
        public RECT NormalPosition;
        public static WINDOWPLACEMENT Default
        {
            get
            {
                var instance = new WINDOWPLACEMENT();
                instance.Length = (uint)Marshal.SizeOf(instance);
                return instance;
            }
        }
    }

    public static void DisableAllMenus()
    {
        IntPtr handle = GetConsoleWindow();
        IntPtr sysMenu = GetSystemMenu(handle, false);

        if (handle != IntPtr.Zero)
        {
            //DeleteMenu(sysMenu, SC_CLOSE, MF_BYCOMMAND);
            DeleteMenu(sysMenu, SC_MINIMIZE, MF_BYCOMMAND);
            DeleteMenu(sysMenu, SC_MAXIMIZE, MF_BYCOMMAND);
            DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
        }
    }

    public static void CenterWindow(Vector2 offset = new Vector2())
    {
        // Get this console window's hWnd (window handle).
        IntPtr hWnd = GetConsoleWindow();

        // Get information on the monitor.
        var mi = MONITORINFO.Default;
        GetMonitorInfo(MonitorFromWindow(hWnd, MONITOR_DEFAULTTOPRIMARY), ref mi);

        // Get information about this window's current placement.
        var wp = WINDOWPLACEMENT.Default;
        GetWindowPlacement(hWnd, ref wp);

        var monWidth = mi.rcMonitor.Right - mi.rcMonitor.Left;
        var monHeight = mi.rcMonitor.Bottom - mi.rcMonitor.Top;

        var conWidth = wp.NormalPosition.Right - wp.NormalPosition.Left;
        var conHeight = wp.NormalPosition.Bottom - wp.NormalPosition.Top;

        wp.NormalPosition = new RECT()
        {
            Left = (monWidth / 2) - (conWidth / 2) + offset.X + 14,
            Top = (monHeight / 2) - (conHeight / 2) + offset.Y,
            Right = (monWidth / 2) + (conWidth / 2) + offset.X + 14,
            Bottom = (monHeight / 2) + (conHeight / 2) + offset.Y
        };

        // Place the window at the new position.
        SetWindowPlacement(hWnd, ref wp);
    }
}
