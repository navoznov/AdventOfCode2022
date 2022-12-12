public static class StringExtensions
{
    public static void Deconstruct(this string[] parts, out string part0, out string part1)
    {
        part0 = parts[0];
        part1 = parts[1];
    }

    public static void Deconstruct(this string[] parts, out string part0, out string part1, out string part2)
    {
        part0 = parts[0];
        part1 = parts[1];
        part2 = parts[2];
    }
}