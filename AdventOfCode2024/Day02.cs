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

    public string SolvePart1(string input)
    {
        long count = 0;
        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Split(' ').Select(long.Parse).ToArray();

            if (IsValid(line))
            {
                count++;
            }
        }

        return $"{count}";
    }

    static bool IsValid(long[] line)
    {
        var prev = line[0];
        bool inc = true, dec = true;
        for (int j = 1; j < line.Length; j++)
        {
            if (line[j] <= prev || line[j] - prev > 3)
            {
                inc = false;
            }
            if (line[j] >= prev || prev - line[j] > 3)
            {
                dec = false;
            }
            prev = line[j];
        }

        return inc || dec;
    }

    public string SolvePart2(string input)
    {
        long count = 0;
        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Split(' ').Select(long.Parse).ToArray();
            bool valid = IsValid(line);

            for (int j = 0; !valid && j < line.Length; j++)
            {
                if (j == 0)
                {
                    valid = IsValid(line[1..]);
                }
                else if (j == lines.Length - 1)
                {
                    valid = IsValid(line[..^1]);
                }
                else
                {
                    valid = IsValid([..line[..j], ..line[(j + 1)..]]);
                }                
            }

            if (valid)
            {
                count++;
            }
        }

        return $"{count}";
    }
}