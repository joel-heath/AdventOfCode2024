using AdventOfCode2024.Utilities;

namespace AdventOfCode2024;

public class Day21 : IDay
{
    public int Day => 21;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "456A", "29184" },
        { "379A", "24256" },
        { "029A\r\n980A\r\n179A\r\n456A\r\n379A", "126384" }
    };
    public Dictionary<string, string> UnitTestsP2 => [];

    private static readonly Point[] directions = [(0, -1), (1, 0), (0, 1), (-1, 0)];
    private static readonly char[] arrows = ['^', '>', 'v', '<'];
    private static readonly Dictionary<char, int> arrowPrecedence = new() { { '^', 1 }, { '>', 0 }, { 'v', 2 }, { '<', 3 } };

    private static Dictionary<char, char[]> GetDistances(Grid<char> map, Point start, int dangerY)
    {
        Dictionary<char, char[]> distances = [];

        foreach (var (key, pos) in map.AllPositions().Select(p => (key: map[p], pos: p)).Where(pair => pair.key != ' '))
        {
            var vector = pos - start;
            bool goUpDanger =  start.Y == dangerY && pos.X == 0;
            bool goRightDanger = pos.Y == dangerY && start.X == 0;
            var horizontalMoves = Enumerable.Repeat(vector.X < 0 ? '<' : '>', (int)Math.Abs(vector.X));
            var verticalMoves = Enumerable.Repeat(vector.Y < 0 ? '^' : 'v', (int)Math.Abs(vector.Y));

            distances[key] = [..goUpDanger
                ? verticalMoves.Concat(horizontalMoves)
                : goRightDanger
                ? horizontalMoves.Concat(verticalMoves)
                : horizontalMoves.Concat(verticalMoves)
                    .OrderByDescending(a => arrowPrecedence[a])];
        }

        return distances;
    }

    private static Grid<Dictionary<char, char[]>> CreateMap(Grid<char> grid, int dangerY)
    {
        var distances = new Grid<Dictionary<char, char[]>>(grid.Width, grid.Height);
        for (int j = 0; j < grid.Width; j++)
        {
            for (int i = 0; i < grid.Height; i++)
            {
                if (grid[(j, i)] == ' ') continue;
                distances[(j, i)] = GetDistances(grid, (j, i), dangerY);
            }
        }
        return distances;
    }

    private static readonly Dictionary<(string, int), long> memo = [];
    private static long PathLength(string code, Point start1, Point start2, Grid<Dictionary<char, char[]>> distances1, Grid<Dictionary<char, char[]>> distances2, int maxDepth, int depth = 1)
    {
        if (memo.TryGetValue((code, depth), out long value)) return value;

        var distances = depth == 1 ? distances1 : distances2;
        var start = depth == 1 ? start1 : start2;
        var (x, y) = start;

        long total = 0;
        for (int j = 0; j < code.Length; j++)
        {
            var c = code[j];
            var path = distances[(x, y)][c].ToList();
            (x, y) = (x, y) + path.Sum(c => directions[Array.IndexOf(arrows, c)]);
            total += depth >= maxDepth
                ? path.Count + 1 // +1 for .Append('A');
                : PathLength(string.Concat(path.Append('A')), start1, start2, distances1, distances2, maxDepth, depth + 1);
        }
        
        return memo[(code, depth)] = total;
    }

    private static long Solve(string input, int robots)
    {
        var numpad = Grid<char>.FromString("789" + Environment.NewLine
                                         + "456" + Environment.NewLine
                                         + "123" + Environment.NewLine
                                         + " 0A");
        var dpad = Grid<char>.FromString(" ^A" + Environment.NewLine
                                       + "<v>");
        var numpadStart = numpad.AllPositions().First(p => numpad[p] == 'A');
        var numpadDistances = CreateMap(numpad, (int)numpadStart.Y);
        var dpadStart = dpad.AllPositions().First(p => dpad[p] == 'A');
        var dpadDistances = CreateMap(dpad, (int)dpadStart.Y);

        return input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Sum(l => long.Parse(l[..^1]) * PathLength(l, numpadStart, dpadStart, numpadDistances, dpadDistances, robots));
    }

    public string SolvePart1(string input)
        => $"{Solve(input, 3)}";

    public string SolvePart2(string input)
        => $"{Solve(input, 26)}";
}