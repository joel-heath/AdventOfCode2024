using System.Drawing;

namespace AdventOfCode2024;

public class Day06 : IDay
{
    public int Day => 6;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "....#.....\r\n.........#\r\n..........\r\n..#.......\r\n.......#..\r\n..........\r\n.#..^.....\r\n........#.\r\n#.........\r\n......#...", "41" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "....#.....\r\n.........#\r\n..........\r\n..#.......\r\n.......#..\r\n..........\r\n.#..^.....\r\n........#.\r\n#.........\r\n......#...", "6" }
    };

    static readonly (int x, int y)[] vectors = [(0, -1), (1, 0), (0, 1), (-1, 0)];

    static List<(int x, int y)> Explore(string[] lines, (int x, int y, int d) start)
    {
        var visited = new HashSet<(int x, int y, int d)>();
        var (x, y, d) = start;
        var (width, height) = (lines[0].Length, lines.Length);

        while (x != 0 && y != 0 && x != width - 1 && y != height - 1 && !visited.Contains((x, y, d)))
        {
            visited.Add((x, y, d));
            var (newX, newY) = (x + vectors[d].x, y + vectors[d].y);
            if (lines[newY][newX] == '#') d = (d + 1) % 4;
            else (x, y) = (newX, newY);
        }

        return visited.Append((x, y, d)).Select(p => (p.x, p.y)).Distinct().ToList();
    }

    public string SolvePart1(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var index = input.IndexOf('^');
        var len = lines[0].Length + Environment.NewLine.Length;
        var start = (index % len, index / len, 0);

        return $"{Explore(lines, start).Count}";
    }

    public string SolvePart2(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var index = input.IndexOf('^');
        var len = lines[0].Length + Environment.NewLine.Length;
        var start = (x: index % len, y: index / len, d: 0);
        
        return $"{Task.WhenAll(Explore(lines, start)[1..]
            .Select(p => lines.Select((l, y) => y == p.y ? l[..p.x] + '#' + l[(p.x + 1)..] : l).ToArray())
            .Select(l => Task.Run(() => Explore(l, start)[^1]))).Result
            .Count(p => p.x != 0 && p.y != 0 && p.x != lines[0].Length - 1 && p.y != lines.Length - 1)}";
    }
}