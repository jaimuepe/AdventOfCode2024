using AdventOfCode2024.Core;

namespace AdventOfCode2024.Day08;

public class Day08 : DayX
{
    private record Node(string Id, string Left, string Right);

    protected override string Test =>
        "LLR\n\nAAA = (BBB, BBB)\nBBB = (AAA, ZZZ)\nZZZ = (ZZZ, ZZZ)";

    protected override void SolvePart1(List<string> lines)
    {
        var instructions = lines[0].ToCharArray();

        var nodes = new Dictionary<string, Node>();

        for (var i = 2; i < lines.Count; i++)
        {
            var newNode = ParseNode(lines[i]);
            nodes[newNode.Id] = newNode;
        }

        var steps = 0L;

        var node = nodes["AAA"];

        while (node.Id != "ZZZ")
        {
            var instruction = instructions[steps % instructions.Length];
            node = instruction == 'R' ? nodes[node.Right] : nodes[node.Left];
            steps++;
        }

        PrintResult(steps);
    }

    protected override void SolvePart2(List<string> lines)
    {
        var instructions = lines[0].ToCharArray();

        var startingNodes = new List<Node>();

        var nodes = new Dictionary<string, Node>();

        for (var i = 2; i < lines.Count; i++)
        {
            var newNode = ParseNode(lines[i]);
            nodes[newNode.Id] = newNode;

            if (newNode.Id[2] == 'A') startingNodes.Add(newNode);
        }

        var numberOfStartingNodes = startingNodes.Count;

        var repetitionPeriods = new long[numberOfStartingNodes];

        for (var i = 0; i < numberOfStartingNodes; i++)
        {
            var node = startingNodes[i];

            var steps = 0L;

            while (node.Id[2] != 'Z')
            {
                var instruction = instructions[steps % instructions.Length];
                node = instruction == 'R' ? nodes[node.Right] : nodes[node.Left];
                steps++;
            }

            repetitionPeriods[i] = steps;
        }

        var result = repetitionPeriods[0];

        for (var i = 1; i < repetitionPeriods.Length; i++)
        {
            result = lcm(result, repetitionPeriods[i]);
        }

        PrintResult(result);
    }

    private static Node ParseNode(string line)
    {
        var id = line[..3];
        var left = line[7..10];
        var right = line[12..15];

        return new Node(id, left, right);
    }

    private static long gcf(long a, long b)
    {
        while (b != 0)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }

    private static long lcm(long a, long b)
    {
        return (a / gcf(a, b)) * b;
    }
}