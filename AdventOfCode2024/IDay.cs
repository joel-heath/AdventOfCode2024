namespace AdventOfCode2024;

public interface IDay
{
    int Day { get; }
    Dictionary<string, string> UnitTestsP1 { get; }
    Dictionary<string, string> UnitTestsP2 { get; }
    string SolvePart1(string input);
    string SolvePart2(string input);

    private static readonly IEnumerable<Type> allDays = typeof(IDay).Assembly.GetTypes().Where(t => typeof(IDay).IsAssignableFrom(t)).Where(t => t != typeof(IDay));
    private static readonly Dictionary<string, Type> lookup = allDays.ToDictionary(d => d.Name[3..]);

    public static IDay? TryGetDay(string dayStr) => lookup.TryGetValue(dayStr, out Type? value) ? (IDay?)Activator.CreateInstance(value) : null;
}