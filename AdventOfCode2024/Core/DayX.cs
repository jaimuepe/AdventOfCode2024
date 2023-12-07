using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode2024.Core;

public abstract class DayX
{
    protected bool IsPart1 { get; private set; }

    protected bool IsPart2 => !IsPart1;
    
    private static readonly Regex DayIndexRegex = new(@"Day(\d\d)");
    
    private bool _measureTime;
    private long _beforeTimestamp;

    private eInputMode _mode;
    private int _part;

    protected abstract string Test { get; }

    private string DayIndexStr
    {
        get
        {
            var match = DayIndexRegex.Match(GetType().Name);
            return match.Groups[1].Value;
        }
    }

    public void Part1(eInputMode mode, bool measureTime = false)
    {
        IsPart1 = true;
        
        _measureTime = measureTime;
        _mode = mode;
        _part = 1;

        var lines = ParseInput(mode);

        PreSolve();
        SolvePart1(lines);
        PostSolve();
    }

    public void Part2(eInputMode mode, bool measureTime = false)
    {
        IsPart1 = false;
        
        _measureTime = measureTime;
        _mode = mode;
        _part = 2;
        
        var lines = ParseInput(mode);

        PreSolve();
        SolvePart2(lines);
        PostSolve();
    }

    protected static void PrintResult(object result)
    {
        Console.WriteLine($"Result: {result}");
    }

    protected abstract void SolvePart1(List<string> lines);

    protected abstract void SolvePart2(List<string> lines);

    private void PreSolve()
    {
        Console.WriteLine();
        Console.WriteLine($"------ DAY {DayIndexStr} PART {_part} {_mode} ------");
        Console.WriteLine("---------------------------");
        
        if (_measureTime) _beforeTimestamp = Stopwatch.GetTimestamp();
    }

    private void PostSolve()
    {
        if (_measureTime)
        {
            Console.WriteLine();
            Console.WriteLine($"Elapsed: {Stopwatch.GetElapsedTime(_beforeTimestamp).TotalMilliseconds}ms");
        }
    }

    private List<string> ParseInput(eInputMode mode)
    {
        return mode switch
        {
            eInputMode.Test => ReadInputFromTest(),
            eInputMode.StdIn => ReadInputFromStdIn(),
            eInputMode.RealInput => ReadInputFromFile(),
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
    }

    private List<string> ReadInputFromTest() => Test.Split('\n').ToList();

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