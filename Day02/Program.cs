var lines = File.ReadAllLines("input1.txt");
Part1(lines);
Part2(lines);

void Part1(string[] strings)
{
    var casesToScore = new Dictionary<string, int>
    {
        {"A X", 1 + 3},
        {"A Y", 2 + 6},
        {"A Z", 3 + 0},
        {"B X", 1 + 0},
        {"B Y", 2 + 3},
        {"B Z", 3 + 6},
        {"C X", 1 + 6},
        {"C Y", 2 + 0},
        {"C Z", 3 + 3},
    };

    var scores = strings.Select(l => casesToScore[l]).ToArray();
    Console.WriteLine(scores.Sum());
}

void Part2(string[] strings)
{
    var casesToScore = new Dictionary<string, int>
    {
        {"A X", 3 + 0},
        {"A Y", 1 + 3},
        {"A Z", 2 + 6},
        {"B X", 1 + 0},
        {"B Y", 2 + 3},
        {"B Z", 3 + 6},
        {"C X", 2 + 0},
        {"C Y", 3 + 3},
        {"C Z", 1 + 6},
    };

    var scores = strings.Select(l => casesToScore[l]).ToArray();
    Console.WriteLine(scores.Sum());
}