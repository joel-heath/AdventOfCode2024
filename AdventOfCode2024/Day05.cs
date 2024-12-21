using AdventOfCode2024.Utilities;

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

    private static bool ValidateOrder(Dictionary<long, HashSet<long>> orders, long[] line)
        => !line.Select(
                (curr, i) => line[..i].Select(
                    (pre, j) => (orders.TryGetValue(curr, out var successors) ? successors : []).Contains(pre) ? (line[i], line[j]) = (line[j], line[i]) : (-1, -1))
                .Any(i => i != (-1, -1)))
            .Any(b => b);

    private static (Dictionary<long, HashSet<long>> orderingRules, IEnumerable<long[]> updates) ParseInput(string input)
    {
        var sections = input.Split(Environment.NewLine + Environment.NewLine);
        var orderingRules = sections[0].Split(Environment.NewLine).Select(l => l.Split('|').Select(long.Parse).ToArray()).GroupBy(l => l[0]).ToDictionary(g => g.Key, g => g.Select(l => l[1]).ToHashSet());
        IEnumerable<long[]> updates = sections[1].Split(Environment.NewLine).Select(l => l.Split(',').Select(long.Parse).ToArray());

        return (orderingRules, updates);
    }

    public string SolvePart1(string input)
    {
        (var orders, var updates) = ParseInput(input);

        return $"{updates
            .Sum(l => l.Select((curr, i) =>
                    l[..i].Any(
                        pre => (orders.TryGetValue(curr, out var successors) ? successors : []).Contains(pre)))
                .Any(b => b) ? 0 : l[l.Length / 2])}";
    }

    public string SolvePart2(string input)
    {
        (var orders, var updates) = ParseInput(input);

        return $"{updates
            .Select((l, i) => Utils.EnumerateForever()
                .AggregateWhile((correctOrder: true, firstAttempt: true), (acc, _) => (acc.correctOrder = ValidateOrder(orders, l), acc.firstAttempt &= acc.correctOrder), acc => !acc.correctOrder)
                .firstAttempt ? 0 : l[l.Length / 2])
            .Sum()}";
    }
}