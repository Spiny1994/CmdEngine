namespace CmdEngine.Core.Rendering;

public struct ConsoleTile
{
    //The icon to draw.
    public char Icon;

    //The icon's colors.
    public ConsoleColor BackgroundColor;
    public ConsoleColor ForegroundColor;

    public ConsoleTile(char icon)
    {
        Icon = icon;
        BackgroundColor = ConsoleColor.Black;
        ForegroundColor = ConsoleColor.Gray;
    }
    public ConsoleTile(char icon, ConsoleColor bgColor, ConsoleColor fgColor)
    {
        Icon = icon;
        BackgroundColor = bgColor;
        ForegroundColor = fgColor;
    }

    public override bool Equals(object? tile)
    {
        if (tile == null)
            return false;

        ConsoleTile otherTile = (ConsoleTile)tile;
        return Icon == otherTile.Icon &&
            BackgroundColor == otherTile.BackgroundColor &&
            ForegroundColor == otherTile.ForegroundColor;
    }

    public static bool operator ==(ConsoleTile a, ConsoleTile b)
    {
        if (ReferenceEquals(a, b))
            return true;

        return a.Equals(b);
    }

    public static bool operator !=(ConsoleTile a, ConsoleTile b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 * Icon.GetHashCode();
        hash = hash * 23 * BackgroundColor.GetHashCode();
        hash = hash * 23 * ForegroundColor.GetHashCode();
        return hash;
    }
}
