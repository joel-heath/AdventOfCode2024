using AdventOfCode2024.Utilities;

namespace AdventOfCode2024;

public class Day18 : IDay
{
    public int Day => 18;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "5,4\r\n4,2\r\n4,5\r\n3,0\r\n2,1\r\n6,3\r\n2,4\r\n1,5\r\n0,6\r\n3,3\r\n2,6\r\n5,1\r\n1,2\r\n5,5\r\n2,5\r\n6,5\r\n1,4\r\n0,4\r\n6,4\r\n1,1\r\n6,1\r\n1,0\r\n0,5\r\n1,6\r\n2,0", "22" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "5,4\r\n4,2\r\n4,5\r\n3,0\r\n2,1\r\n6,3\r\n2,4\r\n1,5\r\n0,6\r\n3,3\r\n2,6\r\n5,1\r\n1,2\r\n5,5\r\n2,5\r\n6,5\r\n1,4\r\n0,4\r\n6,4\r\n1,1\r\n6,1\r\n1,0\r\n0,5\r\n1,6\r\n2,0", "6,1" }
    };
    
    private static int ShortestPath(Point start, Point end, List<Point> corrupted, int width)
    {
        Queue<(Point, int)> queue = new([(start, 0)]);
        HashSet<Point> visited = [start];
        while (queue.TryDequeue(out var data))
        {
            var (current, steps) = data;
            if (current == end)
                return steps;
            foreach (var next in current.Adjacents(width: width).Where(n => !visited.Contains(n) && !corrupted.Contains(n)))
            {
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

        var index = Utils.BinarySearch(corrupted.Count - 1,
            mid => ShortestPath((0, 0), (width - 1, width - 1), corrupted.Take(mid).ToList(), width) > -1);

        return $"{corrupted[index].X},{corrupted[index].Y}";
    }
}