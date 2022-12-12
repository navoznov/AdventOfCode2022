using System.Diagnostics.Contracts;

var lines = File.ReadAllLines("input1.txt").ToList();
lines.Add("");

var monkeys = lines.Select((l, i) => new {l, GroupIndex = i / 7})
    .GroupBy(x => x.GroupIndex)
    .Select(g => g.Select(x => x.l).ToArray())
    .Select(ParseMonkey)
    .OrderBy(m => m.Index);

var worryFactor = monkeys
    .Select(m => m.TestDivisibleByValue)
    .Aggregate(1, (x, y) => x * y);
Part1(monkeys.ToArray());
Part2(monkeys.ToArray());

void Part1(Monkey[] monkeys)
{
    for (var i = 0; i < 20; i++)
    {
        MakeRound(monkeys, worryFactor, false);
    }

    var result = monkeys.Select(m => m.InspectedItemsCount)
        .OrderDescending()
        .Take(2)
        .Aggregate(1, (x, y) => x * y);

    Console.WriteLine(result);
}

void Part2(Monkey[] monkeys)
{
    for (var i = 0; i < 10000; i++)
    {
        MakeRound(monkeys, worryFactor, true);
    }

    var result = monkeys.Select(m => m.InspectedItemsCount)
        .OrderDescending()
        .Take(2)
        .Aggregate(1L, (x, y) => x * y);

    Console.WriteLine(result);
}

Monkey ParseMonkey(string[] lines)
{
    Contract.Assert(lines.Length == 7);

    var monkey = int.Parse(lines[0].Replace("Monkey ", "").Replace(":", ""));

    var items = lines[1].Replace("  Starting items: ", "")
        .Split(", ")
        .Select(long.Parse)
        .ToList();

    var operation = ParseOperation(lines[2]);
    var testDivisibleByValue = int.Parse(lines[3].Replace("  Test: divisible by ", ""));
    var trueMonkey = int.Parse(lines[4].Replace("    If true: throw to monkey ", ""));
    var falseMonkey = int.Parse(lines[5].Replace("    If false: throw to monkey ", ""));

    Func<long, long> ParseOperation(string operationStr)
    {
        var (_, operand, rightSre) = operationStr.Replace("  Operation: new = ", "")
            .Split(" ");
        var isInt = long.TryParse(rightSre, out var right);
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

void MakeRound(Monkey[] monkeys, long worryFactor, bool isWorried)
{
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
            if (!isWorried)
            {
                worryLevel /= 3;
            }

            worryLevel %= worryFactor;
            var targetMonkeyIndex = worryLevel % monkey.TestDivisibleByValue == 0
                ? monkey.TrueMonkey
                : monkey.FalseMonkey;
            monkey.InspectedItemsCount++;
            monkeys.Single(m => m.Index == targetMonkeyIndex).Items.Add(worryLevel);
        }

        monkey.Items.RemoveRange(0, monkey.Items.Count);
    }
}

record Monkey(int Index, List<long> Items, Func<long, long> Operation, int TestDivisibleByValue,
    int TrueMonkey, int FalseMonkey)
{
    public int InspectedItemsCount { get; set; } = 0;

    public override string ToString()
    {
        return $"Monkey {Index}: {InspectedItemsCount}";
    }
}