using AdventOfCode2024.Utilities;

namespace AdventOfCode2024;

public class Day25 : IDay
{
    public int Day => 25;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "#####\r\n.####\r\n.####\r\n.####\r\n.#.#.\r\n.#...\r\n.....\r\n\r\n#####\r\n##.##\r\n.#.##\r\n...##\r\n...#.\r\n...#.\r\n.....\r\n\r\n.....\r\n#....\r\n#....\r\n#...#\r\n#.#.#\r\n#.###\r\n#####\r\n\r\n.....\r\n.....\r\n#.#..\r\n###..\r\n###.#\r\n###.#\r\n#####\r\n\r\n.....\r\n.....\r\n.....\r\n#....\r\n#.#..\r\n#.#.#\r\n#####", "3" },
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "Could you bring the chronicle Santa requested up to Santa?", "Deliver the chronicle." }
    };

    public string SolvePart1(string input)
    {
        long summation = 0;

        var schematics = input.Split(Environment.NewLine + Environment.NewLine);

        var data = schematics
            .Select(schem => schem.Split(Environment.NewLine))
            .Select(rows => (rows, isKey: rows[0].Count(c => c == '#') == 0))
            .Select(data => (data.rows
                .Transpose()
                .Select(column => column.Count(c => c == '#'))
                .ToArray(), data.isKey))
            .ToArray();

        var fullKeyLength = schematics
            .Select(schem => schem.Split(Environment.NewLine)
                .Transpose()
                .First().Length).First();

        var keyLockFits = 0;
        foreach (var (@lock, _) in data.Where(s => !s.isKey))
        {
            foreach (var (key, _) in data.Where(s => s.isKey))
            {
                if (@lock.Zip(key).All(t => t.First + t.Second <= fullKeyLength))
                {
                    keyLockFits++;
                }
            }
        }

        return $"{keyLockFits}";
    }

    public string SolvePart2(string input) => "Deliver the chronicle.";
}