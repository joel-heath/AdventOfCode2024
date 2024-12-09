using AdventOfCode2024.Utilities;

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

    static readonly Point[] xmasDestinations = [(3, 0), (0, 3), (-3, 0), (0, -3), (3, 3), (-3, 3), (-3, -3), (3, -3)];
    static int CountXMAS(long i, long j, Grid<char> grid)
        => xmasDestinations.Count(d => string.Concat(grid.LineTo((i, j), (i, j) + d)) == "XMAS");

    static readonly (Point, Point)[] x_masDiagonals = [((-1, -1), (1, 1)), ((-1, 1), (1, -1)), ((1, -1), (-1, 1)), ((1, 1), (-1, -1))];
    static int CountX_MAS(long i, long j, Grid<char> grid)
        => x_masDiagonals.Count(d => string.Concat(grid.LineTo((i, j) + d.Item1, (i, j) + d.Item2)) == "MAS") == 2 ? 1 : 0;

    public string SolvePart1(string input)
    {
        var grid = Grid<char>.FromString(input);
        return $"{grid.AllPositions().Sum(p => CountXMAS(p.X, p.Y, grid))}";
    }

    public string SolvePart2(string input)
    {
        var grid = Grid<char>.FromString(input);
        return $"{grid.AllPositions().Sum(p => CountX_MAS(p.X, p.Y, grid))}";
    }
}