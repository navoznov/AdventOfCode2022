// See https://aka.ms/new-console-template for more information

using ResultNet;

var lines = File.ReadLines("input.txt");
var rules = lines.Select(ParseRule).ToArray();

var allPoints = rules.SelectMany(x => x.Points).ToArray();
var maxX = allPoints.Max(p => p.X);
var maxY = allPoints.Max(p => p.Y);
var minX = allPoints.Min(p => p.X);
var minY = allPoints.Min(p => p.Y);
var sandFallingStartPoint = new Point(500, 0);

Console.WriteLine(Part1());
Console.WriteLine(Part2());

int Part1()
{
    var map = CreateMap(maxX, maxY + 2, rules);

    var i = 0;
    for (;; i++)
    {
        var checkNextSandPieceFallsToAbyss = CheckNextSandPieceFallsToAbyss(map, maxX + 1, sandFallingStartPoint);
        if (checkNextSandPieceFallsToAbyss)
        {
            return i;
        }
    }
}

int Part2()
{
    var rockBottomLineRule = new Rule(new Point[] { new Point(0, maxY + 2), new Point(maxX + maxY, maxY + 2) });
    var newRules = rules.Union(new[] { rockBottomLineRule }).ToArray();
    var map = CreateMap(maxX + maxY, maxY + 1, newRules);
    PrintMap(map, maxX + maxY, 450, 1);
    var i = 0;
    for (;; i++)
    {
        var checkNextSandPieceBlockStartPoint =
            CheckNextSandPieceBlockStartPoint(map, maxX + maxY + 1, sandFallingStartPoint);
        if (!checkNextSandPieceBlockStartPoint)
        {
            continue;
        }

        PrintMap(map, maxX + maxY, 450, 1);
        return i + 1;
    }
}

bool CheckNextSandPieceFallsToAbyss(CellContent[,] map, int xSize, Point sandFallingStartPoint)
{
    var currentSandPoint = sandFallingStartPoint;
    while (true)
    {
        var nextStepResult = TryGetSandNextStepPoint(map, xSize, currentSandPoint);
        if (nextStepResult.IsFailure())
        {
            break;
        }

        map[currentSandPoint.X, currentSandPoint.Y] = CellContent.Air;

        var nextSandPoint = nextStepResult.Data;
        map[nextSandPoint.X, nextSandPoint.Y] = CellContent.Sand;
        if (nextSandPoint.Y == maxY + 1)
        {
            return true;
        }

        currentSandPoint = nextSandPoint;
    }

    return false;
}

bool CheckNextSandPieceBlockStartPoint(CellContent[,] map, int xSize, Point sandFallingStartPoint)
{
    var currentSandPoint = sandFallingStartPoint;
    while (true)
    {
        var nextStepResult = TryGetSandNextStepPoint(map, xSize, currentSandPoint);
        if (nextStepResult.IsFailure())
        {
            return currentSandPoint == sandFallingStartPoint;
        }

        map[currentSandPoint.X, currentSandPoint.Y] = CellContent.Air;

        var nextSandPoint = nextStepResult.Data;
        map[nextSandPoint.X, nextSandPoint.Y] = CellContent.Sand;
        currentSandPoint = nextSandPoint;
    }
}

Result<Point> TryGetSandNextStepPoint(CellContent[,] map, int xSize, Point currentSandPoint)
{
    var sandDirectionsPriority = new[]
    {
        new Vector(0, 1),
        new Vector(-1, 1),
        new Vector(1, 1),
    };

    foreach (var fallingDirection in sandDirectionsPriority)
    {
        var nextPoint = currentSandPoint + fallingDirection;
        try
        {
            if (nextPoint.X < 0 || nextPoint.X >= xSize)
            {
                return Result.Failure<Point>();
            }

            if (map[nextPoint.X, nextPoint.Y] == CellContent.Air)
            {
                return Result.Success(nextPoint);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    return Result.Failure<Point>();
}

Rule ParseRule(string line)
{
    var points = line.Split(" -> ")
        .Select(ParsePoint)
        .ToArray();
    return new Rule(points);

    Point ParsePoint(string str)
    {
        var (xStr, yStr) = str.Split(',');
        return new Point(int.Parse(xStr), int.Parse(yStr));
    }
}

CellContent[,] CreateMap(int maxX, int maxY, Rule[] rules)
{
    Vector GetNormalizedVector(Vector vector)
    {
        return new Vector(Math.Sign(vector.X), Math.Sign(vector.Y));
    }

    void ApplyRule(CellContent[,] map, Rule rule)
    {
        var points = rule.Points;
        var startPoint = points.First();
        map[startPoint.X, startPoint.Y] = CellContent.Rock;

        for (var j = 1; j < points.Length; j++)
        {
            var nextPoint = points[j];
            var pointVector = nextPoint - startPoint;
            var directionVector = GetNormalizedVector(pointVector);
            var currentPoint = startPoint;
            while (currentPoint != nextPoint)
            {
                currentPoint = currentPoint + directionVector;
                map[currentPoint.X, currentPoint.Y] = CellContent.Rock;
            }

            startPoint = nextPoint;
        }
    }

    void FillMap(CellContent[,] map, Rule[] rules)
    {
        foreach (var rule in rules)
        {
            ApplyRule(map, rule);
        }
    }

    var cellContents = new CellContent[maxX + 1, maxY + 2]; // one for the abyss
    FillMap(cellContents, rules);
    return cellContents;
}

void PrintMap(CellContent[,] map, int maxX, int skipLeft, int addY = 0)
{
    for (var y = 0; y < maxY + 2 + addY; y++)
    {
        Console.Write(y.ToString().PadLeft(3, ' ') + ' ');

        for (var x = skipLeft - 1; x < maxX + 1; x++)
        {
            var ch = map[x, y] switch
            {
                CellContent.Air => '.',
                CellContent.Rock => '#',
                CellContent.Sand => 'o',
                _ => throw new ArgumentOutOfRangeException()
            };
            Console.Write(ch);
        }

        Console.WriteLine();
    }
}

enum CellContent
{
    Air = 0,
    Rock,
    Sand,
}

public record Rule(Point[] Points);