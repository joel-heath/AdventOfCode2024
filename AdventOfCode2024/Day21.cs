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
    private static readonly Dictionary<char, int> arrowPrecedence = new() { { '^', 0 }, { '>', 0 }, { 'v', 1 }, { '<', 2 } };

    private static Dictionary<char, char[]> GetDistances(Grid<char> map, Point start, int dangerY)
    {
        Dictionary<char, char[]> distances = [];

        foreach (var (key, pos) in map.AllPositions().Select(p => (key: map[p], pos: p)).Where(kvp => kvp.key != ' '))
        {
            var vector = pos - start;
            bool goUpDanger =  start.Y == dangerY && pos.X == 0;
            bool goRightDanger = pos.Y == dangerY && start.X == 0;
            var horizontalMoves = Enumerable.Repeat(vector.X < 0 ? '<' : '>', (int)Math.Abs(vector.X));
            var verticalMoves = Enumerable.Repeat(vector.Y < 0 ? '^' : 'v', (int)Math.Abs(vector.Y));

            var result = goUpDanger
                ? verticalMoves.Concat(horizontalMoves)
                : goRightDanger
                ? horizontalMoves.Concat(verticalMoves)
                : horizontalMoves.Concat(verticalMoves)
                    .OrderByDescending(a => arrowPrecedence[a]);

            distances[key] = [..result];
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

    
    private static IEnumerable<char> Path(string code, Point start1, Point start2, Grid<Dictionary<char, char[]>> distances1, Grid<Dictionary<char, char[]>> distances2, int maxDepth, int depth = 1)
    {
        var distances = depth == 1 ? distances1 : distances2;
        var start = depth == 1 ? start1 : start2;
        var (x, y) = start;

        for (int j = 0; j < code.Length; j++)
        {
            var c = code[j];
            var path = distances[(x, y)][c].Append('A').ToList();

            (x, y) = (x, y) + path.Where(c => c != 'A').Sum(c => directions[Array.IndexOf(arrows, c)]);
            foreach (var p in depth == maxDepth
                    ? path
                    : Path(string.Concat(path), start1, start2, distances1, distances2, maxDepth, depth + 1))
                yield return p;
        }
    }

    private static long Solve(string input, int robotDPadCount)
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

        long summation = 0;
        //Console.WriteLine();
        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < lines.Length; i++)
        {
            var code = lines[i];
            var path = Path(code, numpadStart, dpadStart, numpadDistances, dpadDistances, robotDPadCount).ToList();
            var strPath = string.Concat(path);
            //Console.WriteLine($"{code}: {strPath}");
            //Console.Write($"{path.Count} * {long.Parse(code[..^1])} + ");
            summation += path.Count * long.Parse(code[..^1]);
        }

        return summation;
    }

    public string SolvePart1(string input)
        => $"{Solve(input, 3)}";

    public string SolvePart2(string input)
    {
        // dpads | answer | time | memo time | cache size
        // 15 | 9967201472 | 3s 93ms | 412ms |
        // 16 | 24950107452 | 7s 997ms |  |
        // 17 | 62454399010 | 19s 573ms |  |
        // 18 | 156335856824 | 48s 537ms | 5s 24ms |
        // 19 | 391337419096 |  | 8s 549ms | 13.5GB memo
        // 20 | 979593208654 |  | 23s 25ms | 23GB memo
        // 21 | 2452105977334 |  | 1m 7s |

        return $"{Solve(input, 25)}";
    }
}