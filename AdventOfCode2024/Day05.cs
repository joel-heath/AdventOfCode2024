namespace AdventOfCode2024;

public class Day05 : IDay
{
    public int Day => 5;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "47|53\r\n97|13\r\n97|61\r\n97|47\r\n75|29\r\n61|13\r\n75|53\r\n29|13\r\n97|29\r\n53|29\r\n61|53\r\n97|53\r\n61|29\r\n47|13\r\n75|47\r\n97|75\r\n47|61\r\n75|61\r\n47|29\r\n75|13\r\n53|13\r\n\r\n75,47,61,53,29\r\n97,61,53,29,13\r\n75,29,13\r\n75,97,47,61,53\r\n61,13,29\r\n97,13,75,29,47", "143" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "47|53\r\n97|13\r\n97|61\r\n97|47\r\n75|29\r\n61|13\r\n75|53\r\n29|13\r\n97|29\r\n53|29\r\n61|53\r\n97|53\r\n61|29\r\n47|13\r\n75|47\r\n97|75\r\n47|61\r\n75|61\r\n47|29\r\n75|13\r\n53|13\r\n\r\n75,47,61,53,29\r\n97,61,53,29,13\r\n75,29,13\r\n75,97,47,61,53\r\n61,13,29\r\n97,13,75,29,47", "123" }
    };

    public string SolvePart1(string input)
    {
        long sum = 0;

        var sections = input.Split(Environment.NewLine + Environment.NewLine);
        var orders = sections[0].Split(Environment.NewLine).Select(l => l.Split('|').Select(long.Parse).ToArray()).GroupBy(x => x[0]).ToDictionary(g => g.Key, g => g.Select(a => a[1]).ToArray());

        var lines = sections[1].Split(Environment.NewLine);
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Split(',').Select(long.Parse).ToArray();

            bool correctOrder = true;
            for (int j = 0; j < line.Length; j++)
            {
                var num = line[j];
                if (!orders.TryGetValue(num, out long[]? proceedings))
                    proceedings = [];
                if (line[..j].Any(n => proceedings.Contains(n)))
                {
                    correctOrder = false;
                    break;
                }
            }

            if (correctOrder)
                sum += line[line.Length / 2];
        }

        return $"{sum}";
    }

    public string SolvePart2(string input)
    {
        long sum = 0;

        var sections = input.Split(Environment.NewLine + Environment.NewLine);
        var orders = sections[0].Split(Environment.NewLine).Select(l => l.Split('|').Select(long.Parse).ToArray()).GroupBy(x => x[0]).ToDictionary(g => g.Key, g => g.Select(a => a[1]).ToArray());

        var lines = sections[1].Split(Environment.NewLine).Select(l => l.Split(',').Select(long.Parse).ToArray()).ToArray();
        HashSet<int> incorrectlyOrdered = [];
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];

            bool correctOrder = true;
            for (int j = 0; j < line.Length; j++)
            {
                var num = line[j];
                if (!orders.TryGetValue(num, out long[]? proceedings))
                    proceedings = [];

                for (int k = 0; k < j; k++)
                {
                    if (proceedings.Contains(line[k]))
                    {
                        correctOrder = false;
                        (lines[i][j], lines[i][k]) = (line[k], line[j]);
                        incorrectlyOrdered.Add(i);
                        i--; // Retry this line
                        break;
                    }
                }
                if (!correctOrder)
                    break;
            }
            
            // we've now corrected the order
            if (correctOrder && incorrectlyOrdered.Contains(i))
                sum += lines[i][line.Length / 2];
        }

        return $"{sum}";
    }
}