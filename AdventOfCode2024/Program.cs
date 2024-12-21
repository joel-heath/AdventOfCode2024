using Microsoft.Extensions.Configuration;
using System.Reflection;
using TextCopy;
using AdventOfCode2024.Utilities;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode2024;

internal class Program
{
    public static readonly string ProjectName = Assembly.GetCallingAssembly().GetName().Name!; // AdventOfCode[YEAR]
    public static readonly string Year = ProjectName[^4..];

    static async Task<string> FetchInput(int day)
    {
        var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        var sessionToken = config["SessionToken"] ?? throw new NotSupportedException($"No session token available to get input. Please manually provide the problem input at \"\\Inputs\\Day{day}.txt\"");
        string responseBody;
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("Fetching input...");
        using (var handler = new HttpClientHandler { UseCookies = false })
        using (var client = new HttpClient(handler))
        {
            var message = new HttpRequestMessage(HttpMethod.Get, $"https://adventofcode.com/{Year}/day/{day}/input");
            message.Headers.Add("Cookie", $"session={sessionToken}");
            var result = await client.SendAsync(message);
            try
            {
                result.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                throw new NotSupportedException($"Couldn't get problem input, maybe your session token is old. Please manually provide the problem input at \"\\Inputs\\Day{day}.txt\"", e);
            }
            responseBody = await result.Content.ReadAsStringAsync();
        }

        Console.WriteLine("Input successfully fetched");
        return responseBody.ReplaceLineEndings().TrimEnd();
    }

    static string FindSolutionPath()
    {
        var msg = "Cannot find solution directory";
        var directory = Directory.GetParent(AppContext.BaseDirectory) ?? throw new DirectoryNotFoundException(msg);
        while (directory.Name != ProjectName)
        {
            directory = directory.Parent ?? throw new DirectoryNotFoundException(msg);
        }

        return (directory.Parent ?? throw new DirectoryNotFoundException(msg)).FullName;
    }

    static bool BinaryChoice(char trueOption, char falseOption)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;

        bool? choice = null;
        while (!choice.HasValue)
        {
            char key = Console.ReadKey(true).KeyChar.ToString().ToUpper()[0];

            if (key == trueOption) { choice = true; Console.WriteLine(trueOption); }
            else if (key == falseOption) { choice = false; Console.WriteLine(falseOption); }
        }

        Console.ForegroundColor = ConsoleColor.Gray;
        return choice.Value;
    }

    static IDay PromptDay()
    {
        IDay? day = null;
        while (day == null)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("Which day would you like to solve for? ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            string dayStr = Console.ReadLine() ?? "1";

            day = IDay.TryGetDay(dayStr.Length == 1 ? '0' + dayStr : dayStr);
        }
        Console.ForegroundColor = ConsoleColor.Gray;
        return day;
    }

    static int PromptPart()
    {
        Console.Write("Solve for part 1 or 2? ");
        return BinaryChoice('1', '2') ? 1 : 2;
    }

    static bool PromptTests()
    {
        Console.Write("Run unit tests? ");
        return BinaryChoice('Y', 'N');
    }


    static void UnitTests(IDay day, int part)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Unit Tests");

        var tests = (part == 1) ? day.UnitTestsP1 : day.UnitTestsP2;

        if (tests.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("No unit tests available");
            return;
        }

        foreach (var test in tests)
        {
            Console.ForegroundColor = ConsoleColor.White;
            // Console.WriteLine($"Input: {test.Key}");
            Console.Write($"Output: ");

            string input = test.Key.ReplaceLineEndings();
            string output = (part == 1) ? day.SolvePart1(input) : day.SolvePart2(input);

            if (output.Contains(Environment.NewLine))
            {
                Console.WriteLine();
            }
            Console.WriteLine(output);


            if (output == test.Value)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Success!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failure!");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"Expected Output: {tests[input]}");
            }
        }

        Console.WriteLine();
    }

    static async Task Main(string[] args)
    {
        string startUpPath = FindSolutionPath(); //  ASSUMING WE ARE IN AdventOfCode2024\AdventOfCode2024\bin\Debug\net9.0\ it would be Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.Parent!.FullName;
        IDay day;
        int part;
        bool runTests;

        if (args.Contains("init"))
        {
            await RepoInitializer.InitializeRepo();
            Console.WriteLine("Repo initialized");
            return;
        }
        day = args.Length >= 1
            ? IDay.TryGetDay((args[0].Length == 1 ? "0" : "") + args[0]) ?? throw new ArgumentNullException(nameof(args), "Invalid day [1,25]")
            : PromptDay();
        part = args.Length >= 2
            ? (int.TryParse(args[1].Trim(' '), out part) && (part == 1 || part == 2)) ? part : throw new ArgumentNullException(nameof(args), "Invalid part [1,2]")
            : PromptPart();
        runTests = args.Length >= 3
            ? args[2] == "1"
            : PromptTests();

        if (runTests)
            UnitTests(day, part);

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Day {day.Day} Part {part}");

        var inputPath = Path.Combine(startUpPath, @$"Inputs\Day{day.Day}.txt");
        string input;

        if (Path.Exists(inputPath))
            input = File.ReadAllText(inputPath).ReplaceLineEndings();
        else
        {
            input = await FetchInput(day.Day);
            using var inputWriter = new StreamWriter(new FileStream(inputPath, FileMode.Create), System.Text.Encoding.UTF8);
            await inputWriter.WriteAsync(input);
        }

        Console.ForegroundColor = ConsoleColor.White;
        //Console.WriteLine($"Input: {input}");
        Console.Write("Output: ");

        Stopwatch sw = Stopwatch.StartNew();
        string output = part == 1 ? day.SolvePart1(input) : day.SolvePart2(input);
        sw.Stop();

        if (output.Contains(Environment.NewLine)) // if its a single-line answer put it inline with Output: 
            Console.WriteLine();                  // otherwise put it all on the next line

        Console.WriteLine(output);

        string outputLocation = Path.Combine(startUpPath, @$"Outputs\Day{day.Day}Part{part}.txt");

        await ClipboardService.SetTextAsync(output);
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("The output has also been copied to the clipboard");

        using (var outputWriter = new StreamWriter(new FileStream(outputLocation, FileMode.Create), System.Text.Encoding.UTF8))
        {
            await outputWriter.WriteLineAsync(output);
        }

        Console.WriteLine($"The output has also been written to {outputLocation}");
        Console.WriteLine($"Execution took {(
            sw.Elapsed.TotalHours >= 1
            ? $"{sw.Elapsed}:hh\\:mm\\:ss\\.fff"
            : sw.Elapsed.TotalMinutes >= 1
            ? $"{sw.Elapsed}:mm\\:ss\\.fff"
            : sw.Elapsed.TotalSeconds >= 1
            ? $"{sw.Elapsed.Seconds}s {sw.Elapsed.Milliseconds}ms"
            : $"{sw.Elapsed.Milliseconds}ms"
        )}");

        Console.ReadKey();
    }
}