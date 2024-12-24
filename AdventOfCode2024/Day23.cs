using AdventOfCode2024.Utilities;

namespace AdventOfCode2024;

public class Day23 : IDay
{
    public int Day => 23;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "kh-tc\r\nqp-kh\r\nde-cg\r\nka-co\r\nyn-aq\r\nqp-ub\r\ncg-tb\r\nvc-aq\r\ntb-ka\r\nwh-tc\r\nyn-cg\r\nkh-ub\r\nta-co\r\nde-co\r\ntc-td\r\ntb-wq\r\nwh-td\r\nta-ka\r\ntd-qp\r\naq-cg\r\nwq-ub\r\nub-vc\r\nde-ta\r\nwq-aq\r\nwq-vc\r\nwh-yn\r\nka-de\r\nkh-ta\r\nco-tc\r\nwh-qp\r\ntb-vc\r\ntd-yn", "7" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "kh-tc\r\nqp-kh\r\nde-cg\r\nka-co\r\nyn-aq\r\nqp-ub\r\ncg-tb\r\nvc-aq\r\ntb-ka\r\nwh-tc\r\nyn-cg\r\nkh-ub\r\nta-co\r\nde-co\r\ntc-td\r\ntb-wq\r\nwh-td\r\nta-ka\r\ntd-qp\r\naq-cg\r\nwq-ub\r\nub-vc\r\nde-ta\r\nwq-aq\r\nwq-vc\r\nwh-yn\r\nka-de\r\nkh-ta\r\nco-tc\r\nwh-qp\r\ntb-vc\r\ntd-yn", "co,de,ka,ta" }
    };

    private static IEnumerable<HashSet<string>> Find3Cycles(Dictionary<string, HashSet<string>> network, string current, string start, HashSet<string> visited)
    {
        if (visited.Count >= 3)
        {
            if (start == current)
                yield return visited;

            yield break;
        }

        foreach (var next in network[current].Where(d => d != current && !visited.Contains(d) && visited.All(v => network[v].Contains(d))))
        {
            foreach (var @return in Find3Cycles(network, next, start, [.. visited, next]))
                yield return @return;
        }
    }

    private static List<string> ExpandClique(Dictionary<string, HashSet<string>> network, List<string> cycle)
    {
        var candidates = network[cycle.First()].Where(destination => !cycle.Contains(destination) && cycle.Skip(1).All(v => network[v].Contains(destination)));

        int i = 0;
        foreach (var candidate in candidates)
        {
            var connections = network[candidate];
            if (i == 0 || cycle[(i + 3)..].All(connections.Contains))
            {
                cycle.Add(candidate);
                i++;
            }
        }

        return cycle;
    }

    public string SolvePart1(string input)
    {
        var network = input.Split(Environment.NewLine)
            .Select(line => line.Split('-'))
            .SelectMany(l => new[] { l, l.Reverse().ToArray() })
            .GroupBy(l => l[0])
            .ToDictionary(l => l.Key, l => l.SelectMany(g => g).Skip(1).ToHashSet());

        return $"{network.Keys
            .SelectMany(n => Find3Cycles(network, n, n, []))
            .Where(l => l.Any(c => c[0] == 't'))
            .Count() / Combinatorics.Factorial(3)}";
    }

    public string SolvePart2(string input)
    {
        var network = input.Split(Environment.NewLine)
            .Select(line => line.Split('-'))
            .SelectMany(l => new[] { l, l.Reverse().ToArray() })
            .GroupBy(l => l[0])
            .ToDictionary(l => l.Key, l => l.SelectMany(g => g).Skip(1).ToHashSet());

        return $"{string.Join(',', (network.Keys
            .SelectMany(n => Find3Cycles(network, n, n, []))
            .DistinctBy(l => string.Join(',', l.Order()))

            .Select(t => ExpandClique(network, [.. t]))
            .MaxBy(t => t.Count) ?? [])
            .Distinct()
            .Order())}";
    }
}