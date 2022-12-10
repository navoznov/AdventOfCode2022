using System.Diagnostics.Contracts;

var lines = File.ReadAllLines("input1.txt");
var ranges = lines.Select(ParseRanges).ToArray();
Part1();
Part2();


void Part1()
{
    var fullyContainRanges = ranges.Where(CheckFullyContains);
    Console.WriteLine(fullyContainRanges.Count());
}

void Part2()
{
    var overlappedRanges = ranges.Where(CheckOverlap);
    Console.WriteLine(overlappedRanges.Count());
}

bool CheckFullyContains(Range[] ranges)
{
    var orderedRanges = ranges
        .OrderBy(r=>r.End.Value- r.Start.Value)
        .ToArray();
    var small = orderedRanges.First();
    var big = orderedRanges.Last();
    return big.Start.Value <= small.Start.Value && big.End.Value >= small.End.Value;
}

Range[] ParseRanges(string line)
{
    var ranges = line.Split(",")
        .Select(ParseRange)
        .ToArray();
    Contract.Assert(ranges.Length == 2);
    return ranges;
}

Range ParseRange(string str)
{
    var parts = str.Split("-");
    return new Range(int.Parse(parts[0]), int.Parse(parts[1]));
}

bool CheckOverlap(Range[] ranges)
{
    var orderedRanges = ranges
        .OrderBy(r=>r.Start.Value)
        .ToArray();
    var first = orderedRanges.First();
    var second = orderedRanges.Last();
    return second.Start.Value <= first.End.Value;
}
