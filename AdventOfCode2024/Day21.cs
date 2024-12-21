using AdventOfCode2024.Utilities;

namespace AdventOfCode2024;

public class Day21 : IDay
{
    public int Day => 21;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "456A", "29184" },
        { "379A", "24256" },
        { "029A\r\n980A\r\n179A\r\n456A\r\n379A", "126384" },
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "TestInput1", "ExpectedOutput1" },
    };

    private static readonly Point[] directions = [(0, -1), (1, 0), (0, 1), (-1, 0)];
    private static readonly char[] arrows = ['^', '>', 'v', '<'];

    private static Grid<char[]> Dijkstras(Grid<char> map, Point start)
    {
        Grid<char[]> distances = new(map.Width, map.Height);
        distances[start] = [];
        Queue<(Point, char[])> queue = new([(start, [])]);
        HashSet<Point> visited = [start];
        while (queue.TryDequeue(out var data))
        {
            var (current, steps) = data;
            distances[current] = steps;
            foreach (var next in map.Adjacents(current).Where(n => !visited.Contains(n) && map[n] != ' '))
            {
                visited.Add(next);
                queue.Enqueue((next, [..steps, arrows[Array.IndexOf(directions, next - current)]]));
            }
        }
        return distances;
    }

    public string SolvePart1(string input)
    {
        var numpad = Grid<char>.FromString("789" + Environment.NewLine
                                         + "456" + Environment.NewLine
                                         + "123" + Environment.NewLine
                                         + " 0A");

        var numpadStart = numpad.AllPositions().First(p => numpad[p] == 'A');
        var numpadDistances = new Dictionary<char, Grid<char[]>>();
        for (int j = 0; j < numpad.Width; j++)
        {
            for (int i = 0; i < numpad.Height; i++)
            {
                if (numpad[(j, i)] == ' ') continue;
                numpadDistances[numpad[(j, i)]] = Dijkstras(numpad, (j, i));
            }
        }

        var dpad = Grid<char>.FromString(" ^A" + Environment.NewLine
                                       + "<v>");
        var dpadStart = dpad.AllPositions().First(p => dpad[p] == 'A');
        var dpadDistances = new Dictionary<char, Grid<char[]>>();
        for (int j = 0; j < dpad.Width; j++)
        {
            for (int i = 0; i < dpad.Height; i++)
            {
                if (dpad[(j, i)] == ' ') continue;
                dpadDistances[dpad[(j, i)]] = Dijkstras(dpad, (j, i));
            }
        }

        long summation = 0;
        Console.WriteLine();
        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < lines.Length; i++)
        {
            var code = lines[i];
            //code = "2A";
            //code = "^v";
            //code = "v<<A>^A>A";
            //code = "<Av<A>>^A";
            var path = Path(code, numpadStart, dpadStart, numpadDistances, dpadDistances);
            var strPath = string.Concat(path);
            Console.WriteLine($"{code}: {strPath}");
            //Console.Write($"{path.Count} * {long.Parse(code[..^1])} + ");
            summation += path.Count * long.Parse(code[..^1]);
        }

        return $"{summation}";
    }

    private static List<char> Path(string code, Point start1, Point start2, Dictionary<char, Grid<char[]>> distances1, Dictionary<char, Grid<char[]>> distances2, int depth = 0)
    {
        var distances = depth == 0 ? distances1 : distances2;
        var start = depth == 0 ? start1 : start2;
        var (x, y) = start;
        List<char> fullPath = [];

        for (int j = 0; j < code.Length; j++)
        {
            var c = code[j];
            var priority = depth == 0 ? '^' : 'v'; // to avoid the empty space
            var dangerY = start.Y; // the empty space is on the same row as 'A'
            // should be [(x, y)][c] but can't be indexed that way, so need to reverse and invert.
            var path = distances[c][(x, y)]
                .Select(c => arrows[(Array.IndexOf(arrows, c) + 2) % 4]).Reverse()
                .GroupBy(c => c).SelectMany(g => g) // reorder < v < into < < v
                .OrderByDescending(c => distances2[c][start2].Length) // prioritise farthest to repeat
                                                                      // given code 2
                                                                      // we can have ^v or v^
                                                                      // <Av<A>>^A  or  v<<A>^A>A respectively
                                                                      // One repeats > twice, one repeats <
                                                                      // it is more cost-efficient to repeat the farthest one, so repeat <
                                                                      // this means order by farthest first
                // unfortunately, this makes us cheat
                // this turns |v < <| into |< < v |
                // this goes through an illegal empty space, so farthest takes precedence, but going up takes MORE precedence
                .OrderBy(c => c == priority && y == dangerY ? 0 : 1) // this doesn't fix it because of going back down
                .Append('A')
                .ToList();

            (x, y) = path.Where(c => c != 'A').Aggregate(new Point(x, y), (acc, c) => acc + directions[Array.IndexOf(arrows, c)]);
            fullPath.AddRange(
                depth == 3 - 1
                    ? path
                    : Path(string.Concat(path), start1, start2, distances1, distances2, depth + 1));
        }
        // this will convert |< v <| into |v < <| to save on the number of steps
        //fullPath = string.Concat(fullPath).Split('A').SelectMany(i => i.GroupBy(c => c).SelectMany(g => g).Append('A')).ToList();
        return fullPath;
    }

    // 171230 too high

    public string SolvePart2(string input)
    {
        return string.Empty;
    }
}