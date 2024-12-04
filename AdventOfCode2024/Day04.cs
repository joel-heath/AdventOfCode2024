namespace AdventOfCode2024;

public class Day04 : IDay
{
    public int Day => 4;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "MMMSXXMASM\r\nMSAMXMSMSA\r\nAMXSXMAAMM\r\nMSAMASMSMX\r\nXMASAMXAMM\r\nXXAMMXXAMA\r\nSMSMSASXSS\r\nSAXAMASAAA\r\nMAMMMXMMMM\r\nMXMXAXMASX", "18" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "MMMSXXMASM\r\nMSAMXMSMSA\r\nAMXSXMAAMM\r\nMSAMASMSMX\r\nXMASAMXAMM\r\nXXAMMXXAMA\r\nSMSMSASXSS\r\nSAXAMASAAA\r\nMAMMMXMMMM\r\nMXMXAXMASX", "9" }
    };

    static int CountXMAS(int i, int j, Grid<char> grid)
    {
        Point[] destinations = { (i + 3, j), (i, j + 3), (i - 3, j), (i, j - 3), (i + 3, j + 3), (i - 3, j + 3), (i - 3, j - 3), (i + 3, j - 3) };
        return destinations.Count(d => string.Concat(grid.LineTo((i, j), d)) == "XMAS");
    }

    static int CountX_MAS(int i, int j, Grid<char> grid)
    {
        (Point, Point)[] diagonals = [((-1, -1), (1, 1)), ((-1, 1), (1, -1)), ((1, -1), (-1, 1)), ((1, 1), (-1, -1))];
        return diagonals.Count(d => string.Concat(grid.LineTo((i, j) + d.Item1, (i, j) + d.Item2)) == "MAS") == 2 ? 1 : 0;
    }

    public string SolvePart1(string input)
    {
        long count = 0;

        var grid = new Grid<char>(input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray());
        for (int i = 0; i < grid.Height; i++)
        {
            for (int j = 0; j < grid.Width; j++)
            {
                count += CountXMAS(i, j, grid);
            }
        }

        return $"{count}";
    }

    public string SolvePart2(string input)
    {
        long count = 0;

        var grid = new Grid<char>(input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray());
        for (int i = 0; i < grid.Height; i++)
        {
            for (int j = 0; j < grid.Width; j++)
            {
                count += CountX_MAS(i, j, grid);
            }
        }

        return $"{count}";
    }
}