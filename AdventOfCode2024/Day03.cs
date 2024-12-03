using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public partial class Day03 : IDay
{
    public int Day => 3;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))", "161" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))", "48" }
    };

    public string SolvePart1(string input)
        => $"{Multiply().Matches(input)
            .Select(m => m.Groups.Cast<Group>()
                .Select(g => int.Parse(g.Value))
                .Skip(1)
                .Aggregate((a, n) => a * n)
            ).Sum()}";

    public string SolvePart2(string input)
    {
        var muls = Multiply().Matches(input).Select(m => m.Groups.Cast<Group>().Select(g => (n: int.Parse(g.Value), i: g.Index)).Skip(1).ToArray());
        var dos = DoDonts().Matches(input).Select(m => (b: m.Value == "do()", i: m.Index)).Reverse().ToArray();

        return $"{muls.Where(m => dos.FirstOrDefault(d => d.i < m[0].i, (b: true, i: -1)).b).Sum(m => m[0].n * m[1].n)}";
    }

    [GeneratedRegex(@"mul\((\d+),(\d+)\)")]
    private static partial Regex Multiply();

    [GeneratedRegex(@"do(n't)?\(\)")]
    private static partial Regex DoDonts();
}