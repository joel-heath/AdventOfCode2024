using System.Text.RegularExpressions;

namespace AdventOfCode2024.Utilities;

public static partial class Utils
{
    public static T[][] NewJaggedArray<T>(int outerLength, int innerLength)
    {
        T[][] jaggedArray = new T[outerLength][];
        for (int i = 0; i < outerLength; i++)
            jaggedArray[i] = new T[innerLength];
        return jaggedArray;
    }

    /// Return true for a given midpoint if the target is to the right or equal to the midpoint (leftBound = midpoint), false if it's to the left.</param>
    public static int BinarySearch(int rightStart, Func<int, bool> midHandler, int leftStart = 0)
    {
        int left = leftStart;
        int right = rightStart;
        while (left != right)
        {
            int mid = (left + right + 1) / 2;
            if (midHandler(mid))
                left = mid;
            else
                right = mid - 1;
        }
        return left;
    }

    /// <summary>
    /// Standard modulo doesn't exhibit expected behaviour for looping and wrapping indices (-1 % 5 != 4).
    /// </summary>
    /// <param name="dividend">a in a % b (dividend)</param>
    /// <param name="divisor">b in a % b (modolus)</param>
    /// <returns>The reaminder of the dividend and the divisor</returns>
    public static long Mod(long dividend, long divisor)
    {
        long r = dividend % divisor;
        return r < 0 ? r + divisor : r;
    }

    public static IEnumerable<long> Range(long count) => Range(0, count);
    public static IEnumerable<int> Range(int count) => Range(0, count);

    /// <summary>
    /// Enumerable.Range but lazily evaluated & can have negative counts to go in reverse
    /// </summary>
    /// <param name="start">The starting element of the range</param>
    /// <param name="count">The vector difference between the start value and end</param>
    /// <returns>IEnumerable<long> starting at start and spanning count integers.</returns>
    public static IEnumerable<long> Range(long start, long count)
    {
        long end = start + count;
        long inc = end < start ? -1 : 1;
        while (start != end)
        {
            yield return start;
            start += inc;
        }
    }
    public static IEnumerable<int> Range(int start, int count)
    {
        int end = start + count;
        int inc = end < start ? -1 : 1;
        while (start != end)
        {
            yield return start;
            start += inc;
        }
    }

    /// <summary>
    /// Utils.Range but with a custom increment, therefore count is absolute
    /// </summary>
    /// <param name="start">The starting element of the range</param>
    /// <param name="count">The vector difference between the start value and end</param>
    /// <returns>Such numbers as described</returns>
    public static IEnumerable<long> Range(long start, long count, long step)
    {
        long end = start + count * step;
        while (start != end)
        {
            yield return start;
            start += step;
        }
    }

    /// <summary>
    /// Infinitely yields the value parameter (intended to be used with AggregateWhile)
    /// </summary>
    /// <typeparam name="T">Type of value</typeparam>
    /// <param name="value">Value to be server on each enumeration</param>
    /// <returns>Infinite copies of value</returns>
    public static IEnumerable<T> EnumerateForever<T>(T value)
    {
        while (true) yield return value;
    }

    public static IEnumerable<int> EnumerateForever()
    {
        int index = 0;
        while (true) yield return index++;
    }

    public static long GCF(long a, long b) => EnumerateForever().AggregateWhile((a, b), (acc, _) => (acc.b, acc.a % acc.b), acc => acc.b != 0).a;
    public static long LCM(long a, long b) => a * b / GCF(a, b);
    public static long LCM(params long[] a) => a.Aggregate(LCM);
    public static long LCM(IEnumerable<long> a) => a.Aggregate(LCM);

    public static (List<(string name, Dictionary<int, T> edges)> graph, Dictionary<string, int> nameToIndexMap) GenerateGraph<T>(IEnumerable<(string source, string destination, T weight)> mappings, bool directed = false)
    {
        Dictionary<string, int> nameToIndex = [];
        List<(string name, Dictionary<int, T> edges)> graph = [];

        foreach ((string source, string destination, T weight) in mappings)
        {
            if (!nameToIndex.TryGetValue(source, out int fromIndex))
            {
                fromIndex = graph.Count;
                graph.Add((source, []));
                nameToIndex[source] = fromIndex;
            }
            if (!nameToIndex.TryGetValue(destination, out int toIndex))
            {
                toIndex = graph.Count;
                graph.Add((destination, []));
                nameToIndex[destination] = toIndex;
            }

            var fromEdges = graph[fromIndex].edges;
            fromEdges[toIndex] = weight;

            if (!directed)
            {
                var toEdges = graph[toIndex].edges;
                toEdges[fromIndex] = weight;
            }
        }

        return (graph, nameToIndex);
    }

    /* pattern = (?<capital>[A-Z])(?<remaining>\w+)
     * data = "Hello, World!"
     * returns [
     *     { "0": "Hello", "capital": "H", "remaining": "ello" },
     *     { "0": "World", "capital": "W", "remaining": "orld" }
     * ]
     */
    public static IEnumerable<Dictionary<string, string>> MatchNamed(string input, string pattern)
        => Regex.Matches(input, pattern).Select(match => match.Groups.Cast<Group>().ToDictionary(t => t.Name, t => t.Value));

    public static IEnumerable<Capture> FindAllOverlap(string input, string pattern)
        => Regex.Matches(input, "(?=(" + pattern + "))").SelectMany(m => m.Groups[1].Captures);

    public static IEnumerable<long> GetLongs(string input)
        => Integer().Matches(input).Select(M => long.Parse(M.Value));
    public static IEnumerable<int> GetInts(string input)
        => Integer().Matches(input).Select(M => int.Parse(M.Value));


    [GeneratedRegex(@"-?\d+")]
    private static partial Regex Integer();
}
