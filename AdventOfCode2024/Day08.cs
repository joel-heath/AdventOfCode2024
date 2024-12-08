using AdventOfCode2024.Utilities;

namespace AdventOfCode2024;

public class Day08 : IDay
{
    public int Day => 8;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "..........\r\n..........\r\n..........\r\n....a.....\r\n..........\r\n.....a....\r\n..........\r\n..........\r\n..........\r\n..........", "2" },
        { "..........\r\n..........\r\n..........\r\n....a.....\r\n........a.\r\n.....a....\r\n..........\r\n..........\r\n..........\r\n..........", "4" },
        { "..........\r\n..........\r\n..........\r\n....a.....\r\n........a.\r\n.....a....\r\n..........\r\n......A...\r\n..........\r\n..........", "4" },
        { "............\r\n........0...\r\n.....0......\r\n.......0....\r\n....0.......\r\n......A.....\r\n............\r\n............\r\n........A...\r\n.........A..\r\n............\r\n............", "14" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "T.........\r\n...T......\r\n.T........\r\n..........\r\n..........\r\n..........\r\n..........\r\n..........\r\n..........\r\n..........", "9" },
        { "............\r\n........0...\r\n.....0......\r\n.......0....\r\n....0.......\r\n......A.....\r\n............\r\n............\r\n........A...\r\n.........A..\r\n............\r\n............", "34" }
    };

    private static int Solver(string[] lines, Func<((int j, int i) a, Point d, Point here, int width, int height), IEnumerable<Point>> selector)
        => lines.SelectMany((line, i) => line.Select((c, j) => (c, j, i))
            .Where(d => d.c != '.'))
            .GroupBy(d => d.c)
            .ToDictionary(g => g.Key, g => g.Select(d => (d.j, d.i)).ToArray())
            .SelectMany(kvp =>
                kvp.Value.Select((p, i) => (here: (Point)p, i))
                    .SelectMany(d => kvp.Value[d.i..].Where(x => x != (d.here.X, d.here.Y))
                        .Select(p => (a: p, d: p - d.here, d.here, width: lines[0].Length, height:lines.Length)) // a,d are position and direction vector in r = a + i*d
                        .SelectMany(selector)))
            .ToHashSet().Count;

    public string SolvePart1(string input)
        => $"{Solver(input.Split(Environment.NewLine), d =>
            new List<Point> { d.a + d.d, d.here - d.d }
                .Where(p => 0 <= p.X && p.X < d.width && 0 <= p.Y && p.Y < d.height))}";

    public string SolvePart2(string input)
        => $"{Solver(input.Split(Environment.NewLine), l =>
            Utils.EnumerateForever()
                .Select(i => l.a + i * l.d)
                .TakeWhile(p => 0 <= p.X && p.X < l.width && 0 <= p.Y && p.Y < l.height)
            .Concat(
            Utils.EnumerateForever()
                .Select(i => l.here - i * l.d)
                .TakeWhile(p => 0 <= p.X && p.X < l.width && 0 <= p.Y && p.Y < l.height)))}";
}