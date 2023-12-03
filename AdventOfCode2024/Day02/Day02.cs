using AdventOfCode2024.Core;

namespace AdventOfCode2024.Day02;

// --- DISCLAIMER ---
// the solutions for this day could be easily achieved with basic nested loops but wanted to try with linq queries as 
// a personal fun challenge. The resulting code is far less readable but ¯\_(ツ)_/¯

public class Day02 : DayX
{
    private class Game
    {
        public int Id { get; init; }
        public List<SubSet> SubSets { get; } = new();
    }

    private class SubSet
    {
        public Dictionary<string, int> CubesByColor { get; } = new()
        {
            ["red"] = 0,
            ["green"] = 0,
            ["blue"] = 0,
        };
    }

    protected override string Test =>
        "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green\nGame 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue\nGame 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red\nGame 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red\nGame 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green";

    protected override void SolvePart1(List<string> lines)
    {
        var availableCubes = new Dictionary<string, int>
        {
            ["red"] = 12,
            ["green"] = 13,
            ["blue"] = 14,
        };

        var games = ParseGames(lines);

        var result = games
            .Where(game => game.SubSets
                .All(set => set.CubesByColor.Keys
                    .All(color => availableCubes[color] >= set.CubesByColor[color])))
            .Select(game => game.Id)
            .Sum();

        PrintResult(result);
    }

    protected override void SolvePart2(List<string> lines)
    {
        var games = ParseGames(lines);

        var result = games
            .Sum(game => game.SubSets
                .SelectMany(set => set.CubesByColor)
                .GroupBy(kvp => kvp.Key, kvp => kvp.Value)
                .Select(group => group.Max())
                .Aggregate((a, x) => a * x));

        PrintResult(result);
    }

    private static IEnumerable<Game> ParseGames(IReadOnlyList<string> lines)
    {
        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];

            var game = new Game
            {
                Id = i + 1
            };

            var colonIndex = line.IndexOf(':');
            var setTokens = line[(colonIndex + 2)..].Split("; ");

            foreach (var setToken in setTokens)
            {
                var set = new SubSet();

                var colorTokens = setToken.Split(", ");

                foreach (var colorToken in colorTokens)
                {
                    var numberColorTokens = colorToken.Split(" ");

                    var number = int.Parse(numberColorTokens[0]);
                    var color = numberColorTokens[1];

                    set.CubesByColor[color] = number;
                }

                game.SubSets.Add(set);
            }

            yield return game;
        }
    }
}