using System.Text.RegularExpressions;

namespace AdventOfCode2024.Core;

public abstract class DayXX
{
    private static readonly Regex DayIndexRegex = new(@"Day(\d\d)");

    private string DayIndexStr
    {
        get
        {
            var match = DayIndexRegex.Match(GetType().Name);
            return match.Groups[1].Value;
        }
    }

    public void Part1(eInputMode mode = eInputMode.RealInput)
    {
        Console.WriteLine();
        Console.WriteLine($"------ DAY {DayIndexStr} PART 1 ------");
        Console.WriteLine("---------------------------");

        var lines = ParseInput(mode, ePart.Part1);
        SolvePart1(lines);
    }

    public void Part2(eInputMode mode = eInputMode.RealInput)
    {
        Console.WriteLine();
        Console.WriteLine($"------ DAY {DayIndexStr} PART 2 ------");
        Console.WriteLine("---------------------------");

        var lines = ParseInput(mode, ePart.Part2);
        SolvePart2(lines);
    }

    protected void PrintResult(object result)
    {
        Console.WriteLine($"Result: {result}");
    }

    protected abstract void SolvePart1(List<string> lines);

    protected abstract void SolvePart2(List<string> lines);

    private List<string> ParseInput(eInputMode mode, ePart part)
    {
        return mode switch
        {
            eInputMode.StdIn => ReadInputFromStdIn(),
            eInputMode.RealInput => ReadInputFromFile(),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
    }

    private List<string> ReadInputFromStdIn()
    {
        Console.WriteLine("Insert the problem input: ");
        Console.WriteLine();

        var lines = new List<string>();

        string? line;
        while (!string.IsNullOrEmpty(line = Console.ReadLine()))
        {
            lines.Add(line);
        }

        return lines;
    }

    // .\DayXX\Input
    private List<string> ReadInputFromFile() =>
        File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "Inputs", $"{DayIndexStr}.txt"))
            .ToList();
}