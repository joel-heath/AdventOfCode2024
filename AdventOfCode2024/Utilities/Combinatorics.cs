namespace AdventOfCode2024.Utilities;

public static class Combinatorics
{
    public static long Factorial(long n)
        => n < 0 ? throw new ArgumentOutOfRangeException(nameof(n), "n! is undefined for negative numbers.")
            : n <= 1 ? 1
            : Utils.Range(1, n).Aggregate((acc, i) => acc * i);

    public static long RisingFactorial(long x, long n)
        => n < 0 ? throw new ArgumentOutOfRangeException(nameof(n), "(x)^n is undefined for negative n.")
            : n == 0 ? 1
            : Utils.Range(x, n).Aggregate((acc, i) => acc * i);

    public static long FallingFactorial(long x, long n)
        => n < 0 ? throw new ArgumentOutOfRangeException(nameof(n), "(x)_n is undefined for negative n.")
            : n == 0 ? 1
            : Utils.Range(x - n + 1, n).Aggregate((acc, i) => acc * i);


    public static long Multinomial(long n, params long[] k)
        => k.Any(r => r < 0) ? throw new ArgumentOutOfRangeException(nameof(k), "Multinomial coefficients are undefined for negative r.")
            : k.Sum() != n ? 0 : Factorial(n) / k.Aggregate((acc, i) => acc * Factorial(i));

    public static long Choose(long n, long k)
        => k < 0 ? throw new ArgumentOutOfRangeException(nameof(k), "nCk is undefined for negative k.")
            : n < 0 ? (long)Math.Pow(-1, k) * Multichoose(n, k)
            : Math.Abs(n) < Math.Abs(k) ? 0
            : FallingFactorial(n, n - k) / Factorial(n - k);

    public static long Multichoose(long n, long k)
        => Choose(n + k - 1, k);


    public static long Permute(long n, long k)
        => k < 0 ? throw new ArgumentOutOfRangeException(nameof(k), "nPk is undefined for negative k.")
            : n < 0 ? (long)Math.Pow(-1, k) * Permute(n + k - 1, k)
            : n < k ? 0
            : FallingFactorial(n, n - k);
}

public static class CombinatoricsExtensionMethods
{
    public static IEnumerable<IEnumerable<T>> Choose<T>(this IEnumerable<T> elements, int k)
        => elements is IList<T> list ? Choose(list, k) : Choose(elements.ToList(), k);

    private static IEnumerable<IEnumerable<T>> Choose<T>(IList<T> elements, int k)
        => k == 0 ? [[]] :
          elements.SelectMany((e, i) =>
            elements.Skip(i + 1).Choose(k - 1).Select(c => (new[] { e }).Concat(c)));


    public static IEnumerable<IEnumerable<T>> Multichoose<T>(this IEnumerable<T> elements, int k)
        => elements is IList<T> list ? Multichoose(list, k) : Multichoose(elements.ToList(), k);

    private static IEnumerable<IEnumerable<T>> Multichoose<T>(IList<T> elements, int k)
        => k == 0 ? [[]] :
          elements.SelectMany((e, i) =>
            elements.Multichoose(k - 1).Select(c => (new[] { e }).Concat(c)));


    public static IEnumerable<IEnumerable<T>> Permute<T>(this IEnumerable<T> elements)
        => elements is IList<T> list ? Permute(list) : Permute(elements.ToList());

    private static IEnumerable<IEnumerable<T>> Permute<T>(IList<T> elements)
        => elements.Count == 1 ? [elements] :
            elements.SelectMany(e =>
                elements.Except([e]).Permute().Select(p => (new[] { e }).Concat(p)));
}