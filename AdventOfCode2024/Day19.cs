namespace AdventOfCode2024;

public class Day19 : IDay
{
    public int Day => 19;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "r, wr, b, g, bwu, rb, gb, br\r\n\r\nbrwrr\r\nbggr\r\ngbbr\r\nrrbgbr\r\nubwu\r\nbwurrg\r\nbrgr\r\nbbrgwb", "6" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "r, wr, b, g, bwu, rb, gb, br\r\n\r\nbrwrr\r\nbggr\r\ngbbr\r\nrrbgbr\r\nubwu\r\nbwurrg\r\nbrgr\r\nbbrgwb", "16" }
    };

    private static Dictionary<string, bool> memo = [];
    private static bool PatternPossible(string design, string[] towels)
    {
        if (memo.TryGetValue(design, out var possible)) return possible;

        foreach (var towel in towels)
        {
            if (towel.Length > design.Length)
                continue;
            if (design[..towel.Length] == towel)
            {
                if (towel.Length == design.Length || PatternPossible(design[towel.Length..], towels))
                {
                    memo[design] = true;
                    return true;
                }
            }
        }
        memo[design] = false;
        return false;
    }

    private static Dictionary<string, long> memo2 = [];

    private static long TowelDesignCount(string design, string[] towels)
    {
        if (memo2.TryGetValue(design, out var possible)) return possible;

        long sum = 0;
        foreach (var towel in towels)
        {
            if (towel.Length > design.Length)
                continue;
            if (design[..towel.Length] == towel)
            {
                if (towel.Length == design.Length)
                {
                    sum++;
                }
                else
                {
                    sum += TowelDesignCount(design[towel.Length..], towels);
                }
            }
        }

        memo2[design] = sum;
        return sum;
    }

    public string SolvePart1(string input)
    {
        long summation = 0;

        var sections = input.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var towels = sections[0].Split(", ", StringSplitOptions.RemoveEmptyEntries);
        var designs = sections[1].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        memo = [];
        for (int i = 0; i < designs.Length; i++)
        {
            var design = designs[i];

            if (PatternPossible(design, towels))
                summation++;
        }

        return $"{summation}";
    }

    public string SolvePart2(string input)
    {
        long summation = 0;

        var sections = input.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var towels = sections[0].Split(", ", StringSplitOptions.RemoveEmptyEntries);
        var designs = sections[1].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        memo2 = [];
        for (int i = 0; i < designs.Length; i++)
        {
            var design = designs[i];

            summation += TowelDesignCount(design, towels);
        }

        return $"{summation}";
    }
}