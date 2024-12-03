using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public class Day03 : IDay
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
    {
        var matches = Regex.Matches(input, @"mul\((\d+),(\d+)\)").Select(m => m.Groups.Cast<Group>().Select(g => g.Captures.Select(c => int.Parse(c.Value))).Skip(1).SelectMany(e => e).ToArray());
        return $"{matches.Sum(m => m[0] * m[1])}";
    }

    public string SolvePart2(string input)
    {
        var muls = Regex.Matches(input, @"mul\((\d+),(\d+)\)").Select(m => m.Groups.Cast<Group>().Select(g => g.Captures.Select(c => (n: int.Parse(c.Value), i: c.Index))).Skip(1).SelectMany(e => e).ToArray());
        var dos = Regex.Matches(input, @"do(n't)?\(\)").Select(m => (b: m.Value == "do()", i: m.Index)).Reverse().ToArray();

        return $"{muls.Where(m => dos.FirstOrDefault(d => d.i < m[0].i, (b:true, i:-1)).b).Sum(m => m[0].n * m[1].n)}";
    }
}