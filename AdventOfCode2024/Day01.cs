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
        var nums = input.Split(Environment.NewLine).Select(l => l.Split("   ").Select(int.Parse)).Transpose();

        return $"{nums[0].Order().Zip(nums[1].Order()).Sum(x => Math.Abs(x.First - x.Second))}";
    }

    public string SolvePart2(string input)
    {
        var nums = input.Split(Environment.NewLine).Select(l => l.Split("   ").Select(int.Parse)).Transpose();

        return $"{nums[0].Sum(l => l * nums[1].Count(r => r == l))}";
    }
}