using System.Text.RegularExpressions;

namespace AdventOfCode2024;

public partial class Day13 : IDay
{
    public int Day => 13;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "Button A: X+94, Y+34\r\nButton B: X+22, Y+67\r\nPrize: X=8400, Y=5400\r\n\r\nButton A: X+26, Y+66\r\nButton B: X+67, Y+21\r\nPrize: X=12748, Y=12176\r\n\r\nButton A: X+17, Y+86\r\nButton B: X+84, Y+37\r\nPrize: X=7870, Y=6450\r\n\r\nButton A: X+69, Y+23\r\nButton B: X+27, Y+71\r\nPrize: X=18641, Y=10279", "480" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "Button A: X+94, Y+34\r\nButton B: X+22, Y+67\r\nPrize: X=8400, Y=5400\r\n\r\nButton A: X+26, Y+66\r\nButton B: X+67, Y+21\r\nPrize: X=12748, Y=12176\r\n\r\nButton A: X+17, Y+86\r\nButton B: X+84, Y+37\r\nPrize: X=7870, Y=6450\r\n\r\nButton A: X+69, Y+23\r\nButton B: X+27, Y+71\r\nPrize: X=18641, Y=10279", "875318608908" }
    };

    [GeneratedRegex(@"\d+")]
    private static partial Regex Number();

    private static double Solve(string input, long prizeIncrement)
        => input.Split(Environment.NewLine + Environment.NewLine)
            .Select(g => g.Split(Environment.NewLine))
            .Select(g => (
                b: g[..^1]
                    .Select(b => Number().Matches(b)
                    .Select(m => double.Parse(m.Value)).ToArray())
                    .Select(m => (x: m[0], y: m[1]))
                    .ToList(),
                p: Number().Matches(g[^1]).Select(m => double.Parse(m.Value) + prizeIncrement).ToList()))
            .Select(g => (
                buttons: g.b,
                prize: g.p,
                detInverse: 1 / (g.b[0].x * g.b[1].y - g.b[1].x * g.b[0].y)))
            .Select(g => (g.buttons, g.prize,
                A: Math.Round((g.buttons[1].y * g.prize[0] - g.buttons[1].x * g.prize[1]) * g.detInverse),
                B: Math.Round((g.buttons[0].x * g.prize[1] - g.buttons[0].y * g.prize[0]) * g.detInverse)))
            .Sum(g => g.A * g.buttons[0].x + g.B * g.buttons[1].x == g.prize[0]
                   && g.A * g.buttons[0].y + g.B * g.buttons[1].y == g.prize[1]
                   ? 3 * g.A + g.B : 0);

    public string SolvePart1(string input)
        => $"{Solve(input, 0)}";

    public string SolvePart2(string input)
        => $"{Solve(input, 10_000_000_000_000)}";
}