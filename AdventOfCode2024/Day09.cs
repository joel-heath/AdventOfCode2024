using AdventOfCode2024.Utilities;

namespace AdventOfCode2024;

public class Day09 : IDay
{
    public int Day => 9;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "2333133121414131402", "1928" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "2333133121414131402", "2858" },
        { "221221311512411", "578" }
    };

    private static List<(int id, int blocks, int free)> ParseInput(string input)
        => input.ToCharArray()
            .Chunk(2)
            .Select((c, i) => (id: i, blocks: c[0] - '0', free: c.Length > 1 ? c[1] - '0' : 0))
            .ToList();

    public string SolvePart1(string input)
    {
        var blocks = ParseInput(input);

        for (int i = 0; i < blocks.Count - 1; i++)
        {
            var right = blocks[^1];
            var left = blocks[i];

            if (left.free >= right.blocks)
            {
                var remainingFree = left.free - right.blocks;
                blocks.Insert(i + 1, (right.id, right.blocks, remainingFree));
                blocks.RemoveAt(blocks.Count - 1);
            }
            else
            {
                blocks.Insert(i + 1, (right.id, left.free, 0));
                blocks[^1] = (right.id, right.blocks - left.free, 0);
                i++;
            }
        }

        return $"{blocks.SelectMany(b => Enumerable.Repeat(b.id, b.blocks))
            .Select((l, i) => (long)l * i).Sum()}";
    }

    public string SolvePart2(string input)
    {
        var blocks = ParseInput(input);

        for (int i = blocks.Count - 1; i > 0;)
        {
            var right = blocks[i];
            var prev = blocks[i - 1];
            var left = blocks.Take(i)
                .Select((b, i) => (index: i, b.id, b.blocks, b.free))
                .FirstOrDefault(b => b.free >= right.blocks, (index: -1, id: -1, blocks: -1, free: -1));

            if (left.index == -1)
            {
                i--; continue;
            }

            blocks[i - 1] = (prev.id, prev.blocks, prev.free + right.blocks + right.free);
            if (left.index == i - 1)
                left = (left.index, left.id, left.blocks, blocks[i - 1].free);

            blocks.RemoveAt(i);
            blocks[left.index] = (left.id, left.blocks, 0);
            blocks.Insert(left.index + 1, (right.id, right.blocks, left.free - right.blocks));
        }

        return $"{blocks.SelectMany(b => Enumerable.Repeat(b.id, b.blocks).Concat(Enumerable.Repeat(0, b.free)))
            .Select((l, i) => (long)l * i).Sum()}";
    }


    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "One-liners are funny")]
    private static string SolveP1OneLine(string input)
        => $"{input.ToCharArray().Chunk(2)
            .Select((c, i) => (id: i, blocks: c[0] - '0', free: c.Length > 1 ? c[1] - '0' : 0))
            .AssignList(out var blocksInit).WhileTrue()
            .AggregateWhile(
                (i: 0, blocks: blocksInit),
                (a, _) =>
                    a.blocks[a.i].Assign(out var left).free >= a.blocks[^1].Assign(out var right).blocks
                        ? (a.i + 1, [.. a.blocks[..(a.i + 1)], (right.id, right.blocks, left.free - right.blocks), ..a.blocks[(a.i + 1)..^1]])
                        : (a.i + 2, [.. a.blocks[..(a.i + 1)], (right.id, left.free, 0), .. a.blocks[(a.i + 1)..^1], (right.id, right.blocks - left.free, 0)]),
                a => a.i < a.blocks.Count - 1)
            .blocks
            .SelectMany(b => Enumerable.Repeat(b.id, b.blocks))
            .Select((l, i) => (long)l * i).Sum()}";

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "One-liners are funny")]
    private static string SolveP2OneLine(string input)

        => $"{input.ToCharArray().Chunk(2)
            .Select((c, i) => (id: i, blocks: c[0] - '0', free: c.Length > 1 ? c[1] - '0' : 0))
            .AssignList(out var blocks).WhileTrue()
            .AggregateWhile(
                (i: blocks.Count - 1, blocks),
                (a, _) => a.blocks
                    .Take(a.i)
                    .Select((b, i) => (index: i, b.id, b.blocks, b.free))
                    .FirstOrDefault(b => b.free >= a.blocks[a.i].blocks, (index: -1, id: -1, blocks: -1, free: -1)).Assign(out var left).index == -1
                        ? (a.i - 1, a.blocks)
                        : (a.i, [
                            ..a.blocks[..left.index],                                                                                                         // .. left index
                                ..(left.index < a.i - 1
                                    ? [                                                                                                                       /// left index <  i - 1                     
                                        (left.id, left.blocks, 0),                                                                                            // left index
                                        (a.blocks[a.i].Assign(out var right).id, right.blocks, left.free - right.blocks),                                     // INSERT left index + 1
                                        ..a.blocks[(left.index + 1)..(a.i - 1)], // +1 rather than +2 because INSERT not replace                              // left index + 1 .. i - 1
                                        (a.blocks[a.i - 1].Assign(out var prev).id, prev.blocks, prev.free + right.blocks + right.free)                       // i - 1                                                                                                                          // OUTSERT i
                                    ] : new List<(int id, int blocks, int free)>() {                                                                          /// left index == i - 1                       
                                        (left.id, left.blocks, 0),                                                                                            // left index OVERWRITES i - 1
                                                                                                                                                              // OUTSERT i
                                        (a.blocks[a.i].Assign(out right).id, right.blocks, a.blocks[a.i - 1].free + right.blocks + right.free - right.blocks) // INSERT left index + 1 == i
                                    }),
                                ..a.blocks[(a.i + 1)..]                                                                                                       // i + 1 ..
                        ]),
                a => a.i > 0)
            .blocks
            .SelectMany(b => Enumerable.Repeat(b.id, b.blocks).Concat(Enumerable.Repeat(0, b.free)))
            .Select((l, i) => (long)l * i).Sum()}";
}