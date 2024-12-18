using AdventOfCode2024.Utilities;
using System.Security.Cryptography;

namespace AdventOfCode2024;

public class Day18 : IDay
{
    public int Day => 18;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "5,4\r\n4,2\r\n4,5\r\n3,0\r\n2,1\r\n6,3\r\n2,4\r\n1,5\r\n0,6\r\n3,3\r\n2,6\r\n5,1\r\n1,2\r\n5,5\r\n2,5\r\n6,5\r\n1,4\r\n0,4\r\n6,4\r\n1,1\r\n6,1\r\n1,0\r\n0,5\r\n1,6\r\n2,0", "22" },
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "5,4\r\n4,2\r\n4,5\r\n3,0\r\n2,1\r\n6,3\r\n2,4\r\n1,5\r\n0,6\r\n3,3\r\n2,6\r\n5,1\r\n1,2\r\n5,5\r\n2,5\r\n6,5\r\n1,4\r\n0,4\r\n6,4\r\n1,1\r\n6,1\r\n1,0\r\n0,5\r\n1,6\r\n2,0", "6,1" },
    };

    private static readonly Point[] dirs = [(0, -1), (1, 0), (0, 1), (-1, 0)];

    private static bool Contained(Point p, int width)
        => p.X >= 0 && p.Y >= 0 && p.X < width && p.Y < width;

    private static IEnumerable<Point> Adjacents(Point p)
        => dirs.Select(d => p + d);

    private static int ShortestPath(Point start, Point end, List<Point> corrupted, int width)
    {
        Queue<(Point, int)> queue = new([(start, 0)]);
        HashSet<Point> visited = [start];
        while (queue.Count > 0)
        {
            var (current, steps) = queue.Dequeue();
            if (current == end)
            {
                return steps;
            }
            foreach (var next in Adjacents(current).Where(p => Contained(p, width)))
            {
                if (visited.Contains(next) || corrupted.Contains(next))
                {
                    continue;
                }
                visited.Add(next);
                queue.Enqueue((next, steps + 1));
            }
        }
        return -1;
    }

    public string SolvePart1(string input)
    {
        int width = (UnitTestsP1.ContainsKey(input) ? 6 : 70) + 1;
        var corrupted = input.Split(Environment.NewLine)
            .Select(l => l.Split(','))
            .Select(l => new Point(long.Parse(l[0]), long.Parse(l[1])))
            .Take(..(UnitTestsP1.ContainsKey(input) ? 12 : 1024))
            .ToList();

        return $"{ShortestPath((0, 0), (width - 1, width - 1), corrupted, width)}";
    }

    public string SolvePart2(string input)
    {
        int width = (UnitTestsP1.ContainsKey(input) ? 6 : 70) + 1;
        var corrupted = input.Split(Environment.NewLine)
            .Select(l => l.Split(','))
            .Select(l => new Point(long.Parse(l[0]), long.Parse(l[1])))
            .ToList();

        int left = 0;
        int right = corrupted.Count;
        while (left != right)
        {
            int mid = (left + right) / 2;
            if (ShortestPath((0, 0), (width - 1, width - 1), corrupted.Take(mid).ToList(), width) == -1)
                right = mid;
            else
                left = mid + 1;
        }
        return $"{corrupted[left - 1].X},{corrupted[left - 1].Y}";
    }
}