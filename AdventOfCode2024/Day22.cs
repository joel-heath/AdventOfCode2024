using AdventOfCode2024.Utilities;

namespace AdventOfCode2024;

public class Day22 : IDay
{
    public int Day => 22;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "1\r\n10\r\n100\r\n2024", "37327623" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "1\r\n2\r\n3\r\n2024", "23" }
    };


    public string SolvePart1(string input)
    {
        long summation = 0;
        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < lines.Length; i++)
        {
            long num = long.Parse(lines[i]);

            for (int j = 0; j < 2000; j++)
            {
                num ^= num * 64;
                num %= 16777216;

                num ^= num / 32;
                num %= 16777216;

                num ^= num * 2048;
                num %= 16777216;
            }

            summation += num;
        }

        return $"{summation}";
    }

    private static long BuysAt(int[][] prices, Dictionary<string, int>[] changes, string changeSequence)
    {
        long sum = 0;
        for (int i = 0; i < prices.Length; i++)
        {
            if (changes[i].TryGetValue(changeSequence, out int index))
                sum += prices[i][index];
        }
        return sum;
    }

    public string SolvePart2(string input)
    {
        const int priceChanges = 2000;
        long summation = 0;

        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        List<long> monkeys = [];
        for (int i = 0; i < lines.Length; i++)
        {
            long num = long.Parse(lines[i]);
            monkeys.Add(num);
        }

        var prices = new int[monkeys.Count][];
        var changes = new int[monkeys.Count][];
        for (int i = 0; i < monkeys.Count; i++)
        {
            prices[i] = new int[priceChanges];
            changes[i] = new int[priceChanges];
        }

        for (int i = 0; i < priceChanges; i++)
        {
            for (int j = 0; j < monkeys.Count; j++)
            {
                long prev = monkeys[j];
                long num = prev;

                num ^= num * 64;
                num %= 16777216;

                num ^= num / 32;
                num %= 16777216;

                num ^= num * 2048;
                num %= 16777216;

                monkeys[j] = num;

                prices[j][i] = (int)(num % 10);
                changes[j][i] = prices[j][i] - (int)(prev % 10);
            }
        }

        string[][] changeWindows = changes.Select(m => m.Window(4).Select(w => string.Join(",", w)).ToArray()).ToArray();
        Dictionary<string, int>[] windowLookup = changeWindows.Select(m => m.Select((w, i) => (w, i: i + 3)).DistinctBy(t => t.w).ToDictionary(t => t.w, t => t.i)).ToArray();

        long bestSoFar = -1;
        HashSet<string> evaluated = [];
        int cacheHits = 0;
        for (int i = 3; i < priceChanges; i++)
        {
            for (int j = 0; j < monkeys.Count; j++)
            {
                var changeWindow = changeWindows[j][i - 3];
                if (evaluated.Contains(changeWindow))
                    continue;

                evaluated.Add(changeWindow);
                var bananas = BuysAt(prices, windowLookup, changeWindow);

                if (bananas > bestSoFar)
                {
                    //Console.WriteLine($"New best: {bananas} at iteration {i}, monkey {j}");
                    //Console.WriteLine(changeWindow);
                    bestSoFar = bananas;
                }
            }
        }

        return $"{bestSoFar}";
    }
}