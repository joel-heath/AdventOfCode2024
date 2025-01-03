namespace AdventOfCode2024;

public class Day10 : IDay
{
    public int Day => 10;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "0123\r\n1234\r\n8765\r\n9876", "1" },
        { "...0...\r\n...1...\r\n...2...\r\n6543456\r\n7.....7\r\n8.....8\r\n9.....9", "2" },
        { "..90..9\r\n...1.98\r\n...2..7\r\n6543456\r\n765.987\r\n876....\r\n987....", "4" },
        { "10..9..\r\n2...8..\r\n3...7..\r\n4567654\r\n...8..3\r\n...9..2\r\n.....01", "3" },
        { "89010123\r\n78121874\r\n87430965\r\n96549874\r\n45678903\r\n32019012\r\n01329801\r\n10456732", "36" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { ".....0.\r\n..4321.\r\n..5..2.\r\n..6543.\r\n..7..4.\r\n..8765.\r\n..9....", "3" },
        { "..90..9\r\n...1.98\r\n...2..7\r\n6543456\r\n765.987\r\n876....\r\n987....", "13" },
        { "012345\r\n123456\r\n234567\r\n345678\r\n4.6789\r\n56789.", "227" },
        { "89010123\r\n78121874\r\n87430965\r\n96549874\r\n45678903\r\n32019012\r\n01329801\r\n10456732", "81" }
    };

    private static (int y, int x)[] Explore(int[][] grid, int y, int x, int n)
        => y < 0 || y >= grid.Length || x < 0 || x >= grid[0].Length || grid[y][x] != n
            ? [] : n == 9 ? [(y, x)] : [
                ..Explore(grid, y + 1, x, n + 1),
                ..Explore(grid, y - 1, x, n + 1),
                ..Explore(grid, y, x + 1, n + 1),
                ..Explore(grid, y, x - 1, n + 1),
            ];

    private static int Solver(string input, Func<(int y, int x)[], int> counter)
    {
        var grid = input.Split(Environment.NewLine).Select(z => z.Select(x => x - '0').ToArray()).ToArray();
        return grid.SelectMany((r, i) => r.Select((c, j) => counter(Explore(grid, i, j, 0)))).Sum();
    }

    public string SolvePart1(string input)
        => $"{Solver(input, ((int y, int x)[] points) => points.Distinct().Count())}";

    public string SolvePart2(string input)
        => $"{Solver(input, ((int y, int x)[] points) => points.Length)}";
}