namespace AdventOfCode2024;

public static class ExtensionMethods
{
    public static IEnumerable<T> Dump<T>(this IEnumerable<T> input)
    {
        var data = new List<T>();

        foreach (var item in input)
        {
            Console.WriteLine(item);
            data.Add(item);
        }

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

    public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int k)
        => k == 0 ? [[]] :
          elements.SelectMany((e, i) =>
            elements.Skip(i + 1).Combinations(k - 1).Select(c => (new[] { e }).Concat(c)));

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
        action.Invoke();
        return true;
    }
}