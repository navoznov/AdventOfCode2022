using System.Text;

namespace Common;

public class Map2d<T>
{
    public int XSize { get; init; }
    public int YSize { get; init; }

    private readonly T[,] _map;
    public int XMin { get; init; }
    public int XMax { get; init; }
    public int YMin { get; init; }
    public int YMax { get; init; }

    public Map2d(int xSize, int ySize, Func<T>? initValueCreator = null)
    {
        XSize = xSize;
        YSize = ySize;
        
        _map = new T[xSize, ySize];
        
        for (var i = 0; i < xSize; i++)
        {
            for (var j = 0; j < ySize; j++)
            {
                _map[i, j] = (initValueCreator != null ? initValueCreator() : default)!;
            }
        }
    }

    public Map2d(int xMin, int xMax, int yMin, int yMax, Func<T>? initValueCreator = null)
        : this(xMax - xMin + 1, yMax - yMin + 1, initValueCreator)
    {
        XMin = xMin;
        XMax = xMax;
        YMin = yMin;
        YMax = yMax;
    }


    public T this[Point point]
    {
        get => _map[point.X - XMin, point.Y - YMin];
        set => _map[point.X - XMin, point.Y - YMin] = value;
    }

    public T this[int x, int y]
    {
        get => _map[x - XMin, y - YMin];
        set => _map[x - XMin, y - YMin] = value;
    }

    public bool Exists(Point point)
    {
        return point.X - XMin < XSize
               && point.X - XMin > -1
               && point.Y - YMin < YSize
               && point.Y - YMin > -1;
    }

    public string ToString(Func<T, string> valuePrinter, bool addSpaces = true, bool addRowNumbers = false)
    {
        var stringBuilder = new StringBuilder();
        for (var y = 0; y < YSize; y++)
        {
            if (addRowNumbers)
            {
                stringBuilder.Append(y.ToString().PadLeft(3)).Append(' ');
            }
            
            for (var x = 0; x < XSize; x++)
            {
                var cellValue = _map[x, y];
                var str = valuePrinter(cellValue);
                stringBuilder.Append(str);
                if (addSpaces)
                {
                    stringBuilder.Append(' ');
                }
            }

            stringBuilder.AppendLine();
        }

        return stringBuilder.ToString();
    }
}