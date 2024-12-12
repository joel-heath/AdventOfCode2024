using AdventOfCode2024.Utilities;

namespace AdventOfCode2024;

public class Day11 : IDay
{
    public int Day => 11;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "125 17", "55312" },
        { "0 1 10 99 999", "125681" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "125 17", "65601038650482" },
        { "0 1 10 99 999", "149161030616311" }
    };

    private static Dictionary<(long stone, int i), long> memo = [];

    private static long Blink(long stone, int i)
        => i == 0 ? 1
            : memo.ContainsKey((stone, i))
            ? memo[(stone, i)]
            : memo[(stone, i)] = stone == 0
                ? Blink(1, i - 1)
                : stone.ToString().Assign(out var str).Length % 2 == 0
                ? new string[] { str[..(str.Length / 2)], str[(str.Length / 2)..] }.Select(long.Parse).Select(s => Blink(s, i - 1)).Sum()
                : Blink(stone * 2024, i - 1);

    private static string Solve(string input, int blinks)
    {
        memo = [];
        return $"{input.Split(' ')
            .Select(long.Parse)
            .Select(stone => Blink(stone, blinks))
            .Sum()}";
    }

    public string SolvePart1(string input)
        => Solve(input, 25);

    public string SolvePart2(string input)
        => Solve(input, 75);
}