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

    private static HashSet<Point> FindArea(Point startingPos, Grid<char> grid)
    {
        HashSet<Point> visited = [startingPos];
        Queue<Point> queue = new();
        queue.Enqueue(startingPos);
        var cellType = grid[startingPos];

        while (queue.TryDequeue(out Point current))
        {
            foreach (var neighbor in grid.Adjacents(current).Where(n => grid[n] == cellType && !visited.Contains(n)))
            {
                visited.Add(neighbor);
                queue.Enqueue(neighbor);
            }
        }
        return visited;
    }

    private static int FindPerimeter(HashSet<Point> points, Grid<char> grid)
        => points.Sum(point =>
            grid.Adjacents(point, contained: false)
                .Count(n => !points.Contains(n)));

    private static readonly Point[] vertical = [(0, 0), (1, 0)];
    private static readonly Point[] horizontal = [(0, 0), (0, 1)];

    private static int FindSides(HashSet<Point> points)
    {
        HashSet<(Point p, bool d)> verticalLines = [];   // d is if the line is on the right side of the point
        HashSet<(Point p, bool d)> horizontalLines = []; // d is if the line is on the bottom side of the point

        foreach (var point in points)
        {
            foreach (var (neighbor, i) in vertical
                .Select((v, i) => (n: point + v, i))
                .Where(d => !points.Contains(d.n) || !points.Contains(d.n + (-1, 0))))
                verticalLines.Add((neighbor, i == 1));
            
            foreach (var (neighbor, i) in horizontal
                .Select((v, i) => (n: point + v, i))
                .Where(d => !points.Contains(d.n) || !points.Contains(d.n + (0, -1))))
                horizontalLines.Add((neighbor, i == 1));
        }

        return Reduce(verticalLines
                .GroupBy(l => (l.d ? 1 : -1) * l.p.X)
                .Select(g => g.Select(d => d.p).OrderBy(p => p.Y)))
            + Reduce(horizontalLines
                .GroupBy(l => (l.d ? 1 : -1) * l.p.Y)
                .Select(g => g.Select(d => d.p).OrderBy(p => p.X)));
    }

    static int Reduce(IEnumerable<IOrderedEnumerable<Point>> data)
        => data.Select(row => row.ToList())
            .Sum(row => row.Aggregate((count: 1, prev: row[0]), (acc, point) =>
                Math.Abs(point.X - acc.prev.X) + Math.Abs(point.Y - acc.prev.Y) > 1
                    ? (acc.count + 1, point)
                    : (acc.count, point)).count);

    private static int Solver(string input, bool discount)
    {
        int totalPrice = 0;
        HashSet<Point> seen = [];
        Grid<char> grid = Grid<char>.FromString(input);

        foreach (var (x, y) in grid.AllPositions().Where(p => !seen.Contains(p)))
        {
            var visited = FindArea((x, y), grid);
            seen.UnionWith(visited);
            totalPrice += visited.Count * (discount ? FindSides(visited) : FindPerimeter(visited, grid));
        }

        return totalPrice;
    }

    public string SolvePart1(string input)
        => $"{Solver(input, false)}";

    public string SolvePart2(string input)
        => $"{Solver(input, true)}";

}