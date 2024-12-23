namespace AdventOfCode2024.Utilities;

public static class ExtensionMethods
{
    public static Point Sum(this IEnumerable<Point> source)
    {
        long x = 0, y = 0;
        foreach (var point in source)
        {
            x += point.X;
            y += point.Y;
        }
        return (x, y);
    }

    public static Point Sum<T>(this IEnumerable<T> source, Func<T, Point> selector)
    {
        long x = 0, y = 0;
        foreach (var point in source.Select(selector))
        {
            x += point.X;
            y += point.Y;
        }
        return (x, y);
    }

    /// <summary>
    /// When I should just assign a variable but I really want it to be a one-liner
    /// Materialises the source, assigns the collection to `output` and returns the collection
    /// </summary>
    public static List<T> AssignList<T>(this IEnumerable<T> source, out List<T> output)
    {
        var materialised = source.ToList();
        output = materialised;
        return materialised;
    }

    /// <summary>
    /// When I should just assign a variable but I really want it to be a one-liner
    /// Materialises the source, assigns the collection to `output` and returns the collection
    /// </summary>
    public static T Assign<T>(this T source, out T output)
    {
        output = source;
        return source;
    }

    /// <summary>
    /// When I really should've just used a semicolon and started a new line, but I want a one-liner
    /// Materialises the source to ensure whatever impure function has been written is executed, then 
    /// </summary>

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "One-liners are funny")]
    public static TReplace Replace<TSource, TReplace>(this TSource source, TReplace replacement)
    {
        return replacement;
    }

    /// <summary>
    /// Used like Utils.EnumerateForever(), except this will also return the source collection (materialised ONCE)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static IEnumerable<List<T>> ReturnForever<T>(this IEnumerable<T> source)
    {
        var materialised = source.ToList();
        while (true)
        {
            yield return materialised;
        }
    }
    /// <summary>
    /// Used like Utils.EnumerateForever(), except this is an extension method
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "One-liners are funny")]
    public static IEnumerable<object> WhileTrue<T>(this IEnumerable<T> source)
    {
        object o = new();
        while (true)
        {
            yield return o;
        }
    }

    public static IEnumerable<T> DumpInline<T>(this IEnumerable<T> input)
    {
        var data = input.ToList();
        Console.WriteLine($"[{string.Join(", ", input)}]");
        return data;
    }
    public static IEnumerable<T> Dump<T>(this IEnumerable<T> input)
        => input.Dump(Environment.NewLine);

    public static IEnumerable<T> Dump<T>(this IEnumerable<T> input, string delimiter)
    {
        var data = new List<T>();

        Console.Write("[" + delimiter);
        foreach (var item in input)
        {
            Console.Write(item + delimiter);
            data.Add(item);
        }
        Console.WriteLine("]" + delimiter);

        Console.ReadKey();
        return data;
    }

    public static T Dump<T>(this T input) where T : notnull
    {
        Console.WriteLine(input);
        return input;
    }

    public static IEnumerable<(T, long)> RLE<T>(this IEnumerable<T> source)
    {
        using var enumerator = source.GetEnumerator();
        if (!enumerator.MoveNext()) yield break;

        var curr = enumerator.Current;
        long count = 1;
        while (enumerator.MoveNext())
        {
            if (enumerator.Current!.Equals(curr))
            {
                count++;
            }
            else
            {
                yield return (curr, count);
                count = 1;
                curr = enumerator.Current;
            }
        }
        yield return (curr, count);
    }

    public static T[][] Transpose<T>(this IEnumerable<IEnumerable<T>> source)
        => source.SelectMany(inner => inner.Select((item, index) => (item, index)))
            .GroupBy(i => i.index, i => i.item)
            .Select(g => g.ToArray()).ToArray();


    public static IEnumerable<TAcc> Scan<TSource, TAcc>(this IEnumerable<TSource> source, TAcc seed, Func<TAcc, TSource, TAcc> func)
    {
        var acc = seed;
        foreach (var item in source)
        {
            acc = func(acc, item);
            yield return acc;
        }
    }

    public static IEnumerable<TSource> Scan<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> func)
    {
        using var enumerator = source.GetEnumerator();
        if (!enumerator.MoveNext()) throw new InvalidOperationException("Sequence contains no elements");
        TSource acc = enumerator.Current;
        acc = func(acc, enumerator.Current);
        yield return acc;

        while (enumerator.MoveNext())
        {
            acc = func(acc, enumerator.Current);
            yield return acc;
        }
    }

    public static T AggregateWhile<T>(this IEnumerable<T> src, Func<T, T, T> accumFn, Predicate<T> whileFn)
    {
        using var e = src.GetEnumerator();
        if (!e.MoveNext())
            throw new Exception("At least one element required by AggregateWhile");
        var ans = e.Current;
        while (whileFn(ans) && e.MoveNext())
            ans = accumFn(ans, e.Current);
        return ans;
    }

    public static TAccum AggregateWhile<TAccum, TSource>(this IEnumerable<TSource> src, TAccum seed, Func<TAccum, TSource, TAccum> accumFn, Predicate<TAccum> whileFn)
    {
        using var e = src.GetEnumerator();
        if (!e.MoveNext()) return seed;
        var ans = accumFn(seed, e.Current);
        while (whileFn(ans) && e.MoveNext())
            ans = accumFn(ans, e.Current);
        return ans;
    }

    public static TAccumulate AggregateWhileAvailable<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, Func<TAccumulate, TSource, IEnumerable<TSource>> feedback)
    {
        Queue<TSource> queue = new();

        var acc = seed;
        foreach (var item in source)
        {
            acc = accumulator(acc, item);
            foreach (var newItem in feedback(acc, item)) queue.Enqueue(newItem);
        }

        while (queue.TryDequeue(out var item))
        {
            acc = accumulator(acc, item);
            foreach (var newItem in feedback(acc, item)) queue.Enqueue(newItem);
        }

        return acc;
    }

    /// <summary>
    /// For managing two items at once, but inclusively, very similar to chunking
    /// Eg. Chunk: [0, 1, 2, 3]  =>  [[0, 1], [2, 3]]
    ///    Window: [0, 1, 2, 3]  =>  [[0, 1], [1, 2], [2, 3]]
    /// </summary>
    /// <typeparam name="T">Input source type</typeparam>
    /// <param name="source">IEnumerable of elements to chunk inclusively</param>
    /// <param name="windowWidth">Size of sub-arrays</param>
    /// <returns></returns>
    public static IEnumerable<T[]> Window<T>(this IEnumerable<T> source, int windowWidth)
    {
        var previousN = new T[windowWidth];
        var enumerator = source.GetEnumerator();

        int i = 0;

        // populate with n items first before returning anything
        for (; i < windowWidth; i++)
        {
            if (!enumerator.MoveNext()) throw new InvalidOperationException($"Not enough elements in source. Source only contained {i} item{(i == 1 ? "" : "s")} when {windowWidth} {(windowWidth == 1 ? "was" : "were")} required");
            var curr = enumerator.Current;
            previousN[i] = curr;
        }

        yield return previousN.ToArray();

        i = 0;
        while (enumerator.MoveNext())
        {
            previousN[i] = enumerator.Current;
            i = (i + 1) % windowWidth;
            yield return [.. previousN[i..], .. previousN[..i]];
        }
    }

    private static IEnumerable<T> GenerateIterator<T>(Func<int, T> generator, int count)
    {
        for (int index = 0; index < count; index++)
        {
            yield return generator(index);
        }
    }

    public static IEnumerable<IEnumerable<T>> ToJagged<T>(this T[,] source)
        => GenerateIterator(i =>
            GenerateIterator(j =>
                source[i, j], source.GetLength(1)),
            source.GetLength(0));

    public static IEnumerable<IEnumerable<IEnumerable<T>>> ToJagged<T>(this T[,,] source)
        => GenerateIterator(i =>
            GenerateIterator(j =>
                GenerateIterator(k => source[i, j, k], source.GetLength(2)),
                source.GetLength(1)),
            source.GetLength(0));

    public static IEnumerable<T> Flatten<T>(this T[,] source)
    {
        foreach (var item in source) yield return item;
    }

    public static IEnumerable<T> Flatten<T>(this T[,,] source)
    {
        foreach (var item in source) yield return item;
    }



    public static bool InvokeTruthfully(this Action action)
    {
        action();
        return true;
    }
}