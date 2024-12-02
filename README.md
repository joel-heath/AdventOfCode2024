# Advent of Code 2024
My C# solutions to [Advent of Code 2024](https://adventofcode.com/2024).

## Set-up
If you'd like to run my solutions on your input, you can clone this repo, and either manually create the file `Inputs/Day{n}.txt`, or alternatively you can run `dotnet user-secrets set SessionToken your-aoc-session-token`, and your input will be fetched automatically.

This project is using `.NET 9.0`.

## Notes
Here you can easily navigate each days code and read about how well I think I did.

### Legend
🟢 The quintessential one-liner. \
🟡 Short, succinct code. \
🟠 Average solution that is unreduced. \
🔴 A poorer solution than most out there. \
⚫ Unsolved (probably because the problem isn't out yet, or I forgot to push).

| **Day** | **Verbosity** | **Notes** |
|:---:|:---:|:---:|
| [1](AdventOfCode2024/Day01.cs) | 🟡 | Today's was undeserving of the one-liner status, due to the handling of two lists in parallel. Otherwise, a simple start to this year's puzzles. |
| [2](AdventOfCode2024/Day02.cs) | 🟢 | My approach for part two was just to create all possible subsets of size `n-1` and see if any of them are valid. |
| [3](AdventOfCode2024/Day03.cs) | 🟡 | Part one was a nice one-liner but unfortunately not part 2. My approach was to use Regex groups to extract all the numbers and find the indices of the `do()`s and `don't()`s. |
| [4](AdventOfCode2024/Day04.cs) | ⚫ |  |
| [5](AdventOfCode2024/Day05.cs) | ⚫ |  |
| [6](AdventOfCode2024/Day06.cs) | ⚫ |  |
| [7](AdventOfCode2024/Day07.cs) | ⚫ |  |
| [8](AdventOfCode2024/Day08.cs) | ⚫ |  |
| [9](AdventOfCode2024/Day09.cs) | ⚫ |  |
| [10](AdventOfCode2024/Day10.cs) | ⚫ |  |
| [11](AdventOfCode2024/Day11.cs) | ⚫ |  |
| [12](AdventOfCode2024/Day12.cs) | ⚫ |  |
| [13](AdventOfCode2024/Day13.cs) | ⚫ |  |
| [14](AdventOfCode2024/Day14.cs) | ⚫ |  |
| [15](AdventOfCode2024/Day15.cs) | ⚫ |  |
| [16](AdventOfCode2024/Day16.cs) | ⚫ |  |
| [17](AdventOfCode2024/Day17.cs) | ⚫ |  |
| [18](AdventOfCode2024/Day18.cs) | ⚫ |  |
| [19](AdventOfCode2024/Day19.cs) | ⚫ |  |
| [20](AdventOfCode2024/Day20.cs) | ⚫ |  |
| [21](AdventOfCode2024/Day21.cs) | ⚫ |  |
| [22](AdventOfCode2024/Day22.cs) | ⚫ |  |
| [23](AdventOfCode2024/Day23.cs) | ⚫ |  |
| [24](AdventOfCode2024/Day24.cs) | ⚫ |  |
| [25](AdventOfCode2024/Day25.cs) | ⚫ |  |