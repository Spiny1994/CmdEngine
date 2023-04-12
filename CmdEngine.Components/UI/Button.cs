using CmdEngine.Core.Rendering;
using CmdEngine.Core.Components;
using CmdEngine.Core.Data;
using CmdEngine.Core;

namespace CmdEngine.Components.UI;

public class Button : ComponentBase
{
    // ■
    public string Text { get; set; }
    public int Width { get; set; }
    public char Frame { get; set; }
    public ColorSet SelectedColors { get; set; }
    public ColorSet DeselectedColors { get; set; }
    public Anchor Anchor { get; set; }
    public ButtonStyle Style { get; set; }
    public Action? ClickAction { get; set; }

    public Button(
        GameObject gameObject,
        string text,
        Action? clickAction = null) : base(gameObject)
    {
        Text = text;
        Width = 10;
        Frame = ' ';
        SelectedColors = ColorSet.DefaultInverted;
        DeselectedColors = new ColorSet();
        Anchor = Anchor.Center;
        Style = ButtonStyle.Thin;
        ClickAction = clickAction;
    }

    public void Draw(Vector2 position, bool selected)
    {
        var startX = position.X;
        if (Anchor != Anchor.Right)
            startX = Anchor == Anchor.Center ? startX -= Width / 4 : startX -= Width / 2;

        var totalPadding = Width - Text.Length;
        var evenPadding = totalPadding % 2 == 0;
        var rightPadding = totalPadding / 2;
        var leftPadding = totalPadding / 2;
        if (!evenPadding)
            leftPadding = totalPadding / 2 + 1;

        // Build the middle.
        var middleText = string.Empty;
        middleText += Frame;
        for (var left = 0; left < leftPadding - 1; left++) middleText += ' ';
        middleText += Text;
        for (var right = 0; right < rightPadding - 1; right++) middleText += ' ';
        middleText += Frame;

        DrawHelper.DrawText(startX, position.Y, middleText, selected ? SelectedColors : DeselectedColors);

        if (Style != ButtonStyle.Tall)
            return;

        var verticalFrame = string.Empty;
        for (int frame = 0; frame < Width; frame++) verticalFrame += Frame;

        DrawHelper.DrawText(startX, position.Y - 1, verticalFrame, selected ? SelectedColors : DeselectedColors);
        DrawHelper.DrawText(startX, position.Y + 1, verticalFrame, selected ? SelectedColors : DeselectedColors);
    }
}

public enum ButtonStyle
{
    Thin,
    Tall
}

public enum Anchor
{
    Center,
    Left,
    Right
}