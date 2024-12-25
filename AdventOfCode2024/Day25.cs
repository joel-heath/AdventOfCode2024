using AdventOfCode2024.Utilities;

namespace AdventOfCode2024;

public class Day25 : IDay
{
    public int Day => 25;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "#####\r\n.####\r\n.####\r\n.####\r\n.#.#.\r\n.#...\r\n.....\r\n\r\n#####\r\n##.##\r\n.#.##\r\n...##\r\n...#.\r\n...#.\r\n.....\r\n\r\n.....\r\n#....\r\n#....\r\n#...#\r\n#.#.#\r\n#.###\r\n#####\r\n\r\n.....\r\n.....\r\n#.#..\r\n###..\r\n###.#\r\n###.#\r\n#####\r\n\r\n.....\r\n.....\r\n.....\r\n#....\r\n#.#..\r\n#.#.#\r\n#####", "3" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "Could you bring the chronicle Santa requested up to Santa?", "Deliver the chronicle." }
    };

    public string SolvePart1(string input)
    {
        (int[] schem, bool isKey, int totalHeight)[] schematics = input
            .Split(Environment.NewLine + Environment.NewLine)
            .Select(schem => schem.Split(Environment.NewLine))
            .Select(rows => (rows, isKey: !rows[0].Any(c => c == '#')))
            .Select(data => (data: data.rows
                .Transpose()
                .Select(column => (pinLength: column.Count(c => c == '#'), totalHeight: column.Length))
                .ToArray(), data.isKey))
            .Select(data => (data.data.Select(c => c.pinLength).ToArray(), data.isKey, data.data[0].totalHeight))
            .ToArray();

        return $"{schematics.Where(s => !s.isKey)
            .Select(@lock => schematics.Where(s => s.isKey)
                .Count(key => @lock.schem.Zip(key.schem).All(t => t.First + t.Second <= @lock.totalHeight)))
            .Sum()}";
    }

    public string SolvePart2(string input) => "Deliver the chronicle.";
}