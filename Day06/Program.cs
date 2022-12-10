var input = File.ReadAllText("input1.txt").AsSpan();
Part1(input);
Part2(input);

void Part1(ReadOnlySpan<char> input)
{
    const int MARKER_LENGTH = 4;
    var markLetterIndex = GetMarkLetterIndex(input, MARKER_LENGTH);
    Console.WriteLine(markLetterIndex + MARKER_LENGTH);
}

void Part2(ReadOnlySpan<char> input)
{
    const int MARKER_LENGTH = 14;
    var markLetterIndex = GetMarkLetterIndex(input, MARKER_LENGTH);
    Console.WriteLine(markLetterIndex + MARKER_LENGTH);
}


int GetMarkLetterIndex(ReadOnlySpan<char> readOnlySpan, int markerLength)
{
    for (var i = 0; i < readOnlySpan.Length - markerLength; i++)
    {
        var substring = readOnlySpan[i..(i + markerLength)];
        if (CheckDistinctLetters(substring))
        {
            return i;
        }
    }

    return -1;
}

bool CheckDistinctLetters(ReadOnlySpan<char> str)
{
    return str.ToArray().Distinct().Count() == str.Length;
}