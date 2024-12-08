using AdventOfCode2024.Utilities;

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

    private static readonly Dictionary<(string[] opset, int k), string[][]> memo = [];
    
    private static string[][] MemoisedMultichoose(string[] operators, int k)
    {
        lock (memo)
        {
            return memo.TryGetValue((operators, k), out string[][]? value) ? value
                : memo[(operators, k)] = operators.Multichoose(k).Select(x => x.ToArray()).ToArray();
        }
    }

    private static long Solver(string input, params string[] operators)
        => input.Split(Environment.NewLine)
            .Select(l => l.Split(' '))
            .Select(l => (testValue: long.Parse(l[0][..^1]), operands: l[1..].Select(long.Parse).ToList()))
            .AsParallel()
            .Sum(l => MemoisedMultichoose(operators, l.operands.Count - 1).Any(
                sequence => sequence.Zip(l.operands[1..]).AggregateWhile(l.operands[0], (acc, d) =>
                    d.First switch
                    {
                        "+" => acc + d.Second,
                        "*" => acc * d.Second,
                        "||" => acc * (long)Math.Pow(10, Math.Floor(Math.Log10(d.Second)) + 1) + d.Second,
                        _ => throw new InvalidOperationException()
                    }, acc => acc <= l.testValue) == l.testValue) ? l.testValue : 0);

    public string SolvePart1(string input)
        => $"{Solver(input, "+", "*")}";

    public string SolvePart2(string input)
        => $"{Solver(input, "+", "*", "||")}";
}