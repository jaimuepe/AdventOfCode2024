using AdventOfCode2024.Core;

namespace AdventOfCode2024.Day09;

public class Day09 : DayX
{
    private class Sequence
    {
        public List<long> Values { get; }

        public Sequence? Previous { get; set; }

        public Sequence(List<long> values)
        {
            Values = values;
        }

        public override string ToString()
        {
            return string.Join(' ', Values);
        }
    }

    protected override string Test => "0 3 6 9 12 15\n1 3 6 10 15 21\n10 13 16 21 30 45";

    protected override void SolvePart1(List<string> lines)
    {
        var result = 0L;

        foreach (var seq in ParseSequences(lines))
        {
            var predictedValue = PredictNextValue(seq);
            result += predictedValue;
        }

        PrintResult(result);
    }

    protected override void SolvePart2(List<string> lines)
    {
        var result = 0L;

        foreach (var seq in ParseSequences(lines))
        {
            var predictedValue = PredictPreviousValue(seq);
            result += predictedValue;
        }

        PrintResult(result);
    }

    private static long PredictNextValue(Sequence seq)
    {
        seq = CreateDiffChain(seq);

        // add a zero to the last sequence
        seq.Values.Add(0L);

        while (seq.Previous != null)
        {
            var prev = seq.Previous;
            prev.Values.Add(prev.Values[^1] + seq.Values[^1]);
            seq = prev;
        }

        return seq.Values[^1];
    }

    private static long PredictPreviousValue(Sequence seq)
    {
        seq = CreateDiffChain(seq);

        // add a zero to the last sequence
        seq.Values.Insert(0, 0L);

        while (seq.Previous != null)
        {
            var prev = seq.Previous;
            prev.Values.Insert(0, prev.Values[0] - seq.Values[0]);
            seq = prev;
        }

        return seq.Values[0];
    }

    private static Sequence CreateDiffChain(Sequence seq)
    {
        while (seq.Values.Any(val => val != 0L))
        {
            var subSeq = CreateDiffSequence(seq);
            subSeq.Previous = seq;

            seq = subSeq;
        }

        return seq;
    }

    private static Sequence CreateDiffSequence(Sequence seq)
    {
        var subSeqValues = new List<long>(seq.Values.Count - 1);
        for (var i = 1; i < seq.Values.Count; i++)
        {
            subSeqValues.Add(seq.Values[i] - seq.Values[i - 1]);
        }

        return new Sequence(subSeqValues);
    }

    private static IEnumerable<Sequence> ParseSequences(List<string> lines)
    {
        foreach (var line in lines)
        {
            var values = line.Split(' ').Select(long.Parse).ToList();
            yield return new Sequence(values);
        }
    }
}