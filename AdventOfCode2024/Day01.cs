namespace AdventOfCode2024;

public class Day01 : IDay
{
    public int Day => 1;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "3   4\r\n4   3\r\n2   5\r\n1   3\r\n3   9\r\n3   3", "11" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "3   4\r\n4   3\r\n2   5\r\n1   3\r\n3   9\r\n3   3", "31" }
    };

    public string SolvePart1(string input)
    {
        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        List<int> left = [], right = [];

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

            left.Add(line[0]);
            right.Add(line[1]);
        }

        var lefts = left.OrderBy(t => t).ToList();
        var rights = right.OrderBy(t => t).ToList();

        return $"{lefts.Zip(rights).Select(x => Math.Abs(x.First - x.Second)).Sum()}";
    }

    public string SolvePart2(string input)
    {
        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        List<int> left = [], right = [];

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

            left.Add(line[0]);
            right.Add(line[1]);
        }

        var lefts = left.Sum(l => l * right.Count(r => r == l));

        return $"{lefts}";
    }
}