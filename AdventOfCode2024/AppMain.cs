using AdventOfCode2024.Core;
using AdventOfCode2024.Day01;
using AdventOfCode2024.Day02;
using AdventOfCode2024.Day03;
using AdventOfCode2024.Day04;
using AdventOfCode2024.Day05;
using AdventOfCode2024.Day06;
using AdventOfCode2024.Day07;
using AdventOfCode2024.Day08;
using AdventOfCode2024.Day09;

var days = new DayX[]
{
    // new Day01(),
    // new Day02(),
    // new Day03(),
    // new Day04(),
    // new Day05(),
    // new Day06(),
    // new Day07(),
    // new Day08(),
    new Day09(),
};

foreach (var day in days)
{
    day.Part1(eInputMode.Test);
    day.Part2(eInputMode.Test);
}

foreach (var day in days)
{
    day.Part1(eInputMode.RealInput, measureTime: true);
    day.Part2(eInputMode.RealInput, measureTime: true);
}