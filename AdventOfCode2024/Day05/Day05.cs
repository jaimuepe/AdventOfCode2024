using AdventOfCode2024.Core;

namespace AdventOfCode2024.Day05;

public class Day05 : DayX
{
    private class Range : IComparable<Range>
    {
        public long SourceIndex { get; }

        public long DestinationIndex { get; }

        public long Length { get; }

        public Range(long sourceIndex, long destinationIndex, long length)
        {
            SourceIndex = sourceIndex;
            DestinationIndex = destinationIndex;
            Length = length;
        }

        public int CompareTo(Range? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;

            return SourceIndex.CompareTo(other.SourceIndex);
        }

        public override string ToString()
        {
            return $"{SourceIndex}-{SourceIndex + Length}";
        }
    }

    private class OffsetRange
    {
        public Range Range { get; }

        public long Offset { get; }

        public long Length { get; }

        public OffsetRange(Range range, long offset, long length)
        {
            Range = range;
            Offset = offset;
            Length = length;
        }

        public override string ToString()
        {
            return
                $"src: {Range.SourceIndex + Offset}-{Range.SourceIndex + Offset + Length}, dst: {Range.DestinationIndex + Offset}-{Range.DestinationIndex + Offset + Length}";
        }
    }

    private class Almanac
    {
        public static readonly string[] ConversionTables =
        {
            "seed-to-soil",
            "soil-to-fertilizer",
            "fertilizer-to-water",
            "water-to-light",
            "light-to-temperature",
            "temperature-to-humidity",
            "humidity-to-location"
        };

        public List<Range> Seeds { get; }

        public Dictionary<string, List<Range>> AllRanges { get; } = new();

        public Almanac(List<Range> seeds)
        {
            Seeds = seeds;

            foreach (var table in ConversionTables)
            {
                AllRanges[table] = new List<Range>();
            }
        }
    }

    protected override string Test =>
        "seeds: 79 14 55 13\n\nseed-to-soil map:\n50 98 2\n52 50 48\n\nsoil-to-fertilizer map:\n0 15 37\n37 52 2\n39 0 15\n\nfertilizer-to-water map:\n49 53 8\n0 11 42\n42 0 7\n57 7 4\n\nwater-to-light map:\n88 18 7\n18 25 70\n\nlight-to-temperature map:\n45 77 23\n81 45 19\n68 64 13\n\ntemperature-to-humidity map:\n0 69 1\n1 0 69\n\nhumidity-to-location map:\n60 56 37\n56 93 4";

    protected override void SolvePart1(List<string> lines)
    {
        var almanac = ParseAlmanac(lines, isPartB: false);

        var result = long.MaxValue;
        
        foreach (var seed in almanac.Seeds)
        {
            result = long.Min(result,
                GetMinimumDestinationValueForSeedRange(seed.SourceIndex, seed.Length, almanac));
        }

        PrintResult(result);
    }

    protected override void SolvePart2(List<string> lines)
    {
        var almanac = ParseAlmanac(lines, isPartB: true);

        var result = long.MaxValue;

        foreach (var seed in almanac.Seeds)
        {
            result = long.Min(result,
                GetMinimumDestinationValueForSeedRange(seed.SourceIndex, seed.Length, almanac));
        }

        PrintResult(result);
    }

    private static long GetMinimumDestinationValueForSeedRange(long firstIndex, long length, Almanac almanac)
    {
        return GetMinimumDestinationValue(GetLocationRangesForSeedRange(firstIndex, length, almanac));
    }

    private static long GetMinimumDestinationValue(IEnumerable<OffsetRange> ranges)
    {
        return ranges.Min(offset => offset.Range.DestinationIndex + offset.Offset);
    }

    private static IEnumerable<OffsetRange> GetLocationRangesForSeedRange(long from, long length, Almanac almanac)
    {
        IEnumerable<OffsetRange>? ranges = null;

        foreach (var table in Almanac.ConversionTables)
        {
            // first iteration
            if (ranges == null)
            {
                ranges = GetOffsetRanges(from, length, almanac, table);
            }
            else
            {
                ranges = ranges
                    .SelectMany(offset => GetOffsetRanges(
                        offset.Range.DestinationIndex + offset.Offset,
                        offset.Length,
                        almanac,
                        table));
            }
        }

        return ranges!;
    }

    private static IEnumerable<OffsetRange> GetOffsetRanges(
        long firstSourceIndex,
        long sourceLength,
        Almanac almanac,
        string rangeId)
    {
        var offsetRanges = new List<OffsetRange>();

        var ranges = almanac.AllRanges[rangeId];

        var lastSourceIndex = firstSourceIndex + sourceLength;

        var i = firstSourceIndex;
        while (i < lastSourceIndex)
        {
            var rangeIndex = -1;

            // find range for this index
            for (int j = 0; j < ranges.Count; j++)
            {
                var range = ranges[j];
                if (i >= range.SourceIndex && i < range.SourceIndex + range.Length)
                {
                    rangeIndex = j;
                    break;
                }
            }

            if (rangeIndex == -1)
            {
                // find the next range
                var range = ranges.FirstOrDefault(range => range.SourceIndex > i);

                if (range == null)
                {
                    // ok, all values are outside the range
                    range = new Range(firstSourceIndex, firstSourceIndex, sourceLength);
                    offsetRanges.Add(new OffsetRange(range, 0, sourceLength));

                    break;
                }
                else
                {
                    // some values are outside the range, others not
                    var length = long.Min(lastSourceIndex - i, range.SourceIndex);
                    var fakeRange = new Range(firstSourceIndex, firstSourceIndex, length);
                    offsetRanges.Add(new OffsetRange(fakeRange, 0, length));

                    i += length;
                }
            }
            else
            {
                var range = ranges[rangeIndex];

                var sourceOffset = i - range.SourceIndex;

                // how many values do we take from this range?
                var length = long.Min(lastSourceIndex - i, range.SourceIndex + range.Length - i);

                offsetRanges.Add(new OffsetRange(range, sourceOffset, length));

                i += length;
            }
        }

        return offsetRanges;
    }

    private static Almanac ParseAlmanac(IReadOnlyList<string> lines, bool isPartB)
    {
        var seedsLine = lines[0];
        var seedTokens = seedsLine[7..].Split(' ').Select(long.Parse).ToArray();

        var seeds = new List<Range>();

        if (isPartB)
        {
            for (var x = 0; x < seedTokens.Length; x += 2)
            {
                var firstSeedIndex = seedTokens[x];
                var length = seedTokens[x + 1];

                seeds.Add(new Range(firstSeedIndex, firstSeedIndex, length));
            }
        }
        else
        {
            seeds.AddRange(seedTokens.Select(seedToken => new Range(seedToken, seedToken, 1)));
        }

        var almanac = new Almanac(seeds);

        var i = 2;
        while (i < lines.Count)
        {
            var mapLine = lines[i];
            var mapId = mapLine[..^5];

            var startIndex = i + 1;
            var endIndex = startIndex;

            while (endIndex < lines.Count && lines[endIndex] != "") endIndex++;

            for (var j = startIndex; j < endIndex; j++)
            {
                var line = lines[j];
                var values = line.Split(" ").Select(long.Parse).ToArray();

                var destinationIndex = values[0];
                var sourceIndex = values[1];
                var length = values[2];

                almanac.AllRanges[mapId].Add(
                    new Range(sourceIndex, destinationIndex, length));
            }

            foreach (var list in almanac.AllRanges.Values)
            {
                list.Sort();
            }

            i = endIndex + 1;
        }

        return almanac;
    }
}