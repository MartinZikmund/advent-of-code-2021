namespace Tools;

public struct Point
{
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static implicit operator Point((int x, int y) input) => new Point(input.x, input.y);

    public static Point operator +(Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);

    public static bool operator ==(Point a, Point b) => a.X == b.X && a.Y == b.Y;
    public static bool operator !=(Point a, Point b) => !(a == b);

    public void Deconstruct(out int x, out int y)
    {
        x = X;
        y = Y;
    }

    public int X { get; set; }

    public int Y { get; set; }
}
