using Microsoft.Extensions.Configuration;
using System.Reflection;
using TextCopy;
using AdventOfCode2024.Utilities;

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

    static IDay GetUserDay()
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

    static void UnitTests(IDay day, int part)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Unit Tests");

        var tests = (part == 1) ? day.UnitTestsP1 : day.UnitTestsP2;

        foreach (string testI in tests.Keys)
        {
            Console.ForegroundColor = ConsoleColor.White;
            // Console.WriteLine($"Input: {testI}");
            Console.Write($"Output: ");

            var testINormalised = testI.ReplaceLineEndings();

            string testO = (part == 1) ? day.SolvePart1(testINormalised) : day.SolvePart2(testINormalised);

            if (testO.Contains(Environment.NewLine))
            {
                Console.WriteLine();
            }
            Console.WriteLine(testO);


            if (testO == tests[testI])
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Success!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failure!");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"Expected Output: {tests[testINormalised]}");
            }
        }
    }

    static async Task Main(string[] args)
    {
        string startupPath = FindSolutionPath(); //  ASSUMING WE ARE IN AdventOfCode2024\AdventOfCode2024\bin\Debug\net9.0\ it would be Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.Parent!.FullName;
        IDay day;
        int part;

        if (args.Contains("init"))
        {
            await RepoInitializer.InitializeRepo();
            Console.WriteLine("Repo initialized");
            return;
        }
        if (args.Length == 3)
        {
            day = IDay.TryGetDay((args[0].Length == 1 ? "0" : "") + args[0]) ?? throw new ArgumentNullException(nameof(args), "Invalid day [1,25]");
            if (!int.TryParse(args[1].Trim(' '), out part) || !(part == 1 || part == 2)) { throw new ArgumentNullException(nameof(args), "Invalid part [1,2]"); }
            if (args[2] == "1") { UnitTests(day, part); }
        }
        else
        {
            day = GetUserDay();
            Console.Write("Solve for part 1 or 2? ");
            part = BinaryChoice('1', '2') ? 1 : 2;
            Console.Write("Run test inputs? ");
            if (BinaryChoice('Y', 'N')) { Console.WriteLine(); UnitTests(day, part); }
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"{Environment.NewLine}Day {day.Day} Part {part}");

        var inputPath = Path.Combine(startupPath, @$"Inputs\Day{day.Day}.txt");
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

        string output = part == 1 ? day.SolvePart1(input) : day.SolvePart2(input);

        if (output.Contains(Environment.NewLine)) // if its a single-line answer put it inline with Output: 
            Console.WriteLine();                  // otherwise put it all on the next line

        Console.WriteLine(output);

        string outputLocation = Path.Combine(startupPath, @$"Outputs\Day{day.Day}Part{part}.txt");

        await ClipboardService.SetTextAsync(output);
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("The output has also been copied to the clipboard");

        using (var outputWriter = new StreamWriter(new FileStream(outputLocation, FileMode.Create), System.Text.Encoding.UTF8))
        {
            await outputWriter.WriteLineAsync(output);
        }

        Console.WriteLine($"The output has also been written to {outputLocation}");

        Console.ReadKey();
    }
}