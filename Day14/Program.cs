// See https://aka.ms/new-console-template for more information

using ResultNet;

var lines = File.ReadLines("input.txt");
var rules = lines.Select(ParseRule).ToArray();

var allPoints = rules.SelectMany(x => x.Points).ToArray();
var maxX = allPoints.Max(p => p.X);
var maxY = allPoints.Max(p => p.Y);
var minX = allPoints.Min(p => p.X);
var minY = allPoints.Min(p => p.Y);

var skipLeft = 450;

var map = new CellContent[maxX + 1, maxY + 2]; // one for the abyss

FillMap(map, rules);
PrintMap(map, skipLeft);

var sandFallingStartPoint = new Point(500, 0);

var i = 0;
for (;; i++)
{
    var checkNextSandPieceFallsToAbyss = CheckNextSandPieceFallsToAbyss(map, sandFallingStartPoint);
    PrintMap(map, skipLeft);
    if (checkNextSandPieceFallsToAbyss)
    {
        break;
    }
}

Console.WriteLine(i);


void FillMap(CellContent[,] map, Rule[] rules)
{
    foreach (var rule in rules)
    {
        ApplyRule(map, rule);
    }
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

Vector GetNormalizedVector(Vector vector)
{
    return new Vector(Math.Sign(vector.X), Math.Sign(vector.Y));
}

bool CheckNextSandPieceFallsToAbyss(CellContent[,] map, Point sandFallingStartPoint)
{
    var currentSandPoint = sandFallingStartPoint;
    while (true)
    {
        var nextStepResult = TryGetSandNextStepPoint(map, currentSandPoint);
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

void PrintMap(CellContent[,] map, int skipLeft)
{
    for (var y = 0; y < maxY + 2; y++)
    {
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


Result<Point> TryGetSandNextStepPoint(CellContent[,] map, Point currentSandPoint)
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
        if (map[nextPoint.X, nextPoint.Y] == CellContent.Air)
        {
            return Result.Success(nextPoint);
        }
    }

    return Result.Failure<Point>();
}


Console.WriteLine("Finish");

Rule ParseRule(string line)
{
    var points = line.Split(" -> ")
        .Select(ParsePoint)
        .ToArray();
    return new Rule(points);
}

Point ParsePoint(string str)
{
    var (xStr, yStr) = str.Split(',');
    return new Point(int.Parse(xStr), int.Parse(yStr));
}

enum CellContent
{
    Air = 0,
    Rock,
    Sand,
}

public record Rule(Point[] Points);