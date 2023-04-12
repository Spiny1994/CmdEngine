using CmdEngine.Core.Data;

namespace CmdEngine.Core.Rendering
{
    public class DrawHelper
    {
        public static void DrawSingle(Vector2 position, ColorChar colorChar)
        {
            var adjustedPostion = new Vector2(position.X * 2, position.Y);

            ConsoleBuffer.SetTile(adjustedPostion.X, adjustedPostion.Y, colorChar.Char, colorChar.ColorSet.ForegroundColor, colorChar.ColorSet.BackgroundColor);
            ConsoleBuffer.SetTile(adjustedPostion.X + 1, adjustedPostion.Y, colorChar.Char, colorChar.ColorSet.ForegroundColor, colorChar.ColorSet.BackgroundColor);
        }

        public static void DrawStraightLine(Vector2 from, Vector2 to, ColorChar colorChar)
        {
            // Not straight.
            if (from.X != to.X && from.Y != to.Y)
                return;

            // Vertical.
            if (from.X == to.X)
            {
                var smallest = from.Y < to.Y ? from.Y : to.Y;
                var biggest = smallest == from.Y ? to.Y : from.Y;

                for (var y = smallest; y < biggest; y++)
                    DrawSingle(new Vector2(from.X, y), colorChar);
            }
            else // Horizontal.
            {
                var smallest = from.X < to.X ? from.X : to.X;
                var biggest = smallest == from.X ? to.X : from.X;

                for (var x = smallest; x < biggest; x++)
                    DrawSingle(new Vector2(x, from.Y), colorChar);
            }
        }

        public static void DrawText(int x, int y, string text, ColorSet colors = new ColorSet(), bool centerText = false)
        {
            ConsoleBuffer.DrawText(x * 2, y, text, colors.ForegroundColor, colors.BackgroundColor, centerText);
        }
    }
}