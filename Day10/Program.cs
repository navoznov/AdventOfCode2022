using System.Text;

var lines = File.ReadAllLines("input1.txt");
var commands = lines.Select(ParseCommand).ToArray();

var signals = GetSignals(commands).ToArray();
Part1();
Part2();

void Part1()
{
    var cycles = new[] {20, 60, 100, 140, 180, 220};
    var signalStrengths = signals
        .Where(s => cycles.Contains(s.Cycle))
        .Select(s => s.X * s.Cycle);

    Console.WriteLine(signalStrengths.Sum());
}

void Part2()
{
    var screen = new StringBuilder();
    foreach (var signal in signals)
    {
        var screenCursorPosition = (signal.Cycle - 1) % 40;
        var letter = Math.Abs(signal.X - screenCursorPosition) < 2 ? '#' : '.';
        screen.Append(letter);

        if (signal.Cycle % 40 == 0)
        {
            screen.AppendLine();
        }
    }

    Console.WriteLine(screen);
}

ICommand ParseCommand(string line)
{
    if (line == "noop")
    {
        return new NoopCommand();
    }

    var (_, valueStr) = line.Split(" ");
    return new AddCommand(int.Parse(valueStr));
}

IEnumerable<Signal> GetSignals(IEnumerable<ICommand> commands)
{
    var cycle = 1;
    int x = 1;
    foreach (var command in commands)
    {
        if (command is NoopCommand)
        {
            yield return new Signal(cycle++, x);
        }
        else if (command is AddCommand addCommand)
        {
            yield return new Signal(cycle++, x);
            yield return new Signal(cycle++, x);
            x += addCommand.Value;
        }
    }
}

record Signal(int Cycle, int X);

interface ICommand
{
}

record AddCommand(int Value) : ICommand;

record NoopCommand() : ICommand;