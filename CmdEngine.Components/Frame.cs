using CmdEngine.Core.Data;
using CmdEngine.Core.Rendering;

namespace CmdEngine.Core.Components;

public class Frame : GraphicBase
{
    public Frame(GameObject gameObject) : base(gameObject) { }

    public ConsoleColor Color { get; set; } = ConsoleColor.DarkCyan;
    public Vector2 Margin { get; set; } = new Vector2(5, 5);

    public Vector2 Size => GameScreen.Size - Margin * new Vector2(2, 2);

    public bool InsideFrame(Vector2 position)
    {
        if (position.X > Margin.X
            && position.X < GameScreen.Size.X - Margin.X
            && position.Y > Margin.Y
            && position.Y < GameScreen.Size.Y - Margin.Y)
            return true;

        return false;
    }

    public override void Draw()
    {
        var topLeft = new Vector2(Margin.X, Margin.Y);
        var topRight = new Vector2(GameScreen.Size.X - Margin.X, Margin.Y);
        var lowerLeft = new Vector2(Margin.X, GameScreen.Size.Y - Margin.Y);
        var lowerRight = new Vector2(GameScreen.Size.X - Margin.X, GameScreen.Size.Y - Margin.Y);

        // Top.
        DrawHelper.DrawStraightLine(topLeft, topRight, new ColorChar(' ', Color));

        // Bottom.
        DrawHelper.DrawStraightLine(lowerLeft, lowerRight, new ColorChar(' ', Color));

        // Left.
        DrawHelper.DrawStraightLine(topLeft, lowerLeft, new ColorChar(' ', Color));

        // Right.
        DrawHelper.DrawStraightLine(topRight, lowerRight + new Vector2(0, 1), new ColorChar(' ', Color));
    }
}
