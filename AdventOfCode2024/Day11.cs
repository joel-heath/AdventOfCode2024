using AdventOfCode2024.Utilities;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2024;

public class Day11 : IDay
{
    public int Day => 11;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "125 17", "55312" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "125 17", "55312" }
    };

    public string SolvePart1(string input)
    {
        memo = [];
        var stones = input.Split(' ').Select(long.Parse)
            .Select(stone => Task.Run(async () => await StoneSorter(stone, 25)))
            .ToArray();

        Task.WhenAll(stones).Wait();

        return $"{stones.Sum(t => t.Result)}";
    }

    private static ConcurrentDictionary<(long stone, int i), long> memo = [];

    private static async Task<long> StoneSorter(long stone, int i)
    {
        if (i == 0)
            return 1;

        if (memo.ContainsKey((stone, i)))
            return memo[(stone, i)];

        long @return;
        if (stone == 0)
        {
            @return = await StoneSorter(1, i - 1);
        }
        else if (stone.ToString().Length % 2 == 0)
        {
            var str = stone.ToString();
            string[] halves = [str[..(str.Length / 2)], str[(str.Length / 2)..]];
            var tasks = halves.Select(long.Parse).Select(s => Task.Run(async () => await StoneSorter(s, i - 1))).ToArray();
            await Task.WhenAll(tasks);

            @return = tasks.Sum(t => t.Result);
        }
        else
        {
            @return = await StoneSorter(stone * 2024, i - 1);
        }

        memo[(stone, i)] = @return;
        return @return;
    }


    public string SolvePart2(string input)
    {
        memo = [];
        var stones = input.Split(' ').Select(long.Parse)
            .Select(stone => Task.Run(async () => await StoneSorter(stone, 75)))
            .ToArray();

        Task.WhenAll(stones).Wait();

        return $"{stones.Sum(t => t.Result)}";
    }
}