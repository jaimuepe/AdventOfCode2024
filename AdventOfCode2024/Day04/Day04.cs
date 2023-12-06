using AdventOfCode2024.Core;

namespace AdventOfCode2024.Day04;

public class Day04 : DayX
{
    private class ScratchCard
    {
        public ScratchCard(List<int> winningNumbers, List<int> myNumbers)
        {
            WinningNumbers = winningNumbers;
            MyNumbers = myNumbers;
        }

        public List<int> WinningNumbers { get; }

        public List<int> MyNumbers { get; }
    }

    protected override string Test =>
        "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53\nCard 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19\nCard 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1\nCard 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83\nCard 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36\nCard 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11";

    protected override void SolvePart1(List<string> lines)
    {
        var scratchCards = ParseLines(lines);

        var result = 0;

        foreach (var scratchCard in scratchCards)
        {
            var matches = scratchCard.WinningNumbers
                .Count(winningNumber => scratchCard.MyNumbers.Contains(winningNumber));

            if (matches > 0) result += (int)Math.Pow(2, matches - 1);
        }

        PrintResult(result);
    }

    protected override void SolvePart2(List<string> lines)
    {
        var scratchCards = ParseLines(lines).ToArray();

        var copies = new int[scratchCards.Length];
        Array.Fill(copies, 1);

        for (var i = 0; i < scratchCards.Length; i++)
        {
            var scratchCard = scratchCards[i];

            var matches = scratchCard.WinningNumbers
                .Count(winningNumber => scratchCard.MyNumbers.Contains(winningNumber));

            for (var j = i + 1; j < i + 1 + matches && j < scratchCards.Length; j++)
            {
                copies[j] += copies[i];
            }
        }

        var result = copies.Sum();
        PrintResult(result);
    }

    private static IEnumerable<ScratchCard> ParseLines(List<string> lines)
    {
        foreach (var line in lines)
        {
            var colonIndex = line.IndexOf(":", StringComparison.Ordinal);

            var strAfterColon = line[(colonIndex + 2)..];

            var tokens = strAfterColon.Split(" | ");

            var winningNumbers = tokens[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            var myNumbers = tokens[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

            yield return new ScratchCard(winningNumbers, myNumbers);
        }
    }
}