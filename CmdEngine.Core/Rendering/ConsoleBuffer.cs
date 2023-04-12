using System.Diagnostics;

namespace CmdEngine.Core.Rendering;

public static class ConsoleBuffer
{
    private static bool _initialized;

    private static ConsoleTile[,]? _buffer { get; set; }
    private static ConsoleTile[,]? _lastFrame { get; set; }

    public static ConsoleColor GameBaseRefreshColor { get; set; }

    public static int Width => _buffer?.GetLength(0) ?? 0;
    public static int Height => _buffer?.GetLength(1) ?? 0;

    /// <summary>
    /// Initializes the buffer. This is required to use the buffer.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="defaultBackgroundColor"></param>
    public static void Initialize(
        int width,
        int height,
        ConsoleColor defaultBackgroundColor = ConsoleColor.Black)
    {
        _buffer = new ConsoleTile[width, height];
        _lastFrame = new ConsoleTile[width, height];

        GameBaseRefreshColor = defaultBackgroundColor;

        _initialized = true;

        Clear();
    }

    /// <summary>
    /// Clears the whole buffer with a color.
    /// </summary>
    /// <param name="color"></param>
    public static void Clear()
    {
        if (!_initialized)
        {
            Debug.Print("Failed to clear ConsoleBuffer. Please initialize it first.");
            return;
        }

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                // Dont' wanna use SetTile function here, because I'm doing a "blank" check in there so that the sprites can have transparency.
                // Since all tiles are set to "blank" in this function, using SetTile would give a false positive of transparency.
                _buffer![x, y].Icon = ' ';
                _buffer![x, y].BackgroundColor = GameBaseRefreshColor;
            }
        }
    }

    /// <summary>
    /// Clears a row with a color.
    /// </summary>
    /// <param name="color"></param>
    /// <param name="y"></param>
    public static void ClearRow(ConsoleColor color, int y)
    {
        if (!_initialized)
        {
            Debug.Print("Failed to clear row of ConsoleBuffer. Please initialize it first.");
            return;
        }

        for (var x = 0; x < Width; x++)
            SetTile(x, y, ' ', ConsoleColor.Gray, color);
    }

    /// <summary>
    /// Clears a column with a color.
    /// </summary>
    /// <param name="color"></param>
    /// <param name="x"></param>
    public static void ClearColumn(ConsoleColor color, int x)
    {
        if (!_initialized)
        {
            Debug.Print("Failed to clear column of ConsoleBuffer. Please initialize it first.");
            return;
        }

        for (var y = 0; y < Height; y++)
            SetTile(x, y, ' ', ConsoleColor.Gray, color);
    }

    /// <summary>
    /// Changes a tile of the buffer.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="newTile"></param>
    public static void SetTile(
        int x,
        int y,
        char icon,
        ConsoleColor foregroundColor,
        ConsoleColor backgroundColor)
    {
        if (!_initialized)
        {
            Debug.Print("Failed to set tile of ConsoleBuffer. Please initialize it first.");
            return;
        }

        if (x < Width && y < Height && x >= 0 && y >= 0)
        {
            if (backgroundColor != GameBaseRefreshColor || icon != ' ')
                _buffer![x, y] = new ConsoleTile(icon, backgroundColor, foregroundColor);
        }
    }

    /// <summary>
    /// Gets a tile from the grid.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static ConsoleTile GetTile(int x, int y)
    {
        if (x <= Width - 1 && y <= Height - 1 && x >= 0 && y >= 0)
            return _buffer![x, y];

        return new ConsoleTile();
    }


    /// <summary>
    /// Draws the buffer onto the console.
    /// </summary>
    public static void Render()
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                if (x != Width - 1 && y != Height - 1)
                {
                    var t = _buffer![x, y];

                    if (_lastFrame![x, y] != t)
                    {
                        Console.SetCursorPosition(x, y);
                        Console.BackgroundColor = t.BackgroundColor;
                        Console.ForegroundColor = t.ForegroundColor;
                        Console.Write(t.Icon);

                        _lastFrame[x, y] = t;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Draws a string.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="text"></param>
    /// <param name="foregroundColor"></param>
    /// <param name="backgroundColor"></param>
    /// <param name="centerText"></param>
    public static void DrawText(
        int x,
        int y,
        string text,
        ConsoleColor foregroundColor = ConsoleColor.Gray,
        ConsoleColor backgroundColor = ConsoleColor.Black,
        bool centerText = false)
    {
        var numberOfSpaces = 0;
        if (centerText)
            numberOfSpaces = Width / 2 - text.Length / 2;

        for (var i = 0; i < text.Length; i++)
        {
            char t = text[i];
            int actualX = numberOfSpaces + x + i;
            int actualY = y;
            SetTile(actualX, actualY, t, foregroundColor, backgroundColor);
        }
    }
}
