using System.Resources;

var lines = File.ReadAllLines("input1.txt");
var map = ParseInput(lines);
var sizeX = map[0].Length;
var sizeY = map.Length;

Part1();
Part2();

void Part1()
{
    var counter = 0;
    for (int y = 0; y < sizeY; y++)
    {
        for (int x = 0; x < sizeX; x++)
        {
            if (isVisible(map, x, y))
            {
                counter++;
            }
        }
    }

    Console.WriteLine(counter);
}

void Part2()
{
 
    var maxScore = 0;
    for (int y = 0; y < sizeY; y++)
    {
        for (int x = 0; x < sizeX; x++)
        {
            var score = GetTreeScore(map, x, y);
            maxScore = int.Max(score, maxScore);
        }
    }

    Console.WriteLine(maxScore);
}

int[][] ParseInput(string[] lines)
{
    return lines.Select(l => l.ToArray().Select(x => int.Parse(x.ToString())).ToArray()).ToArray();
}

bool isVisible(int[][] map, int treeX, int treeY)
{
    if (treeX == 0 || treeY == 0 || treeX == sizeX - 1 || treeY == sizeY - 1)
    {
        return true;
    }

    var candidate = map[treeY][treeX];
    var left = map[treeY].Take(treeX);
    var right = map[treeY].Skip(treeX + 1);
    if (left.All(t => t < candidate)
        || right.All(t => t < candidate))
    {
        return true;
    }

    var top = Enumerable.Range(0, treeY).Select(y => map[y][treeX]);
    var bottom = Enumerable.Range(treeY + 1, sizeY - treeY - 1).Select(y => map[y][treeX]);
    if (top.All(t => t < candidate)
        || bottom.All(t => t < candidate))
    {
        return true;
    }

    return false;
}

int GetTreeScore(int[][] map, int treeX, int treeY)
{
    if (treeX == 0 || treeY == 0 || treeX == sizeX - 1 || treeY == sizeY - 1)
    {
        return 0;
    }

    var tree = map[treeY][treeX];
    var left = map[treeY].Take(treeX).Reverse().ToArray();
    var right = map[treeY].Skip(treeX + 1).ToArray();
    var top = Enumerable.Range(0, treeY).Select(y => map[y][treeX]).Reverse().ToArray();
    var bottom = Enumerable.Range(treeY + 1, sizeY - treeY - 1).Select(y => map[y][treeX]).ToArray();
    return GetDistance(tree, left)
           * GetDistance(tree, right)
           * GetDistance(tree, top)
           * GetDistance(tree, bottom);
}

int GetDistance(int tree, int[] neighbours)
{
    int counter = 1;
    foreach (var neighbour in neighbours)
    {
        if (neighbour >= tree)
        {
            return counter;
        }

        counter++;
    }

    return neighbours.Length;
}

record Point(int X, int Y);