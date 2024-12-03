namespace AdventOfCode2024;

public class Day02 : IDay
{
    public int Day => 2;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "7 6 4 2 1\r\n1 2 7 8 9\r\n9 7 6 2 1\r\n1 3 2 4 5\r\n8 6 4 4 1\r\n1 3 6 7 9", "2" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "7 6 4 2 1\r\n1 2 7 8 9\r\n9 7 6 2 1\r\n1 3 2 4 5\r\n8 6 4 4 1\r\n1 3 6 7 9", "4" }
    };

    static bool IsValid(int[] report)
        => report.Skip(1).Aggregate(
                (prev: report[0], dir: report[1] > report[0] ? 1 : -1, valid: true),
                (acc, curr) => (curr, acc.dir, acc.valid && acc.dir * (curr - acc.prev) >= 1 && acc.dir * (curr - acc.prev) <= 3)
            ).valid;

    public string SolvePart1(string input)
        => $"{input.Split(Environment.NewLine)
            .Select(l => l.Split(' ').Select(int.Parse))
            .Count(r => IsValid([..r]))}";

    public string SolvePart2(string input)
        => $"{input.Split(Environment.NewLine)
            .Select(l => (int[])[..l.Split(' ').Select(int.Parse)])
            .Select(r => Enumerable.Range(0, r.Length).Select(n => (int[])[..r[..n], ..r[(n + 1)..]]))
            .Count(r => r.Any(IsValid))}";
}