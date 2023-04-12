using CmdEngine.Core.Data;
using System.Diagnostics;

namespace CmdEngine.Core.Components;

public class Transform : ComponentBase
{
    public Vector2 Position { get; private set; }

    public Transform(GameObject gameObject) : base(gameObject)
    {
        if (GameObject.Transform != null)
            Debug.Print("Yo!");
    }

    public void Translate(int x, int y)
    {
        Translate(new Vector2(x, y));
    }

    public void Translate(Vector2 translation)
    {
        var oldPos = Position;

        Position = new Vector2(oldPos.X + translation.X, oldPos.Y + translation.Y);
    }

    public void SetPositionHorizontal(int x)
    {
        SetPosition(new Vector2(x, Position.Y));
    }

    public void SetPositionVertical(int y)
    {
        SetPosition(new Vector2(Position.X, y));
    }

    public void SetPosition(int x, int y)
    {
        SetPosition(new Vector2(x, y));
    }

    public void SetPosition(Vector2 position)
    {
        Position = position;
    }
}