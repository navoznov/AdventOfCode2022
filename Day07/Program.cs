var lines = System.IO.File.ReadAllLines("input1.txt");

var (dirs, files) = ParseInput(lines);

Part1();
// 34257857 - too hogh
Part2();

(HashSet<string>, Dictionary<string, int>) ParseInput(string[] lines)
{
    var dirs = new HashSet<string>();
    var files = new Dictionary<string, int>();
    
    var current = "/";
    foreach (var line in lines.Skip(1))
    {
        if (line.StartsWith("$ cd "))
        {
            var dirName = line[5..];
            if (dirName == "..")
            {
                current = current[0..(current.LastIndexOf("/") - 1)];
            }
            else
            {
                // "$ cd dirName"
                current = $"{current}{dirName}/";
                dirs.Add(current);
            }
        }
        else if (line.StartsWith("$ ls"))
        {
        }
        else if (line.StartsWith("dir"))
        {
            var dirName = line.Split(" ").Last();

            dirs.Add($"{current}{dirName}/");
        }
        else
        {
            var parts = line.Split(" ");
            var fileSize = int.Parse(parts.First());
            var fileName = parts.Last();

            files.Add($"{current}/{fileName}", fileSize);
        }
    }

    return (dirs, files);
}

void Part1()
{
    var bigDirSizeSum = dirs.Select(GetDirectorySize)
        .Where(s => s <= 100_000)
        .Sum();
    Console.WriteLine(bigDirSizeSum);
}

void Part2()
{
    const int TOTAL_DISK_SIZE = 70_000_000;
    var allFilesSize = files.Values.Sum();
    var freeSpaceSize = TOTAL_DISK_SIZE - allFilesSize;
    const int MIN_SPACE = 30_000_000;
    var needToFreeSize = MIN_SPACE - freeSpaceSize;

    var enoughSizes = dirs
        .Select(GetDirectorySize)
        .Where(x => x >= needToFreeSize)
        .ToArray();
    var totalSize = enoughSizes.Min();
    Console.WriteLine(totalSize);
}

int GetDirectorySize(string dir)
{
    return files.Where(f => f.Key.StartsWith(dir)).Sum(f => f.Value);
}