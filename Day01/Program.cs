var lines = File.ReadAllLines("input1.txt");
var allCalories = ParseAllCalories(lines);
var summedCalories = allCalories.Select(x => x.Sum());

// Part 1
var maxCalories = summedCalories.Max();
Console.WriteLine(maxCalories);

// Part 2
var caloriesSum = summedCalories.OrderDescending().Take(3).Sum();
Console.WriteLine(caloriesSum);

List<List<int>> ParseAllCalories(string[] strings)
{
    var allCalories = new List<List<int>>();
    var calories = new List<int>();
    foreach (var line in strings)
    {
        if (string.IsNullOrEmpty(line))
        {
            allCalories.Add(calories);
            calories = new List<int>();
            continue;
        }

        var caloriesCount = int.Parse(line);
        calories.Add(caloriesCount);
    }

    allCalories.Add(calories);
    return allCalories;
}