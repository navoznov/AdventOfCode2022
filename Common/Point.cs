public record Point(int X, int Y)
{
    public static Point operator +(Point point, Vector vector)
    {
        return new Point(point.X + vector.X, point.Y + vector.Y);
    }

    public static Vector operator -(Point point1, Point point2)
    {
        return new Vector(point1.X - point2.X, point1.Y - point2.Y);
    }
}