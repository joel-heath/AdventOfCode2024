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

    static List<(int x, int y)> Explore(string[] lines, (int x, int y, int d) start)
    {
        var visited = new HashSet<(int x, int y, int d)>();
        var (x, y, d) = start;
        var (width, height) = (lines[0].Length, lines.Length);

        bool looped = false;
        while (x != 0 && y != 0 && x != width - 1 && y != height - 1 && !looped)
        {
            foreach (var point in LineOut(x, y, d, width, height).TakeWhile(p => lines[p.y][p.x] != '#'))
            {
                if (visited.Contains(point))
                {
                    looped = true;
                    break;
                }

                visited.Add(point);
                x = point.x;
                y = point.y;
            }
            d = (d + 1) % 4;
        }

        return visited.Select(p => (p.x, p.y)).Distinct().ToList();
    }

    static IEnumerable<(int x, int y, int d)> LineOut(int x, int y, int d, int width, int height)
        => d switch
        {
            0 => Utils.Range(y, 0 - y - 1).Select(i => (x, i, d)),
            1 => Utils.Range(x, width - x).Select(i => (i, y, d)),
            2 => Utils.Range(y, height - y).Select(i => (x, i, d)),
            3 => Utils.Range(x, 0 - x - 1).Select(i => (i, y, d)),
            _ => throw new InvalidOperationException()
        };

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
        var start = (index % len, index / len, 0);

        return $"{Explore(lines, start)[1..]
            .Select(p => lines.Select((l, y) => y == p.y ? l[..p.x] + '#' + l[(p.x + 1)..] : l).ToArray())
            .Select(l => Explore(l, start)[^1])
            .Count(p => p.x != 0 && p.y != 0 && p.x != lines[0].Length - 1 && p.y != lines.Length - 1)}";

        /* multithreaded option?
         * return $"{Task.WhenAll(Explore(lines, start)[1..]
         * .Select(p => lines.Select((l, y) => y == p.y ? l[..p.x] + '#' + l[(p.x + 1)..] : l).ToArray())
         * .Select(l => Task.Run(() => Explore(l, start)[^1]))).Result
         * .Count(p => p.x != 0 && p.y != 0 && p.x != lines[0].Length - 1 && p.y != lines.Length - 1)}";
         */
    }
}