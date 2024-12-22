using AdventOfCode2024.Utilities;
using System.Runtime.InteropServices;

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

    private static long BuysAt(int[][] prices, int[][] changes, int[] changeSequence)
    {
        long sum = 0;
        for (int i = 0; i < prices.Length; i++)
        {
            var index = changes[i].Select((p, i) => (p, i)).Window(4).FirstOrDefault(w => w.Select(w => w.p).SequenceEqual(changeSequence))?[3].i;
            if (index.HasValue)
                sum += prices[i][index.Value];
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

        long bestSoFar = -1;
        HashSet<string> evaluated = [];
        int cacheHits = 0;
        for (int i = 3; i < priceChanges; i++)
        {
            for (int j = 0; j < monkeys.Count; j++)
            {
                //var num = prices[j][i];
                var changeSequence = changes[j][(i - 3)..(i + 1)];
                var seqStr = string.Join(',', changeSequence);
                if (evaluated.Contains(seqStr))
                    continue;

                evaluated.Add(seqStr);
                var bananas = BuysAt(prices, changes, changeSequence);

                if (bananas > bestSoFar)
                {
                    Console.WriteLine($"New best: {bananas} at iteration {i}, monkey {j}");
                    Console.WriteLine(string.Join(", ", changeSequence));
                    bestSoFar = bananas;
                    //bestSequence = changeSequence;
                }
            }
        }

        return $"{bestSoFar}";
    }

    // New best: 2306 at iteration 3, monkey 339
    // 0, 2, 0, 0
}