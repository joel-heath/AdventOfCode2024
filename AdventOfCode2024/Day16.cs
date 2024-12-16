using AdventOfCode2024.Utilities;

namespace AdventOfCode2024;

public class Day16 : IDay
{
    public int Day => 16;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "###############\r\n#.......#....E#\r\n#.#.###.#.###.#\r\n#.....#.#...#.#\r\n#.###.#####.#.#\r\n#.#.#.......#.#\r\n#.#.#####.###.#\r\n#...........#.#\r\n###.#.#####.#.#\r\n#...#.....#.#.#\r\n#.#.#.###.#.#.#\r\n#.....#...#.#.#\r\n#.###.#.#.#.#.#\r\n#S..#.....#...#\r\n###############", "7036" },
        { "#################\r\n#...#...#...#..E#\r\n#.#.#.#.#.#.#.#.#\r\n#.#.#.#...#...#.#\r\n#.#.#.#.###.#.#.#\r\n#...#.#.#.....#.#\r\n#.#.#.#.#.#####.#\r\n#.#...#.#.#.....#\r\n#.#.#####.#.###.#\r\n#.#.#.......#...#\r\n#.#.###.#####.###\r\n#.#.#...#.....#.#\r\n#.#.#.#####.###.#\r\n#.#.#.........#.#\r\n#.#.#.#########.#\r\n#S#.............#\r\n#################", "11048" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "######\r\n#S.#E#\r\n#....#\r\n######", "7" },
        { "###############\r\n#.......#....E#\r\n#.#.###.#.###.#\r\n#.....#.#...#.#\r\n#.###.#####.#.#\r\n#.#.#.......#.#\r\n#.#.#####.###.#\r\n#...........#.#\r\n###.#.#####.#.#\r\n#...#.....#.#.#\r\n#.#.#.###.#.#.#\r\n#.....#...#.#.#\r\n#.###.#.#.#.#.#\r\n#S..#.....#...#\r\n###############", "45" },
        { "#################\r\n#...#...#...#..E#\r\n#.#.#.#.#.#.#.#.#\r\n#.#.#.#...#...#.#\r\n#.#.#.#.###.#.#.#\r\n#...#.#.#.....#.#\r\n#.#.#.#.#.#####.#\r\n#.#...#.#.#.....#\r\n#.#.#####.#.###.#\r\n#.#.#.......#...#\r\n#.#.###.#####.###\r\n#.#.#...#.....#.#\r\n#.#.#.#####.###.#\r\n#.#.#.........#.#\r\n#.#.#.#########.#\r\n#S#.............#\r\n#################", "64" }
    };
    
    private static readonly Point[] dirs = [(0, -1), (1, 0), (0, 1), (-1, 0)];

    private static IEnumerable<(Point p, Point dir)> Adjacents(Point p, Point dir)
    {
        yield return (p + dir, dir);
        var index = Array.IndexOf(dirs, dir);
        var right = dirs[Utils.Mod(index + 1, 4)];
        yield return (p + right, right);
        yield return (p - right, -right);
    }

    private static int SingleSolve(Grid<char> map, Point start, Point end)
    {
        var queue = new PriorityQueue<(Point pos, Point dir), int>();
        queue.Enqueue((start, (1, 0)), 0);
        int bestSoFar = int.MaxValue;
        HashSet <(Point pos, Point dir)> visited = [];
        while (queue.TryDequeue(out var data, out int steps))
        {
            var (current, dir) = data;
            if (steps >= bestSoFar)
                continue;
            if (current == end)
            {
                if (steps < bestSoFar)
                    bestSoFar = steps;
            }
            if (visited.Contains((current, dir)))
                continue;

            visited.Add((current, dir));

            foreach (var (next, newDir) in Adjacents(current, dir).Where(n => map[n.p] != '#'))
            {
                var cost = dir == newDir ? steps + 1 : steps + 1000 + 1;
                queue.Enqueue((next, newDir), cost);
            }
        }
        return bestSoFar;
    }

    private static int SolveAll(Grid<char> map, Point start, Point end)
    {
        var queue = new PriorityQueue<(Point pos, Point dir, HashSet<Point> history), int>();
        queue.Enqueue((start, (1, 0), []), 0);
        int bestSoFar = int.MaxValue;
        Dictionary<(Point pos, Point dir), int> visited = [];
        HashSet<Point> bestPath = [];
        while (queue.TryDequeue(out var data, out int steps))
        {
            var (current, dir, history) = data;
            if (steps > bestSoFar)
                continue;
            if (current == end)
            {
                if (steps == bestSoFar)
                {
                    bestPath.UnionWith(history);
                }
                else
                {
                    bestPath = [..history];
                    bestSoFar = steps;
                }
                continue;
            }
            if (visited.TryGetValue((current, dir), out int existingSteps))
            {
                if (steps > existingSteps)
                    continue;
            }

            visited.TryAdd((current, dir), steps);
            history = [.. history];
            history.Add(current);

            foreach (var (next, newDir) in Adjacents(current, dir).Where(n => !history.Contains(n.p) && map[n.p] != '#'))
            {
                var cost = dir == newDir ? steps + 1 : steps + 1000 + 1;
                queue.Enqueue((next, newDir, history), cost);
            }
        }

        bestPath.Add(end);
        return bestPath.Count;
    }

    public string SolvePart1(string input)
    {
        var map = Grid<char>.FromString(input);
        var start = map.AllPositions().First(p => map[p] == 'S');
        var end = map.AllPositions().First(p => map[p] == 'E');

        return $"{SingleSolve(map, start, end)}";
    }

    public string SolvePart2(string input)
    {
        var map = Grid<char>.FromString(input);
        var start = map.AllPositions().First(p => map[p] == 'S');
        var end = map.AllPositions().First(p => map[p] == 'E');

        return $"{SolveAll(map, start, end)}";
    }
}