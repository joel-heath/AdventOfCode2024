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

    public string SolvePart1(string input)
    {
        var blocks = input.ToCharArray().Chunk(2).Select((c, i) => (id: i, blocks: c[0] - '0', free: c.Length > 1 ? c[1] - '0' : 0)).ToList();

        for (int i = 0; i < blocks.Count - 1; i++)
        {
            var right = blocks[^1];
            var left = blocks[i];

            if (left.free >= right.blocks)
            {
                var remainingFree = left.free - right.blocks;
                blocks.Insert(i+1, (right.id, right.blocks, remainingFree));
                blocks.RemoveAt(blocks.Count - 1);
            }
            else
            {
                blocks.Insert(i+1, (right.id, left.free, 0));
                blocks[^1] = (right.id, right.blocks - left.free, 0);
                i++;
            }
        }

        return $"{blocks.SelectMany(b => Enumerable.Repeat(b.id, b.blocks)).Select((l, i) => (long)l * i).Sum()}";
    }

    public string SolvePart2(string input)
    {
        var blocks = input.ToCharArray().Chunk(2).Select((c, i) => (id: i, blocks: c[0] - '0', free: c.Length > 1 ? c[1] - '0' : 0)).ToList();

        for (int i = blocks.Count - 1; i > 0;)
        {
            var right = blocks[i];
            var prev = blocks[i - 1];
            var left = blocks.Take(i).Select((b, i) => (index: i, b.id, b.blocks, b.free))
                .FirstOrDefault(b => b.free >= right.blocks, (index: -1, id: -1, blocks: -1, free: -1));

            if (left.index == -1)
            {
                i--;
                continue;
            }

            blocks[i - 1] = (prev.id, prev.blocks, prev.free + right.blocks + right.free);
            if (left.index == i - 1)
                left = (left.index, left.id, left.blocks, blocks[i - 1].free);

            blocks.RemoveAt(i);

            blocks[left.index] = (left.id, left.blocks, 0);
            var remainingFree = left.free - right.blocks;
            blocks.Insert(left.index + 1, (right.id, right.blocks, remainingFree));
        }

        return $"{blocks.SelectMany(b => Enumerable.Repeat(b.id, b.blocks).Concat(Enumerable.Repeat(0, b.free))).Select((l, i) => (long)l * i).Sum()}";
    }
}