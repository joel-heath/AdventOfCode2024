using AdventOfCode2024.Utilities;

namespace AdventOfCode2024;

public class Day21 : IDay
{
    public int Day => 21;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "029A\r\n980A\r\n179A\r\n456A\r\n379A", "126384" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "029A\r\n980A\r\n179A\r\n456A\r\n379A", "154115708116294" }
    };

    // ==                            arrow precedence explanation                             ==
    // - for the most efficient route between two points without trying them all and comparing -

    // we must consider the directional keypad not the numeric keypad
    // prioritise farthest from A first, that is |<|, then, |v|, finally |>| and |^| have equal precedence
    // but after 2 iterations |^| is preferred over |>|:
    // |^| -> |<| -> |v < <|
    // |>| -> |v| -> |< v  |
    // we can choose |^| then |>| = |v < < < v|
    //            or |<| then |v| = |< v v < <|
    // the former is more efficient because we only have to go to |<| once, which is further than |v|
    // going to use an OrderBy so smaller number = higher precedence, values in this dictionary are just their indices

    private static readonly Dictionary<char, int> arrowPrecedence = new char[] { '>', '^', 'v', '<' }
        .Select((c, i) => (c, i))
        .ToDictionary(t => t.c, t => t.i);

    private static Dictionary<char, (Point, char[])> GetDistances(Grid<char> map, Point start, int dangerY)
    {
        Dictionary<char, (Point, char[])> distances = [];

        foreach (var (key, pos) in map.AllPositions().Select(p => (key: map[p], pos: p)).Where(pair => pair.key != ' '))
        {
            var vector = pos - start;
            var horizontalMoves = Enumerable.Repeat(vector.X < 0 ? '<' : '>', (int)Math.Abs(vector.X));
            var verticalMoves = Enumerable.Repeat(vector.Y < 0 ? '^' : 'v', (int)Math.Abs(vector.Y));

            distances[key] = (pos, [.. start.Y == dangerY && pos.X == 0
                ? verticalMoves.Concat(horizontalMoves) // following precedence rules would go over the empty space- go up then left
                : pos.Y == dangerY && start.X == 0 
                ? horizontalMoves.Concat(verticalMoves) // following precedence rules would go over the empty space- go right then down
                : horizontalMoves.Concat(verticalMoves)
                    .OrderByDescending(a => arrowPrecedence[a])]); // otherwise follow precedence rules
        }

        return distances;
    }

    private static Grid<Dictionary<char, (Point, char[])>> CreateMap(Grid<char> grid, int dangerY)
        => new(grid.Width, grid.Height, grid.AllPositions()
                .Select(p => grid[p] == ' ' ? [] : GetDistances(grid, p, dangerY)));

    private static readonly Dictionary<(string, int), long> memo = [];
    private static long PathLength(string code, Point start1, Point start2, Grid<Dictionary<char, (Point, char[])>> distances1, Grid<Dictionary<char, (Point, char[])>> distances2, int maxDepth, int depth = 1)
    {
        if (memo.TryGetValue((code, depth), out long value)) return value;

        var (start, distances) = depth == 1 ? (start1, distances1) : (start2, distances2);
        return memo[(code, depth)] =
            code.Scan((pos: start, path: Array.Empty<char>()), (acc, c) => distances[acc.pos][c])
                .Select(data => data.path)
                .Sum(p => depth >= maxDepth
                    ? p.Length + 1 // +1 for .Append('A');
                    : PathLength(string.Concat(p.Append('A')), start1, start2, distances1, distances2, maxDepth, depth + 1)); ;
    }

    private static long Solve(string input, int robots)
    {
        var numpad = Grid<char>.FromString("789\r\n456\r\n123\r\n 0A");
        var dpad = Grid<char>.FromString(" ^A\r\n<v>");
        var numpadDistances = CreateMap(numpad, 3); // 3 is the y-index of the empty space
        var dpadDistances = CreateMap(dpad, 0);     // 0 is the y-index of the empty space

        return input.Split(Environment.NewLine)
            .Sum(l => long.Parse(l[..^1]) * PathLength(l, (2, 3), (2, 0), numpadDistances, dpadDistances, robots));
    }

    public string SolvePart1(string input)
        => $"{Solve(input, 3)}";

    public string SolvePart2(string input)
        => $"{Solve(input, 26)}";
}