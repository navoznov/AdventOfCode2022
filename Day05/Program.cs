using System.Diagnostics.Contracts;

var lines = File.ReadAllLines("input1.txt");


Part1();

void Part1()
{
    var (stacks, instructions) = Parse(lines);
    foreach (var instruction in instructions)
    {
        ProcessInstruction(stacks, instruction, false);
    }

    var message = GetStacksMessage(stacks);
    Console.WriteLine(message);
}

Part2();

void Part2()
{
    var (stacks, instructions) = Parse(lines);
    foreach (var instruction in instructions)
    {
        ProcessInstruction(stacks, instruction, true);
    }

    var message = GetStacksMessage(stacks);
    Console.WriteLine(message);
}

(Stack<char>[], Instruction[]) Parse(string[] lines)
{
    var emptyLineIndex = GetEmptyLineIndex(lines);
    var stackLines = lines[0..(emptyLineIndex - 1)];
    var instructionLines = lines[(emptyLineIndex + 1)..];

    var stacks = ParseStacks(stackLines);
    var instructions = instructionLines.Select(ParseInstruction).ToArray();
    return (stacks, instructions);
}

int GetEmptyLineIndex(string[] lines)
{
    for (var i = 0; i < lines.Length; i++)
    {
        if (lines[i].Length == 0)
        {
            return i;
        }
    }

    throw new Exception("Empty line was not found");
}

Stack<char>[] ParseStacks(string[] lines)
{
    var reversedLines = lines.Reverse().ToArray();
    var stacksCount = (reversedLines.First().Length + 1) / 4;
    var stackItemIndexes = Enumerable.Range(0, stacksCount).Select(i => i * 4 + 1).ToArray();
    var stacks = stackItemIndexes.Select(_ => new Stack<char>()).ToArray();
    foreach (var line in reversedLines)
    {
        var items = stackItemIndexes.Select(i=>line[i]).ToArray();
        for (var i = 0; i < items.Length; i++)
        {
            var item = items[i];
            if (item != ' ')
            {
                stacks[i].Push(item);
            }
        }
    }

    return stacks;
}

Instruction ParseInstruction(string line)
{
    // example "move 1 from 2 to 1"
    var parts = line.Split(" ");
    return new Instruction(int.Parse(parts[1]), int.Parse(parts[3]) - 1, int.Parse(parts[5]) - 1);
}

void ProcessInstruction(Stack<char>[] stacks, Instruction instruction, bool reverseHand)
{
    var craneHand = Enumerable.Range(0, instruction.Count)
        .Select(_ => stacks[instruction.From].Pop())
        .ToArray();
    
    if (reverseHand)
    {
        craneHand = craneHand.Reverse().ToArray();
    }
    
    foreach (var item in craneHand)
    {
        stacks[instruction.To].Push(item);
    }
}


void Print(Stack<char>[] stacks)
{
    foreach (var stack in stacks)
    {
        Console.WriteLine(stack.Reverse().ToArray());
    }

    Console.WriteLine();
}

string GetStacksMessage(Stack<char>[] chars)
{
    return new string(chars.Select(s => s.Pop()).ToArray());
}

record Instruction(int Count, int From, int To);