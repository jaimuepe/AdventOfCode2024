using AdventOfCode2024.Core;

namespace AdventOfCode2024.Day03;

public class Day03 : DayX
{
    private class Schematic
    {
        public char[][] Grid { get; }

        public Schematic(IReadOnlyList<string> lines)
        {
            var w = lines[0].Length;
            var h = lines.Count;

            Grid = new char[h][];

            for (var i = 0; i < h; i++)
            {
                Grid[i] = new char[w];

                for (var j = 0; j < w; j++)
                {
                    Grid[i][j] = lines[i][j];
                }
            }
        }

        public IEnumerable<Cell> GetNeighbors(int fromX, int toX, int y)
        {
            for (var i = y - 1; i <= y + 1; i++)
            {
                if (i < 0 || i >= Grid.Length) continue;

                for (var j = fromX - 1; j <= toX + 1; j++)
                {
                    if (j < 0 || j >= Grid[i].Length) continue;
                    if (i == y && (fromX == toX && j == fromX || j >= fromX && j <= toX)) continue;

                    yield return new Cell()
                    {
                        X = j,
                        Y = i,
                        Value = Grid[i][j],
                    };
                }
            }
        }
    }

    private class Cell
    {
        public int X { get; init; }

        public int Y { get; init; }

        public char Value { get; init; }

        public override bool Equals(object? obj)
        {
            if (obj is not Cell otherCell) return false;
            return Equals(otherCell);
        }

        protected bool Equals(Cell other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }

    protected override string Test =>
        "467..114..\n...*......\n..35..633.\n......#...\n617*......\n.....+.58.\n..592.....\n......755.\n...$.*....\n.664.598..";

    protected override void SolvePart1(List<string> lines)
    {
        var schematic = new Schematic(lines);
        var grid = schematic.Grid;

        var result = 0;

        for (var i = 0; i < grid.Length; i++)
        {
            var j = 0;
            while (j < grid[i].Length)
            {
                if (!IsDigit(grid[i][j]))
                {
                    j++;
                    continue;
                }

                var k = j;

                while (k < grid[i].Length && IsDigit(grid[i][k])) k++;

                var neighbors = schematic.GetNeighbors(j, k - 1, i);

                var isAdjacentToSymbol = neighbors
                    .Any(cell => IsSymbol(cell.Value));

                if (isAdjacentToSymbol)
                {
                    var number = int.Parse(grid[i].AsSpan()[j..k]);
                    result += number;
                }

                j = k;
            }
        }

        PrintResult(result);
    }

    protected override void SolvePart2(List<string> lines)
    {
        var schematic = new Schematic(lines);
        var grid = schematic.Grid;

        var gearNeighbors = new Dictionary<Cell, List<int>>();

        for (var i = 0; i < grid.Length; i++)
        {
            var j = 0;
            while (j < grid[i].Length)
            {
                if (!IsDigit(grid[i][j]))
                {
                    j++;
                    continue;
                }

                var k = j;

                while (k < grid[i].Length && IsDigit(grid[i][k])) k++;

                var neighborGearCells = schematic
                    .GetNeighbors(j, k - 1, i)
                    .Where(cell => cell.Value == '*')
                    .ToArray();

                if (neighborGearCells.Length > 0)
                {
                    var number = int.Parse(grid[i].AsSpan()[j..k]);

                    foreach (var cell in neighborGearCells)
                    {
                        gearNeighbors.TryAdd(cell, new List<int>());
                        gearNeighbors[cell].Add(number);
                    }
                }

                j = k;
            }
        }

        var result = gearNeighbors.Values
            .Where(list => list.Count == 2)
            .Sum(list => list[0] * list[1]);

        PrintResult(result);
    }

    private static bool IsSymbol(char c) => !IsBlank(c) && !IsDigit(c);

    private static bool IsBlank(char c) => c == '.';

    private static bool IsDigit(char c)
    {
        return c is >= '0' and <= '9';
    }
}