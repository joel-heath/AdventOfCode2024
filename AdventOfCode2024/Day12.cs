using AdventOfCode2024.Utilities;

namespace AdventOfCode2024;

public class Day12 : IDay
{
    public int Day => 12;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "AAAA\r\nBBCD\r\nBBCC\r\nEEEC", "140" },
        { "OOOOO\r\nOXOXO\r\nOOOOO\r\nOXOXO\r\nOOOOO", "772" },
        { "RRRRIICCFF\r\nRRRRIICCCF\r\nVVRRRCCFFF\r\nVVRCCCJFFF\r\nVVVVCJJCFE\r\nVVIVCCJJEE\r\nVVIIICJJEE\r\nMIIIIIJJEE\r\nMIIISIJEEE\r\nMMMISSJEEE", "1930" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "AAAAAA\r\nAAABBA\r\nAAABBA\r\nABBAAA\r\nABBAAA\r\nAAAAAA", "368" },
        { "AAAA\r\nBBCD\r\nBBCC\r\nEEEC", "80" },
        { "OOOOO\r\nOXOXO\r\nOOOOO\r\nOXOXO\r\nOOOOO", "436" },
        { "EEEEE\r\nEXXXX\r\nEEEEE\r\nEXXXX\r\nEEEEE", "236" },
        { "RRRRIICCFF\r\nRRRRIICCCF\r\nVVRRRCCFFF\r\nVVRCCCJFFF\r\nVVVVCJJCFE\r\nVVIVCCJJEE\r\nVVIIICJJEE\r\nMIIIIIJJEE\r\nMIIISIJEEE\r\nMMMISSJEEE", "1206" }
    };

    static HashSet<Point> FindArea(Point startingPos, Grid<char> grid)
    {
        HashSet<Point> visited = [startingPos];
        Queue<Point> queue = new();
        queue.Enqueue(startingPos);
        var cellType = grid[startingPos];

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            foreach (var neighbor in grid.Adjacents(current).Where(n => grid[n] == cellType))
            {
                if (visited.Contains(neighbor))
                    continue;
                visited.Add(neighbor);
                queue.Enqueue(neighbor);
            }
        }
        return visited;
    }

    static int FindPerimeter(HashSet<Point> points, Grid<char> grid)
    {
        int count = 0;
        foreach (var point in points)
        {
            foreach (var neighbor in grid.Adjacents(point, contained: false))
            {
                if (points.Contains(neighbor))
                    continue;
                count++;
            }
        }
        return count;
    }

    static int FindSides(HashSet<Point> points, Grid<char> grid)
    {
        HashSet<(Point p, bool d)> verticalLines = [];
        HashSet<(Point p, bool d)> horizontalLines = [];
        int count = 0;
        Point[] vertical = [(0, 0), (1, 0)];
        Point[] horizontal = [(0, 0), (0, 1)];
        foreach (var point in points)
        {
            foreach (var (neighbor, i) in vertical.Select((v, i) => (point + v, i)))
            {
                if (points.Contains(neighbor) && points.Contains(neighbor + (-1, 0)))
                    continue;
                verticalLines.Add((neighbor, i == 1));
            }
            foreach (var (neighbor, i) in horizontal.Select((v, i) => (point + v, i)))
            {
                if (points.Contains(neighbor) && points.Contains(neighbor + (0, -1)))
                    continue;
                horizontalLines.Add((neighbor, i == 1));
            }
        }
        return ReduceVertical(verticalLines, grid) + ReduceHorizonal(horizontalLines, grid);
    }

    static int ReduceVertical(HashSet<(Point p, bool d)> points, Grid<char> grid)
    {
        var data = points.GroupBy(l => (l.d ? 1 : -1) * l.p.X)
            .Select(g => g.Select(d => d.p).OrderBy(p => p.Y));
        var count = 0;
        foreach (var row in data)
        {
            var line = row.ToList();
            var prev = line[0];
            count++;

            if (line.Count > 1)
            {
                foreach (var point in line.Skip(1))
                {
                    var diff = point - prev;

                    if (Abs(diff) > 1)
                        count++;

                    prev = point;
                }
            }
        }

        return count;
    }

    static int ReduceHorizonal(HashSet<(Point p, bool d)> points, Grid<char> grid)
    {
        var data = points.GroupBy(l => (l.d ? 1 : -1) * l.p.Y)
            .Select(g => g.Select(d => d.p).OrderBy(p => p.X));
        var count = 0;
        foreach (var row in data)
        {
            var line = row.ToList();
            var prev = line[0];
            count++;

            if (line.Count > 1)
            {
                foreach (var point in line.Skip(1))
                {
                    var diff = point - prev;

                    if (Abs(diff) > 1)
                        count++;

                    prev = point;
                }
            }
        }

        return count;
    }

    static long Abs(Point point) => Math.Abs(point.X) + Math.Abs(point.Y);

    public string SolvePart1(string input)
    {
        long totalPrice = 0;

        List<HashSet<Point>> regions = [];
        Dictionary<Point, int> regionMap = [];

        Grid<char> grid = Grid<char>.FromString(input);
        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                if (regionMap.ContainsKey((x, y)))
                    continue;

                var cell = grid[x, y];
                var visited = FindArea((x, y), grid);
                foreach (var point in visited)
                {
                    regionMap[point] = regions.Count;
                }
                regions.Add(visited);

                int area = visited.Count;
                int permimeter = FindPerimeter(visited, grid);

                totalPrice += area * permimeter;
            }
        }

        return $"{totalPrice}";
    }

    public string SolvePart2(string input)
    {
        long totalPrice = 0;

        List<HashSet<Point>> regions = [];
        Dictionary<Point, int> regionMap = [];

        Grid<char> grid = Grid<char>.FromString(input);
        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                if (regionMap.ContainsKey((x, y)))
                    continue;

                var cell = grid[x, y];
                var visited = FindArea((x, y), grid);
                foreach (var point in visited)
                {
                    regionMap[point] = regions.Count;
                }
                regions.Add(visited);

                int area = visited.Count;
                int sides = FindSides(visited, grid);

                totalPrice += area * sides;
            }
        }

        return $"{totalPrice}";
    }
}