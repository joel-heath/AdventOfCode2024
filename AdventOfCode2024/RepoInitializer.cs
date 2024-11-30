namespace AdventOfCode2024;

public static class RepoInitializer
{
    public static async Task InitializeRepo()
    {
        var workingDirectory = Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!;
        string path = workingDirectory.FullName;
        var tasks = new Task[26];
        for (int i = 1; i <= 25; i++)
        {
            using StreamWriter dayWriter = new(new FileStream(Path.Join(path, $"Day{i:00}.cs"), FileMode.Create));
            tasks[i-1] = dayWriter.WriteAsync(DayCode.Replace("{{DAY}}", $"{i}").Replace("{{DAY_2D}}", $"{i:00}"));
        }

        using StreamWriter readmeWriter = new(new FileStream(Path.Join(workingDirectory.Parent!.FullName, $"README.md"), FileMode.Create));
        tasks[25] = readmeWriter.WriteAsync(Readme.Replace("{{PROJECT_NAME}}", Program.ProjectName).Replace("{{YEAR}}", Program.Year));

        await Task.WhenAll(tasks);
    }

    private static readonly string DayCode =
$@"namespace {typeof(Program).Namespace}" + @";

public class Day{{DAY_2D}} : IDay
{
    public int Day => {{DAY}};
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { ""TestInput1"", ""ExpectedOutput1"" },
        { ""TestInput2"", ""ExpectedOutput2"" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { ""TestInput1"", ""ExpectedOutput1"" },
        { ""TestInput2"", ""ExpectedOutput2"" }
    };

    public string SolvePart1(string input)
    {
        return string.Empty;
    }

    public string SolvePart2(string input)
    {
        return string.Empty;
    }
}";

    private static readonly string Readme = @"# Advent of Code {{YEAR}}
My C# solutions to [Advent of Code {{YEAR}}](https://adventofcode.com/{{YEAR}}).

## Set-up
If you'd like to run my solutions on your input, you can clone this repo, and either manually create the file Inputs/Day{n}.txt, or alternatively you can run `dotnet user-secrets set SessionToken your-aoc-session-token`, and your input will be fetched automatically.

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
| [1]({{PROJECT_NAME}}/Day01.cs) | ⚫ |  |
| [2]({{PROJECT_NAME}}/Day02.cs) | ⚫ |  |
| [3]({{PROJECT_NAME}}/Day03.cs) | ⚫ |  |
| [4]({{PROJECT_NAME}}/Day04.cs) | ⚫ |  |
| [5]({{PROJECT_NAME}}/Day05.cs) | ⚫ |  |
| [6]({{PROJECT_NAME}}/Day06.cs) | ⚫ |  |
| [7]({{PROJECT_NAME}}/Day07.cs) | ⚫ |  |
| [8]({{PROJECT_NAME}}/Day08.cs) | ⚫ |  |
| [9]({{PROJECT_NAME}}/Day09.cs) | ⚫ |  |
| [10]({{PROJECT_NAME}}/Day10.cs) | ⚫ |  |
| [11]({{PROJECT_NAME}}/Day11.cs) | ⚫ |  |
| [12]({{PROJECT_NAME}}/Day12.cs) | ⚫ |  |
| [13]({{PROJECT_NAME}}/Day13.cs) | ⚫ |  |
| [14]({{PROJECT_NAME}}/Day14.cs) | ⚫ |  |
| [15]({{PROJECT_NAME}}/Day15.cs) | ⚫ |  |
| [16]({{PROJECT_NAME}}/Day16.cs) | ⚫ |  |
| [17]({{PROJECT_NAME}}/Day17.cs) | ⚫ |  |
| [18]({{PROJECT_NAME}}/Day18.cs) | ⚫ |  |
| [19]({{PROJECT_NAME}}/Day19.cs) | ⚫ |  |
| [20]({{PROJECT_NAME}}/Day20.cs) | ⚫ |  |
| [21]({{PROJECT_NAME}}/Day21.cs) | ⚫ |  |
| [22]({{PROJECT_NAME}}/Day22.cs) | ⚫ |  |
| [23]({{PROJECT_NAME}}/Day23.cs) | ⚫ |  |
| [24]({{PROJECT_NAME}}/Day24.cs) | ⚫ |  |
| [25]({{PROJECT_NAME}}/Day25.cs) | ⚫ |  |";
}