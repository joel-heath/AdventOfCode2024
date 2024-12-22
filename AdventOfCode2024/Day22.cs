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

    private static long Evolve(long num)
    {
        num ^= num * 64;
        num %= 16777216;

        num ^= num / 32;
        num %= 16777216;

        num ^= num * 2048;
        num %= 16777216;

        return num;
    }

    private static long BuysAt(long[][] prices, Dictionary<string, int>[] changes, string changeSequence)
        => prices.Select((p, i) =>
            changes[i].TryGetValue(changeSequence, out int index)
                ? prices[i][index]
                : 0)
            .Sum();

    public string SolvePart1(string input)
        => $"{input.Split(Environment.NewLine)
            .Sum(l => Utils.Range(2000)
                .Aggregate(long.Parse(l), (num, _) => Evolve(num)))}";

    public string SolvePart2(string input)
    {
        const int priceChanges = 2000;

        long[] monkeys = input.Split(Environment.NewLine)
            .Select(long.Parse)
            .ToArray();

        var prices = Utils.NewJaggedArray<long>(monkeys.Length, priceChanges);
        var changes = Utils.NewJaggedArray<long>(monkeys.Length, priceChanges);

        for (int i = 0; i < priceChanges; i++)
        {
            for (int j = 0; j < monkeys.Length; j++)
            {
                long prev = monkeys[j];
                long num = Evolve(prev);

                monkeys[j] = num;
                prices[j][i] = num % 10;
                changes[j][i] = prices[j][i] - prev % 10;
            }
        }

        string[][] changeWindows = changes.Select(m => m.Window(4).Select(w => string.Join(",", w)).ToArray()).ToArray();
        Dictionary<string, int>[] windowLookup = changeWindows.Select(m => m.Select((w, i) => (w, i: i + 3)).DistinctBy(t => t.w).ToDictionary(t => t.w, t => t.i)).ToArray();

        return $"{changeWindows.SelectMany(m => m).Distinct().Max(w => BuysAt(prices, windowLookup, w))}";
    }
}