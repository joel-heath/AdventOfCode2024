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

    public string SolvePart1(string input)
    {
        long sum = 0;

        var lines = input.Split(Environment.NewLine);
        var index = input.IndexOf('^');
        var len = lines[0].Length + Environment.NewLine.Length;
        var start = (index % len, index / len, 0);

        return $"{Explore(lines, start, 0)}";
    }

    static int Explore(string[] lines, (int x, int y, int d) start, int meta)
    {
        var visited = new HashSet<(int x, int y, int d)>();
        var queue = new Queue<(int x, int y, int d)>();
        queue.Enqueue(start);

        int x=-1, y=-1, d=-1;
        while (queue.Count > 0)
        {
            (x, y, d) = queue.Dequeue();
            if (visited.Contains((x, y, d)))
                continue;
            
            if (lines[y][x] == '#')
            {
                // undo move forward
                x += d == 1 ? -1 : d == 3 ? 1 : 0;
                y += d == 0 ? 1 : d == 2 ? -1 : 0;
                d = (d + 1) % 4; // turn right
            }
            
            visited.Add((x, y, d));

            if (d == 0 && y > 0)
            {    
                queue.Enqueue((x, y - 1, d));
            }
            else if (d == 1 && x < lines[0].Length - 1)
            {
                queue.Enqueue((x + 1, y, d));
            }
            else if (d == 2 && y < lines.Length - 1)
            {
                queue.Enqueue((x, y + 1, d));
            }
            else if (d == 3 && x > 0)
            {
                queue.Enqueue((x - 1, y, d));
            }
        }

        var positions = visited.Select(p => (p.x, p.y)).Distinct().ToList();
        if (meta == 0) return positions.Count; // part 1

        if (meta == 1)
        {
            if (x == 0 || y == 0 || x == lines[0].Length - 1 || y == lines.Length - 1)
                return 0;
            return 1;
        }

        // if (meta == 2)
        int count = 0;
        foreach (var pos in positions)
        {
            var newLines = lines.Select(l => l.ToCharArray()).ToArray();
            newLines[pos.y][pos.x] = '#';
            count += Explore(newLines.Select(l => string.Concat(l)).ToArray(), start, 1);
        }

        return count;
    }

    public string SolvePart2(string input)
    {
        long sum = 0;

        var lines = input.Split(Environment.NewLine);
        var index = input.IndexOf('^');
        var len = lines[0].Length + Environment.NewLine.Length;
        var start = (index % len, index / len, 0);

        return $"{Explore(lines, start, 2)}";
    }
}