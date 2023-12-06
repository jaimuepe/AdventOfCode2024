using System.Text.RegularExpressions;
using AdventOfCode2024.Core;

namespace AdventOfCode2024.Day06;

public class Day06 : DayX
{
    private class Race
    {
        public long Duration { get; init; }

        public long RecordDistance { get; init; }
    }

    protected override string Test => "Time:      7  15   30\nDistance:  9  40  200";

    protected override void SolvePart1(List<string> lines)
    {
        var races = ParseRaces(lines, true);

        var result = 1L;

        foreach (var race in races)
        {
            result *= GetNumberOfWaysToBeat(race);
        }

        PrintResult(result);
    }

    protected override void SolvePart2(List<string> lines)
    {
        var race = ParseRaces(lines, false).First();

        var waysToWin = GetNumberOfWaysToBeat(race);
        PrintResult(waysToWin);
    }

    private static long GetNumberOfWaysToBeat(Race race)
    {
        // speed * leftTime > record
        // leftTime = duration - pressedTime
        // pressedTime = x

        // speed * (duration - x) > record

        // since the speed matches the pressed time:
        // x * (duration - x) > record
        // -x * x + x * duration - record > 0

        // we can find roots for the quadratic equation
        // a * x * x + b * x + c == 0

        var duration = race.Duration;
        var distance = race.RecordDistance;

        var a = -1;
        var b = duration;
        // since we dont want to match the record but beat it, we add a little extra to it
        var c = -(distance + 0.1);

        // roots -> (-b +- sqrt(b * b - 4 * a * c)) / (2 * a)

        var sqrt = Math.Sqrt(b * b - 4 * a * c);
        var div = 1.0 / (2.0 * a);

        var min = (long)Math.Ceiling((-b + sqrt) * div);
        var max = (long)Math.Floor((-b - sqrt) * div);

        var waysToWin = max - min + 1;
        return waysToWin;
    }

    private static readonly Regex WhitespaceRegex = new(@"\s+");

    private static IEnumerable<Race> ParseRaces(IReadOnlyList<string> lines, bool isPartA)
    {
        var timeSubstr = lines[0][6..];
        var distanceSubstr = lines[1][9..];

        if (isPartA)
        {
            var timeTokens = timeSubstr
                .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            var distanceTokens = distanceSubstr
                .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            return timeTokens
                .Select((time, i) => new Race
                {
                    Duration = long.Parse(time),
                    RecordDistance = long.Parse(distanceTokens[i]),
                })
                .ToList();
        }
        else
        {
            var duration = long.Parse(WhitespaceRegex.Replace(timeSubstr, ""));
            var distance = long.Parse(WhitespaceRegex.Replace(distanceSubstr, ""));

            return new[]
            {
                new Race
                {
                    Duration = duration,
                    RecordDistance = distance,
                }
            };
        }
    }
}