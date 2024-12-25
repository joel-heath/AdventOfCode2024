# Advent of Code 2024
My C# solutions to [Advent of Code 2024](https://adventofcode.com/2024).

## Set-up
If you'd like to run my solutions on your input, you can clone this repo, and either manually create the file `Inputs/Day{n}.txt`, or alternatively you can run `dotnet user-secrets set SessionToken your-aoc-session-token`, and your input will be fetched automatically.

This project is using `.NET 9.0`.

## Notes
Here you can easily navigate each days code and read about how well I think I did.

My primary goal in previous years has been to write one-liners, and having done that for some time I now realise with my suite of custom extension methods, anything can be turned into one line.

So my goal this year is to write short, clean, and efficient code, with pure LINQ queries. This means sometimes having some lines for variable initialisations, then a one-liner, and other times means I will create two solutions: one that is (hopefully) fast and readable, the other being a one-liner for the challenge.

### Legend
🟣 The quintessential one-liner. Each LINQ query is pure and there's no use of my goofy custom extension methods. \
🔵 A one-liner that uses my weird Extension Methods and is probably pretty unreadable, hopefully comes with a normal solution as well. \
🟢 A couple variable initialisations, perhaps a function, and a one-liner return. \
🟡 Short, succinct code. \
🟠 An average, mostly imperative solution. \
🔴 An inefficient / heuristic-based solution (in my opinion heuristics are cop-outs). \
⚫ Unsolved (probably because the problem isn't out yet, or I forgot to push).

| **Day** | **Verbosity** | **Notes** |
|:---:|:---:|:---:|
| [1](AdventOfCode2024/Day01.cs) | 🟢 | Failing to achieve the prestigious "quintessential one-liner" title on day 1 might seem pretty horrific, but that's because you didn't read my [Notes](#Notes). Due to the handling of two lists in parallel, it is wise to store the parsed data in a variable, then calculating the result. A simple start to this year's puzzles. |
| [2](AdventOfCode2024/Day02.cs) | 🟣 | My approach for part two was just to create all possible subsets of size `n-1` and see if any of them are valid. |
| [3](AdventOfCode2024/Day03.cs) | 🟢 | Part one was a nice one-liner but unfortunately not part 2. My approach was to use Regex groups to extract all the numbers and find the indices of the `do()`s and `don't()`s. |
| [4](AdventOfCode2024/Day04.cs) | 🟢 | Very happy with today's solution, using my `Grid` class and it's `LineTo()` function (though I had to completely rewrite it). |
| [5](AdventOfCode2024/Day05.cs) | 🟢 | Today's allowed me to make use of my custom `AggregateWhile()` function which is exciting, along with `EnumerateForever()` which sounds a bit silly but makes sense alongside `AggregateWhile()`. |
| [6](AdventOfCode2024/Day06.cs) | 🟡 | I brute-forced part 2, with two optimisations: limiting my options for the extra obstacle to only part 1's solution, and using multithreading for each scenario. |
| [7](AdventOfCode2024/Day07.cs) | 🟣 | Today I am overjoyed as I don't have to make very inefficient, unreadable and unnecessary decisions to force my solution into one line (which I did much of in [2023](https://github.com/joel-heath/AdventOfCode2023/blob/master/AdventOfCode2023/Day20.cs)). My part 2 takes ~~16s~~, ~~5s with parallelisation.~~ 1.5s with parallelisation & memoisation. |
| [8](AdventOfCode2024/Day08.cs) | 🟣 | I solved today's by grouping the antenna by frequency, taking each pair and calculating the antinode positions. For part 2 I used again `EnumerateForever()`, this time with a `TakeWhile()` to get all antinodes until exiting the map. |
| [9](AdventOfCode2024/Day09.cs) | 🔵 | I relapsed and wrote another atrocious one-liner. This year, however, I will not stand for impure uses of LINQ, so rather than modifying an external list, I stuck it in the accumulator, creating a new list each time. This truly does demonstrate that my one-liners are both harder to read and slower to execute. This fact will not stop me from writing them though. |
| [10](AdventOfCode2024/Day10.cs) | 🟢 | A very tame day 10; I, like many, managed to accidentally code part 2 while trying to figure out part 1, so as soon as I read it it was a rush to undo. I created a method that returns all reachable 9s, part one runs `.Distinct()`, and part 2 doesn't. |
| [11](AdventOfCode2024/Day11.cs) | 🟢 | Memoisation is enough to get part 2 in 50ms, though I initially implemented them storing the whole stones array. My computer came screeching to a halt at 120GB of RAM usage, and I realised I could just stone the length. |
| [12](AdventOfCode2024/Day12.cs) | 🟡 | Today, I used flood-fill for areas, counted all neighbours of points that are outside the specified points for the perimeter, and for the sides I accumulated all the vertical and horizontal sides into lists, then counted only the ones that differed from the previous by >1 to determine if they were continuous. |
| [13](AdventOfCode2024/Day13.cs) | 🟣 | I determined the number of button presses by solving the system of linear equations using their matrix equation (Cramer's rule), the only thing left to do is test if the results are integers. |
| [14](AdventOfCode2024/Day14.cs) | 🟢 | Today's was... weird. Initially I thought the 'tree' would be the one we've seen on the advent calendar, then I thought it was any NxM tree in the AoC style. Eventually I thought to check for when the robots don't overlap, which has NO RIGHT IN BEING THE CORRECT SOLUTION. I hear minimising the safety factor also works but I'm not swapping out one illogical solution for another. |
| [15](AdventOfCode2024/Day15.cs) | 🟠 | Today's part 2 seemed so innocent at first then slowly revealed it's true colours after many fails at the test input. My approach for part 2 was to recur my block moving function, at the leaf nodes returning true and causing all the blocks to be moved. Unfortunately that means one branch won't know if another hit a wall and the blocks will be already moved, so I created a copy of the blocks collection, then discarded it if the top-level call returned false. |
| [16](AdventOfCode2024/Day16.cs) | 🟠 | My part 2 solution takes just over 3 seconds, so it could be better. My approach was to store the history of points traversed on each visit, and when reaching the end with the same number of points as the best so far, performing a set union on a stored variable for all best path positions, and this visit's history. |
| [17](AdventOfCode2024/Day17.cs) | 🟠 | What an exciting part 2! Unfortunately, my solution is hard-coded to my input, I might revisit expanding it to be generic. The program is an iterative structure, each iteration focusing on the next 3 bits of A, back to front. My method was to take each iteration and test all possibilities for 3-bit values for if they produce the right output. Then, recursively continue for all possibilities and take the smallest at the end. |
| [18](AdventOfCode2024/Day18.cs) | 🟡 | Eric was clearly asking for forgiveness for the previous days of horror, because today was a very light task. I used a binary search for part 2 because figuring out how to correct the path on each iteration seemed much harder, and the binary search runs in < 250ms. |
| [19](AdventOfCode2024/Day19.cs) | 🟢 | A second, very tame puzzle, clearly Eric felt he required more forgiveness. My approach for both parts was as simple as iterate through the available designs and see if any of them get us any closer to creating the required towel, if they do, return true if the towel is complete, recur if not. The only optimisation required is memoisation. |
| [20](AdventOfCode2024/Day20.cs) | 🟡 | In my opinion today's deserves a better verbosity level, the only thing keeping it from a 🟢 rating is my Dijkstra's implementation. My approach was to perform a full Dijkstra's from the start and the end point, getting the all the distances from every point to the start and finish. Then, it's a matter of iterating through each spot in the map, and performing all possible cheats. Getting the resulting path's length is O(1) since the two Dijkstra's have already been performed. |
| [21](AdventOfCode2024/Day21.cs) | 🟡 | I'll be honest this one barely scrapes it's rating, but it is indeed mostly just variable initialisations and one-line processing. *I hear* many people solved it by considering multiple routes between any two points and simply picking the shorter; I, however, figured out the closed formula for the optimal path between any two points, so my method is to apply that formula between all pairs of points, then recursively iterate down the robotic chain with a memo. |
| [22](AdventOfCode2024/Day22.cs) | 🟡 | Honestly just very surprised I managed to get the second star, I expected that regular optimisations wouldn't cut it and I'd need a complete rewrite, but simply by ensuring things were only evaluated once with dictionaries and sets, I was able to get my part 2 to 9 seconds on the Release configuration. |
| [23](AdventOfCode2024/Day23.cs) | 🟡 | My approach for part 2 was to get all 3-cycles from part 1, get all the nodes connected to those 3, and iterate through adding them to the collection as long they connect to all the extra nodes added so far. |
| [24](AdventOfCode2024/Day24.cs) | 🟢 | My method for part 2 is to iterate through each connection and perform a load of checks based on the recurrence relation for output bits, if any fail then that node is included in the output. Very glad I chose to go about it this way as it lends itself nicely to reducing to a one-liner. |
| [25](AdventOfCode2024/Day25.cs) | 🟢 | I solved the puzzle in the same way I read it, divided the schematics into locks and keys, counted the number of cells taken up by the schematic in each column, then paired every lock with every key and checked if the sum of the number of cells taken up exceeds the total length of column. |

Merry Christmas!