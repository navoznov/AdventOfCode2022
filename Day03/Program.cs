using System.Diagnostics.Contracts;

const string ABC = "abcdefghijklmnopqrstuvwxyz";
var priority = ABC.ToLower() + ABC.ToUpper();

var lines = File.ReadAllLines("input1.txt");


Part1();
Part2();

void Part1()
{
    var doubledItems = lines.Select(GetDoubledItem);
    var priorities = doubledItems.Select(i => priority.IndexOf(i) + 1);
    Console.WriteLine(priorities.Sum());
}

void Part2()
{
    var sum = lines.Select((l,i) => new {Line = l, Index = i})
        .GroupBy(x => x.Index/3, x => x.Line )
        .Select(g => GetCommonItem(g.ToArray()))
        .Select(i => priority.IndexOf(i) + 1)
        .Sum();
    
    Console.WriteLine(sum);
}

char GetDoubledItem(string rucksack)
{
    var first = rucksack[..(rucksack.Length/2)].Distinct().Order();
    var second = rucksack[(rucksack.Length/2)..].Distinct().Order();
    var intersection = first.Intersect(second).ToArray();
    return intersection.First();
}

char GetCommonItem(string[] rucksacks)
{
    Contract.Assert(rucksacks.Length == 3);
    return rucksacks[0].First(l1 => rucksacks[1].IndexOf(l1) != -1 && rucksacks[2].IndexOf(l1) != -1);
}

