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

    private static Dictionary<string, long> memo = [];

    private static long TowelDesignCount(string design, string[] towels)
        => memo.TryGetValue(design, out var possible)
            ? possible
            : memo[design] = towels
                .Where(t => t.Length <= design.Length)
                .Sum(towel =>
                    design[..towel.Length] == towel
                        ? towel.Length == design.Length
                            ? 1
                            : TowelDesignCount(design[towel.Length..], towels)
                        : 0);

    public string SolvePart1(string input)
    {
        var sections = input.Split(Environment.NewLine + Environment.NewLine);
        var towels = sections[0].Split(", ");
        var designs = sections[1].Split(Environment.NewLine);

        memo = [];
        return $"{designs.Count(d => TowelDesignCount(d, towels) > 0)}";
    }

    public string SolvePart2(string input)
    {
        var sections = input.Split(Environment.NewLine + Environment.NewLine);
        var towels = sections[0].Split(", ");
        var designs = sections[1].Split(Environment.NewLine);

        memo = [];
        return $"{designs.Sum(d => TowelDesignCount(d, towels))}";
    }
}