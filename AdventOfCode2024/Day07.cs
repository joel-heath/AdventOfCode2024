using AdventOfCode2024.Utilities;
using System.Windows.Markup;

namespace AdventOfCode2024;

public class Day07 : IDay
{
    public int Day => 7;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "190: 10 19\r\n3267: 81 40 27\r\n83: 17 5\r\n156: 15 6\r\n7290: 6 8 6 15\r\n161011: 16 10 13\r\n192: 17 8 14\r\n21037: 9 7 18 13\r\n292: 11 6 16 20", "3749" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "190: 10 19\r\n3267: 81 40 27\r\n83: 17 5\r\n156: 15 6\r\n7290: 6 8 6 15\r\n161011: 16 10 13\r\n192: 17 8 14\r\n21037: 9 7 18 13\r\n292: 11 6 16 20", "11387" }
    };

    private static readonly string[] operators = ["+", "*", "||"];

    public static long Solver(string input, string[] operators)
        => input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Split(' '))
            .Select(l => (testValue: long.Parse(l[0][..^1]), operands: l[1..].Select(long.Parse).ToList()))
            .AsParallel()
            .Sum(l => operators.Multichoose(l.operands.Count - 1).Any(
                sequence => sequence.Zip(l.operands[1..]).AggregateWhile(l.operands[0], (acc, d) =>
                    d.First switch
                    {
                        "+" => acc + d.Second,
                        "*" => acc * d.Second,
                        "||" => long.Parse($"{acc}{d.Second}"),
                        _ => throw new InvalidOperationException()
                    }, acc => acc <= l.testValue) == l.testValue) ? l.testValue : 0);

    public string SolvePart1(string input)
        => $"{Solver(input, operators[..^1])}";

    public string SolvePart2(string input)
        => $"{Solver(input, operators)}";
}