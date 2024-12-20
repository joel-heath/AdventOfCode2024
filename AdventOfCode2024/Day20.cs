using AdventOfCode2024.Utilities;
using System.Reflection.Metadata.Ecma335;

namespace AdventOfCode2024;

public class Day20 : IDay
{
    public int Day => 20;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "#####\r\n#S#E#\r\n#.#.#\r\n#...#\r\n#####", "2" },
        { "###############\r\n#...#...#.....#\r\n#.#.#.#.#.###.#\r\n#S#...#.#.#...#\r\n#######.#.#.###\r\n#######.#.#...#\r\n#######.#.###.#\r\n###..E#...#...#\r\n###.#######.###\r\n#...###...#...#\r\n#.#####.#.###.#\r\n#.#...#.#.#...#\r\n#.#.#.#.#.#.###\r\n#...#...#...###\r\n###############", "44" },
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "TestInput1", "ExpectedOutput1" },
    };

    private static int ShortestPath(Grid<char> map, Point start, Point end)
    {
        Queue<(Point, int)> queue = new([(start, 0)]);
        HashSet<Point> visited = [start];
        while (queue.TryDequeue(out var data))
        {
            var (current, steps) = data;
            if (current == end)
                return steps;
            foreach (var next in map.Adjacents(current).Where(n => !visited.Contains(n) && map[n] != '#'))
            {
                visited.Add(next);
                queue.Enqueue((next, steps + 1));
            }
        }
        return -1;
    }

    // using the same idea as with a dijkstra's from A to C via B, you can do a dijkstra's from B, to A and C, and then sum the distances
    private static int ShortestPath(Grid<char> map, Point start, Point via, Point end)
    {
        Queue<(Point, int)> queue = new([(via, 0)]);
        HashSet<Point> visited = [via];
        int distToStart = -1;
        int distToEnd = -1;
        while (queue.TryDequeue(out var data))
        {
            var (current, steps) = data;

            if (distToEnd == -1 && current == end)
                distToEnd = steps;
            else if (distToStart == -1 && current == start)
                distToStart = steps;

            foreach (var next in map.Adjacents(current).Where(n => !visited.Contains(n) && map[n] != '#'))
            {
                visited.Add(next);
                queue.Enqueue((next, steps + 1));
            }
        }
        return distToStart + distToEnd;
    }

    public string SolvePart1(string input)
    {
        var map = Grid<char>.FromString(input);
        var start = map.AllPositions().First(p => map[p] == 'S');
        var end = map.AllPositions().First(p => map[p] == 'E');
        var normLength = ShortestPath(map, start, end);
        var targetLength = normLength - (UnitTestsP1.ContainsKey(input) ? 2 : 100);

        int count = 0;
        foreach (var pos in map.AllPositions()
            .Where(p => map[p] == '#')
            .Where(p => start.MDistanceTo(p) + p.MDistanceTo(end) <= targetLength)
            .Where(p => map.Adjacents(p).Count(n => map[n] != '#') >= 2))
        {    
            map[pos] = '.';
            var len = ShortestPath(map, start, pos, end);
            if (len > 0 && len <= targetLength)
                count++;
            map[pos] = '#';
        }

        return $"{count}";
    }

    public string SolvePart2(string input)
    {
        return string.Empty;
    }
}