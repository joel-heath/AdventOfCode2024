using AdventOfCode2024.Utilities;

namespace AdventOfCode2024;

public class Day20 : IDay
{
    public int Day => 20;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "###############\r\n#...#...#.....#\r\n#.#.#.#.#.###.#\r\n#S#...#.#.#...#\r\n#######.#.#.###\r\n#######.#.#...#\r\n#######.#.###.#\r\n###..E#...#...#\r\n###.#######.###\r\n#...###...#...#\r\n#.#####.#.###.#\r\n#.#...#.#.#...#\r\n#.#.#.#.#.#.###\r\n#...#...#...###\r\n###############", "44" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "###############\r\n#...#...#.....#\r\n#.#.#.#.#.###.#\r\n#S#...#.#.#...#\r\n#######.#.#.###\r\n#######.#.#...#\r\n#######.#.###.#\r\n###..E#...#...#\r\n###.#######.###\r\n#...###...#...#\r\n#.#####.#.###.#\r\n#.#...#.#.#...#\r\n#.#.#.#.#.#.###\r\n#...#...#...###\r\n###############", "285" }
    };

    private static IEnumerable<Point> Radius(int radius)
    {
        for (int x = -radius; x <= radius; x++)
            for (int y = -radius; y <= radius; y++)
                if (Math.Abs(x) + Math.Abs(y) <= Math.Abs(radius))
                    yield return (x, y);
    }

    private static Grid<int> Dijkstras(Grid<char> map, Point start)
    {
        Grid<int> distances = new(map.Width, map.Height, -1);
        distances[start] = 0;
        Queue<(Point, int)> queue = new([(start, 0)]);
        HashSet<Point> visited = [start];
        while (queue.TryDequeue(out var data))
        {
            var (current, steps) = data;
            distances[current] = steps;
            foreach (var next in map.Adjacents(current).Where(n => !visited.Contains(n) && map[n] != '#'))
            {
                visited.Add(next);
                queue.Enqueue((next, steps + 1));
            }
        }
        return distances;
    }

    private int Solve(string input, int radius, int testThreshold)
    {
        var map = Grid<char>.FromString(input);
        var start = map.AllPositions().First(p => map[p] == 'S');
        var end = map.AllPositions().First(p => map[p] == 'E');
        var startDistances = Dijkstras(map, start);
        var endDistances = Dijkstras(map, end);
        var normLength = endDistances[start];
        var targetLength = normLength - (UnitTestsP1.ContainsKey(input) ? testThreshold : 100);

        int count = 0;
        foreach (var pos in map.AllPositions()
            .Where(p => map[p] != '#'))
        {
            var candidates = Radius(radius)
                .Select(p => p + pos)
                .Where(p => p != pos)
                .Where(map.Contains)
                .Where(p => map[p] != '#');

            foreach (var cand in candidates)
            {
                var dist = startDistances[pos] + pos.MDistanceTo(cand) + endDistances[cand];
                if (dist <= targetLength)
                    count++;
            }
        }

        return count;
    }

    public string SolvePart1(string input)
        => $"{Solve(input, 2, 2)}";

    public string SolvePart2(string input)
       => $"{Solve(input, 20, 50)}";
}