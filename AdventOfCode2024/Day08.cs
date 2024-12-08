using AdventOfCode2024.Utilities;

namespace AdventOfCode2024;

public class Day08 : IDay
{
    public int Day => 8;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "..........\r\n..........\r\n..........\r\n....a.....\r\n..........\r\n.....a....\r\n..........\r\n..........\r\n..........\r\n..........", "2" },
        { "..........\r\n..........\r\n..........\r\n....a.....\r\n........a.\r\n.....a....\r\n..........\r\n..........\r\n..........\r\n..........", "4" },
        { "............\r\n........0...\r\n.....0......\r\n.......0....\r\n....0.......\r\n......A.....\r\n............\r\n............\r\n........A...\r\n.........A..\r\n............\r\n............", "14" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "T.........\r\n...T......\r\n.T........\r\n..........\r\n..........\r\n..........\r\n..........\r\n..........\r\n..........\r\n..........", "9" },
        { "............\r\n........0...\r\n.....0......\r\n.......0....\r\n....0.......\r\n......A.....\r\n............\r\n............\r\n........A...\r\n.........A..\r\n............\r\n............", "34" }
    };

    public string SolvePart1(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var width = lines.Length;
        var height = lines[0].Length;

        var data = lines.SelectMany((line, i) => line.Select((c, j) => (c, j, i)).Where(d => d.c != '.')).GroupBy(d => d.c).ToDictionary(g => g.Key, g => g.Select(d => (d.j, d.i)).ToArray());


        HashSet<Point> uniques = [];
        foreach (var kvp in data)
        {
            var c = kvp.Key;
            var points = kvp.Value;

            int index = 0;
            foreach (var (j, i) in points)
            {
                Point here = (j, i);
                foreach (var p in points[index..].Where(x => x != (j, i))
                    .SelectMany(p => new List<Point> { p + (p - here), here - (p - here) })
                    .Where(p => 0 <= p.X && p.X < width && 0 <= p.Y && p.Y < height))
                    uniques.Add(p);   

                index++;
            }
        }

        return $"{uniques.Count}";
    }

    public string SolvePart2(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var width = lines.Length;
        var height = lines[0].Length;

        var data = lines.SelectMany((line, i) => line.Select((c, j) => (c, j, i)).Where(d => d.c != '.')).GroupBy(d => d.c).ToDictionary(g => g.Key, g => g.Select(d => (d.j, d.i)).ToArray());


        HashSet<Point> uniques = [];
        foreach (var kvp in data)
        {
            var c = kvp.Key;
            var points = kvp.Value;

            int index = 0;
            foreach (var (j, i) in points)
            {
                Point here = (j, i);
                foreach (var p in points[index..].Where(x => x != (j, i))
                    .Select(p => (a: p, d:p - here))
                    .SelectMany(l =>
                        Utils.EnumerateForever()
                            .Select(i => l.a + i*l.d)
                            .TakeWhile(p => 0 <= p.X && p.X < width && 0 <= p.Y && p.Y < height)
                    .Concat(
                        Utils.EnumerateForever()
                            .Select(i => here - i * l.d)
                            .TakeWhile(p => 0 <= p.X && p.X < width && 0 <= p.Y && p.Y < height)
                    )))
                    uniques.Add(p);

                index++;
            }
        }

        return $"{uniques.Count}";
    }
}