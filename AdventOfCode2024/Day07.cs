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

    private static readonly char[] operations = ['+', '*'];

    public string SolvePart1(string input)
    {
        long summation = 0;

        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < lines.Length; i++)
        {
            var tmp = lines[i].Split(": ");
            var target = long.Parse(tmp[0]);
            var values = tmp[1].Split(" ").Select(long.Parse).ToArray();
            
            foreach (var sequence in operations.Multichoose(values.Length - 1))
            {
                long acc = values[0];
                foreach (var (op, val) in sequence.Zip(values.Skip(1)))
                {
                    acc = op switch
                    {
                        '+' => acc + val,
                        '*' => acc * val,
                        _ => throw new InvalidOperationException()
                    };
                }

                if (acc == target)
                {
                    summation += target;
                    break;
                }
            }
        }

        return $"{summation}";
    }

    private static readonly string[] operations2 = ["+", "*", "||"];

    public string SolvePart2(string input)
    {
        long summation = 0;

        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < lines.Length; i++)
        {
            var tmp = lines[i].Split(": ");
            var target = long.Parse(tmp[0]);
            var values = tmp[1].Split(" ").Select(long.Parse).ToArray();

            foreach (var sequence in operations2.Multichoose(values.Length - 1))
            {
                long acc = values[0];
                foreach (var (op, val) in sequence.Zip(values.Skip(1)))
                {
                    acc = op switch
                    {
                        "+" => acc + val,
                        "*" => acc * val,
                        "||" => long.Parse($"{acc}{val}"),
                        _ => throw new InvalidOperationException()
                    };
                }

                if (acc == target)
                {
                    summation += target;
                    break;
                }
            }
        }

        return $"{summation}";
    }
}