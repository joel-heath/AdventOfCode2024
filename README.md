# Advent of Code 2024
My C# solutions to [Advent of Code 2024](https://adventofcode.com/2024).

## Set-up
If you'd like to run my solutions on your input, you can clone this repo, and either manually create the file `Inputs/Day{n}.txt`, or alternatively you can run `dotnet user-secrets set SessionToken your-aoc-session-token`, and your input will be fetched automatically.

This project is using `.NET 9.0`.

## Notes
Here you can easily navigate each days code and read about how well I think I did.

### Legend
🔵 The quintessential one-liner. \
🟢 A couple variable initialisations, perhaps a function, and a one-liner return. \
🟡 Short, succinct code. \
🟠 Average solution with some code unreduced. \
🔴 A disgusting typical imperative solution. \
⚫ Unsolved (probably because the problem isn't out yet, or I forgot to push).

| **Day** | **Verbosity** | **Notes** |
|:---:|:---:|:---:|
| [1](AdventOfCode2024/Day01.cs) | 🟢 | Today's was undeserving of the one-liner status, due to the handling of two lists in parallel. Otherwise, a simple start to this year's puzzles. |
| [2](AdventOfCode2024/Day02.cs) | 🔵 | My approach for part two was just to create all possible subsets of size `n-1` and see if any of them are valid. |
| [3](AdventOfCode2024/Day03.cs) | 🟢 | Part one was a nice one-liner but unfortunately not part 2. My approach was to use Regex groups to extract all the numbers and find the indices of the `do()`s and `don't()`s. |
| [4](AdventOfCode2024/Day04.cs) | 🟢 | Very happy with today's solution, using my `Grid` class and it's `LineTo()` function (though I had to completely rewrite it). |
| [5](AdventOfCode2024/Day05.cs) | 🟢 | Today's allowed me to make use of my custom `AggregateWhile()` function which is exciting, along with `EnumerateForever()` which sounds a bit silly but makes sense alongside `AggregateWhile()`. |
| [6](AdventOfCode2024/Day06.cs) | 🟡 | I brute-forced part 2, limiting my options to only part 1's solution and used some parallelisation to improve speed. |
| [7](AdventOfCode2024/Day07.cs) | 🔵 | Today I am overjoyed as I don't have to make very inefficient, unreadable and unnecessary decisions to force my solution into one line (which I did much of in [2023](https://github.com/joel-heath/AdventOfCode2023/blob/master/AdventOfCode2023/Day20.cs)). My part 2 took about 16s, and I got that down to 5s with parallelisation. |
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