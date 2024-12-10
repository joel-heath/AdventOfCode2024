using AdventOfCode2024.Utilities;

namespace AdventOfCode2024;

public class Day10 : IDay
{
    public int Day => 10;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "0123\r\n1234\r\n8765\r\n9876\r\n", "1" },
        { "...0...\r\n...1...\r\n...2...\r\n6543456\r\n7.....7\r\n8.....8\r\n9.....9\r\n", "2" },
        { "..90..9\r\n...1.98\r\n...2..7\r\n6543456\r\n765.987\r\n876....\r\n987....\r\n", "4" },
        { "10..9..\r\n2...8..\r\n3...7..\r\n4567654\r\n...8..3\r\n...9..2\r\n.....01\r\n", "3" },
        { "89010123\r\n78121874\r\n87430965\r\n96549874\r\n45678903\r\n32019012\r\n01329801\r\n10456732\r\n", "36" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { ".....0.\r\n..4321.\r\n..5..2.\r\n..6543.\r\n..7..4.\r\n..8765.\r\n..9....\r\n", "3" },
        { "..90..9\r\n...1.98\r\n...2..7\r\n6543456\r\n765.987\r\n876....\r\n987....\r\n", "13" },
        { "012345\r\n123456\r\n234567\r\n345678\r\n4.6789\r\n56789.\r\n", "227" },
        { "89010123\r\n78121874\r\n87430965\r\n96549874\r\n45678903\r\n32019012\r\n01329801\r\n10456732\r\n", "81" }
    };

    private static HashSet<(int y, int x)> ExploreP1(int[][] grid, int y, int x, int n)
    {
        if (y < 0 || y >= grid.Length || x < 0 || x >= grid[0].Length)
            return [];
        if (grid[y][x] != n)
            return [];
        if (n == 9)
            return [(y, x)];

        var heads = ExploreP1(grid, y + 1, x, n + 1);
        heads.UnionWith(ExploreP1(grid, y - 1, x, n + 1));
        heads.UnionWith(ExploreP1(grid, y, x + 1, n + 1));
        heads.UnionWith(ExploreP1(grid, y, x - 1, n + 1));
        return heads;
    }

    private static int ExploreP2(int[][] grid, int y, int x, int n)
    {
        if (y < 0 || y >= grid.Length || x < 0 || x >= grid[0].Length)
            return 0;
        if (grid[y][x] != n)
            return 0;
        if (n == 9)
            return 1;

        var sum = 0;
        sum += ExploreP2(grid, y + 1, x, n + 1);
        sum += ExploreP2(grid, y - 1, x, n + 1);
        sum += ExploreP2(grid, y, x + 1, n + 1);
        sum += ExploreP2(grid, y, x - 1, n + 1);
        return sum;
    }

    private static int Solver(string input, Func<int[][], int, int, int, int> exploreFunc)
    {
        int summation = 0;

        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(z => z.ToCharArray().Select(x => x - '0').ToArray()).ToArray();
        for (int i = 0; i<lines.Length; i++)
        {
            var line = lines[i];

            for (int j = 0; j < line.Length; j++)
            {
                summation += exploreFunc(lines, i, j, 0);
            }
        }

        return summation;
    }

    public string SolvePart1(string input)
        => $"{Solver(input, (int[][] grid, int y, int x, int n) => ExploreP1(grid, y, x, n).Count)}";

    public string SolvePart2(string input)
        => $"{Solver(input, ExploreP2)}";
}