using System.Diagnostics.Contracts;

var lines = File.ReadAllLines("input1.txt").ToList();
lines.Add("");

var monkeys = lines.Select((l, i) => new {l, GroupIndex = i / 7})
    .GroupBy(x => x.GroupIndex)
    .Select(g => g.Select(x => x.l).ToArray())
    .Select(ParseMonkey)
    .OrderBy(m => m.Index)
    .ToArray();

for (int i = 0; i < 20; i++)
{
    MakeRound(monkeys, i + 1);
}

var result = monkeys.Select(m => m.InspectedItemsCount)
    .OrderDescending()
    .Take(2)
    .Aggregate(1, (x, y) => x * y);

// 2851200 - high
Console.WriteLine(result);

Monkey ParseMonkey(string[] lines)
{
    Contract.Assert(lines.Length == 7);

    var monkey = int.Parse(lines[0].Replace("Monkey ", "").Replace(":", ""));

    var items = lines[1].Replace("  Starting items: ", "")
        .Split(", ")
        .Select(int.Parse)
        .ToList();

    var operation = ParseOperation(lines[2]);
    var testDivisibleByValue = int.Parse(lines[3].Replace("  Test: divisible by ", ""));
    var trueMonkey = int.Parse(lines[4].Replace("    If true: throw to monkey ", ""));
    var falseMonkey = int.Parse(lines[5].Replace("    If false: throw to monkey ", ""));

    Func<int, int> ParseOperation(string operationStr)
    {
        var (leftStr, operand, rightSre) = operationStr.Replace("  Operation: new = ", "")
            .Split(" ");
        var isInt = int.TryParse(rightSre, out var right);
        return operand switch
        {
            "+" => old => old + (isInt ? right : old),
            "-" => old => old - (isInt ? right : old),
            "*" => old => old * (isInt ? right : old),
            "/" => old => old / (isInt ? right : old),
            _ => throw new ArgumentOutOfRangeException("Invalid operand"),
        };
    }

    return new Monkey(monkey, items, operation, testDivisibleByValue, trueMonkey, falseMonkey);
}

void MakeRound(Monkey[] monkeys, int roundIndex)
{
    Console.WriteLine($"Round: {roundIndex}");

    foreach (var monkey in monkeys)
    {
        if (monkey.Items.Count == 0)
        {
            continue;
        }

        for (var i = 0; i < monkey.Items.Count; i++)
        {
            var item = monkey.Items[i];
            var worryLevel = monkey.Operation(item);
            var correctedWorryLevel = worryLevel / 3;
            var targetMonkeyIndex = correctedWorryLevel % monkey.TestDivisibleByValue == 0
                ? monkey.TrueMonkey
                : monkey.FalseMonkey;
            monkey.InspectedItemsCount++;
            monkeys.Single(m => m.Index == targetMonkeyIndex).Items.Add(correctedWorryLevel);
        }

        monkey.Items.RemoveRange(0, monkey.Items.Count);
    }

    foreach (var monkey in monkeys)
    {
        Console.WriteLine(monkey);
    }

    Console.WriteLine();
}

record Monkey(int Index, List<int> Items, Func<int, int> Operation, int TestDivisibleByValue,
    int TrueMonkey, int FalseMonkey)
{
    public int InspectedItemsCount { get; set; } = 0;

    public override string ToString()
    {
        return $"Monkey {Index}: {string.Join(", ", Items)}";
    }
}