namespace CmdEngine.Core.Data;

public struct Vector2
{
    public static Vector2 Zero => new Vector2(0, 0);
    public static Vector2 Up => new Vector2(0, -1);
    public static Vector2 Down => new Vector2(0, 1);
    public static Vector2 Left => new Vector2(-1, 0);
    public static Vector2 Right => new Vector2(1, 0);

    public int X { get; set; }
    public int Y { get; set; }

    public Vector2(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override bool Equals(object? other)
    {
        if (other == null)
            return false;

        Vector2 otherVector = (Vector2)other;
        return X == otherVector.X &&
            Y == otherVector.Y;
    }

    public static bool operator ==(Vector2 a, Vector2 b)
    {
        if (ReferenceEquals(a, b))
            return true;

        return a.Equals(b);
    }

    public static bool operator !=(Vector2 a, Vector2 b)
    {
        return !(a == b);
    }

    public static Vector2 operator -(Vector2 a, Vector2 b)
    {
        return new Vector2(a.X - b.X, a.Y - b.Y);
    }

    public static Vector2 operator +(Vector2 a, Vector2 b)
    {
        return new Vector2(a.X + b.X, a.Y + b.Y);
    }

    public static Vector2 operator *(Vector2 a, Vector2 b)
    {
        return new Vector2(a.X * b.X, a.Y * b.Y);
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 * X.GetHashCode();
        hash = hash * 23 * Y.GetHashCode();
        return hash;
    }
}