var lines = File.ReadAllLines("input1.txt");
var directionVectors = new Dictionary<string, Vector>
{
    {"U", new Vector(0, -1)},
    {"D", new Vector(0, 1)},
    {"L", new Vector(-1, 0)},
    {"R", new Vector(1, 0)},
};
var rules = lines.Select(ParseLine).ToArray();

// Part1();
Part2();

Rule ParseLine(string line)
{
    var (direction, countStr) = line.Split(" ");
    return new Rule(directionVectors[direction], int.Parse(countStr));
}

void Part1()
{
    var startPoint = new Point(0, 0);
    var head = startPoint;
    var tail = head;
    var tailVisitedPoints = new HashSet<Point>();
    foreach (var rule in rules)
    {
        // Console.WriteLine($"*** {rule}");
        // Console.WriteLine();
        for (var step = 0; step < rule.Count; step++)
        {
            head += rule.Vector;
            var tailMovementVector = CalculateTailMovementVector(head, tail);
            tail = tail + tailMovementVector;
            tailVisitedPoints.Add(tail);

            // Console.WriteLine($"Head: {head}");
            // Console.WriteLine($"Tail: {tail}");
            // Console.WriteLine();
        }
    }

    Console.WriteLine($"Visited points count: {tailVisitedPoints.Count}");
}

void Part2()
{
    const int ROPE_LENGTH = 10;
    var startPoint = new Point(0, 0);
    var rope = Enumerable.Range(0, ROPE_LENGTH).Select(_ => startPoint).ToArray();
    var tailVisitedPoints = new HashSet<Point>();
    foreach (var rule in rules)
    {
        for (var step = 0; step < rule.Count; step++)
        {
            rope[0] += rule.Vector;

            for (var i = 1; i < rope.Length; i++)
            {
                var tailMovementVector = CalculateTailMovementVector(rope[i - 1], rope[i]);
                rope[i] += tailMovementVector;
            }

            tailVisitedPoints.Add(rope.Last());
        }
    }

    Console.WriteLine($"Visited points count: {tailVisitedPoints.Count}");
}

Vector CalculateTailMovementVector(Point head, Point tail)
{
    var vector = head - tail;

    if (Math.Abs(vector.X) == 2)
    {
        return vector with {X = vector.X / 2};
    }

    if (Math.Abs(vector.Y) == 2)
    {
        return vector with {Y = vector.Y / 2};
    }

    return new Vector(0, 0);
}

record Rule(Vector Vector, int Count);