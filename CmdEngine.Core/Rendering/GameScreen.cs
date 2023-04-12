using CmdEngine.Core.Data;
using CmdEngine.Core.Utilities;

namespace CmdEngine.Core.Rendering;

public class GameScreen
{
    private static readonly int _maxWidth = 220;
    private static readonly int _maxHeight = 62;

    public static Vector2 Size { get; private set; }

    static GameScreen()
    {
        Recalculate();
    }

    public static void SetResolution(int x, int y, Vector2 offset = new Vector2(), ConsoleColor screenColor = ConsoleColor.Black)
    {
        var width = x * 2;
        var height = y;
        if (width > _maxWidth)
            width = _maxWidth;
        if (height > _maxHeight)
            height = _maxHeight;

        Console.SetWindowSize(width, height);

        // Centers the window and apply a small offset.
        WindowHelper.CenterWindow(offset);

        Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
        Console.SetWindowPosition(0, 0);
        Console.CursorVisible = false;

        Recalculate();

        ConsoleBuffer.Initialize(Console.WindowWidth, Console.WindowHeight, screenColor);
    }

    public static void Recalculate()
    {
        Size = new Vector2(Console.WindowWidth / 2, Console.WindowHeight);
    }

    public static Vector2 GetCenter()
    {
        return new Vector2(Size.X / 2, Size.Y / 2);
    }
}