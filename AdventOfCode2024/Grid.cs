using System.Text;

namespace AdventOfCode2024;

public class Grid<T>(int x, int y)
{
    private readonly T[,] points = new T[x, y];
    public int Width { get; } = x;
    public int Height { get; } = y;

    public T this[Point c]
    {
        get => points[c.X, c.Y];
        set => points[c.X, c.Y] = value;
    }
    public T this[int x, int y]
    {
        get => points[x, y];
        set => points[x, y] = value;
    }

    public Grid(int x, int y, T defaultValue) : this(x, y)
    {
        foreach (var p in AllPositions())
            this[p] = defaultValue;
    }
    public Grid(T[][] contents, bool transpose = true) : this(transpose ? contents[0].Length : contents.Length, transpose ? contents.Length : contents[0].Length)
    {
        for (int r = 0; r < Height; r++)
        {
            for (int c = 0; c < Width; c++)
            {
                points[c, r] = transpose ? contents[r][c] : contents[c][r];
            }
        }
    }
    public Grid(int width, int height, IEnumerable<T> contents) : this(width, height)
    {
        var enumerator = contents.GetEnumerator();
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                enumerator.MoveNext();
                this[x, y] = enumerator.Current;
            }
        }
    }

    public bool Contains(Point p) => 0 <= p.X && p.X < Width && 0 <= p.Y && p.Y < Height;

    public static readonly IEnumerable<Point> CardinalVectors = new Point[] { (0, -1), (1, 0), (0, 1), (-1, 0) };
    public static readonly IEnumerable<Point> DiagonalVectors = new Point[] { (1, -1), (1, 1), (-1, 1), (-1, -1) };

    public IEnumerable<Point> Adjacents(Point p, bool includeDiagonals = false)
    {
        var neighbours = includeDiagonals ? CardinalVectors.Concat(DiagonalVectors) : CardinalVectors;
        return neighbours.Select(n => p + n).Where(Contains);
    }

    public IEnumerable<Point> AllPositions()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                yield return (x, y);
            }
        }
    }

    public IEnumerable<Point> LineOut(Point start, int direction, bool inclusive)
    {
        if (!Contains(start)) yield break;

        if (inclusive) yield return start;

        if (direction == 0) // North
        {
            for (long i = start.Y - 1; i >= 0; i--)
            {
                yield return (start.X, i);
            }
        }
        else if (direction == 2) // South
        {
            for (long i = start.Y + 1; i < Height; i++)
            {
                yield return (start.X, i);
            }
        }
        else if (direction == 3) // West
        {
            for (long i = start.X - 1; i >= 0; i--)
            {
                yield return (i, start.Y);
            }
        }
        else if (direction == 1) // East
        {
            for (long i = start.X + 1; i < Width; i++)
            {
                yield return (i, start.Y);
            }
        }
        else { throw new ArgumentException("Invalid direction, may only be 0-3 (N,E,S,W)", nameof(direction)); }
    }

    public IEnumerable<Point> LineThrough(Point target, int direction, bool inclusive)
    {
        if (!Contains(target)) yield break;

        if (direction == 0) // North to south
        {
            for (int i = 0; i < Height; i++)
            {
                if (!inclusive || (i != target.Y))
                    yield return (target.X, i);
            }
        }
        else if (direction == 1) // East to west
        {
            for (int i = 0; i < Width; i++)
            {
                if (!inclusive || (i != target.X))
                    yield return (target.Y, i);
            }
        }
        
        else throw new ArgumentException("Invalid direction, may only be 0-1 (N-S,E-W)", nameof(direction));
    }

    public IEnumerable<T> LineTo(Point start, Point end, bool inclusive = true)
    {
        ///if (!(start.X == end.X || start.Y == end.Y)

        int xCmp = end.X.CompareTo(start.X);
        int yCmp = end.Y.CompareTo(start.Y);

        for (long x = start.X, y = start.Y; (x, y) != end; x += xCmp, y += yCmp) // (xCmp == 0 ? true : (xCmp < 0 ? x <= end.X : x >= end.X)) && (yCmp == 0 ? true : (yCmp < 0 ? y <= end.Y : y >= end.Y))
        {
            if (Contains((x, y)))
                yield return this[(x, y)];
        }

        if (inclusive && Contains(end))
            yield return this[end];

        /*
        if (start.X == end.X)
        {
            long small = Math.Min(start.Y, end.Y);
            long large = Math.Max(start.Y, end.Y);

            for (long i = small; !inclusive && i < large || inclusive && i <= large; i++)
            {
                if (!Contains((start.X, i))) yield break;
                yield return this[(start.X, i)];
            }
        }
        else if (start.Y == end.Y)
        {
            long small = Math.Min(start.X, end.X);
            long large = Math.Max(start.X, end.X);

            for (long i = small; !inclusive && i < large || inclusive && i <= large; i++)
            {
                if (!Contains((i, start.Y))) yield break;
                yield return this[(i, start.Y)];
            }
        }
        else
        {
            throw new Exception($"Not a straight line between {start} and {end}");
        }*/
    }

    public override string ToString()
    {
        var s = new StringBuilder();
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                s.Append(points[x, y]!.ToString());
            }
            s.AppendLine();
        }
        return s.ToString();
    }
}

public struct Point(long x, long y)
{
    public long X { get; set; } = x;
    public long Y { get; set; } = y;

    public readonly long MDistanceTo(Point b) => Math.Abs(b.X - X) + Math.Abs(b.Y - Y);
    public readonly long this[int index] => index == 0 ? X : Y;

    public static Point operator *(long scalar, Point point) => new(scalar * point.X, scalar * point.Y);
    public static Point operator +(Point a, Point b) => new(a.X + b.X, a.Y + b.Y);
    public static Point operator -(Point a, Point b) => new(a.X - b.X, a.Y - b.Y);
    public static Point operator -(Point a) => new(-a.X, a.Y);
    public static bool operator ==(Point? a, Point? b) => a.Equals(b);
    public static bool operator !=(Point? a, Point? b) => !(a == b);

    public override readonly bool Equals(object? obj) => obj is Point p && p.X.Equals(X) && p.Y.Equals(Y);
    public override readonly int GetHashCode() => HashCode.Combine(X, Y);


    public static implicit operator Point((long x, long y) coords) => new(coords.x, coords.y);
    //public static implicit operator (long X, long Y)(Point p) => (p.X, p.Y);
    public override readonly string ToString() => $"({X}, {Y})";
    public readonly long[] ToArray() => [X, Y];
    public readonly void Deconstruct(out long x, out long y)
    {
        x = X;
        y = Y;
    }
}

public struct Coord(long x, long y, long z)
{
    public long X { get; set; } = x;
    public long Y { get; set; } = y;
    public long Z { get; set; } = z;

    public static implicit operator Coord((int x, int y, int z) coords) => new(coords.x, coords.y, coords.z);

    static IEnumerable<IEnumerable<T>> GetPermutationsWithRept<T>(IEnumerable<T> list, int length)
    {
        if (length == 1) return list.Select(t => new T[] { t });
        return GetPermutationsWithRept(list, length - 1).SelectMany(t => list, (t1, t2) => t1.Concat(new T[] { t2 }));
    }

    public readonly Coord[] Adjacents() => [this + (0, 0, 1), this - (0, 0, 1), this + (0, 1, 0), this - (0, 1, 0), this + (1, 0, 0), this - (1, 0, 0)];

    public static Coord operator +(Coord a, Coord b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Coord operator -(Coord a, Coord b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public long this[int index]
    {
        readonly get => index == 0 ? X : index == 1 ? Y : index == 2 ? Z : throw new IndexOutOfRangeException();
        set
        {
            switch (index)
            {
                case 0: X = value; break;
                case 1: Y = value; break;
                case 2: Z = value; break;
                default: throw new IndexOutOfRangeException();
            }
        }

    }
    public override readonly bool Equals(object? obj) => obj is Coord p && p.X.Equals(X) && p.Y.Equals(Y) && p.Z.Equals(Z);
    public override readonly int GetHashCode() => HashCode.Combine(X, Y, Z);

    public static bool operator ==(Coord a, Coord b) => a.X == b.X && a.Y == b.Y && a.Z == b.Z;
    public static bool operator !=(Coord a, Coord b) => a.X != b.X || a.Y != b.Y || a.Z != b.Z;
    public static bool operator <(Coord a, Coord b) => a.X < b.X && a.Y < b.Y && a.Z < b.Z;
    public static bool operator >(Coord a, Coord b) => a.X > b.X && a.Y > b.Y && a.Z > b.Z;
    public static bool operator <=(Coord a, Coord b) => a.X <= b.X && a.Y <= b.Y && a.Z <= b.Z;
    public static bool operator >=(Coord a, Coord b) => a.X >= b.X && a.Y >= b.Y && a.Z >= b.Z;
    public override readonly string ToString() => $"({X}, {Y}, {Z})";
    public readonly long[] ToArray() => [X, Y, Z];
    public readonly void Deconstruct(out long x, out long y, out long z)
    {
        x = X;
        y = Y;
        z = Z;
    }
}