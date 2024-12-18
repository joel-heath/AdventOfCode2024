using System.ComponentModel;
using System.Text;

namespace AdventOfCode2024.Utilities;

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
    public static Grid<char> FromString(string contents)
        => new(contents.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray());
    public static Grid<char> FromString(string contents, string rowDelimiter)
        => new(contents.Split(rowDelimiter).Select(l => l.ToCharArray()).ToArray());

    public bool Contains(Point p) => 0 <= p.X && p.X < Width && 0 <= p.Y && p.Y < Height;

    public static readonly IEnumerable<Point> CardinalVectors = [(0, -1), (1, 0), (0, 1), (-1, 0)];
    public static readonly IEnumerable<Point> DiagonalVectors = [(1, -1), (1, 1), (-1, 1), (-1, -1)];

    public IEnumerable<Point> Adjacents(Point p, bool includeDiagonals = false, bool contained = true)
    {
        var neighbours = includeDiagonals ? CardinalVectors.Concat(DiagonalVectors) : CardinalVectors;
        return contained ? neighbours.Select(n => p + n).Where(Contains) : neighbours.Select(n => p + n);
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

        switch (direction)
        {
            case 0: // North
                for (long i = start.Y - 1; i >= 0; i--)
                {
                    yield return (start.X, i);
                }
                break;
            case 1: // East
                for (long i = start.X + 1; i < Width; i++)
                {
                    yield return (i, start.Y);
                }
                break;
            case 2: // South
                for (long i = start.Y + 1; i < Height; i++)
                {
                    yield return (start.X, i);
                }
                break;
            case 3: // West
                for (long i = start.X - 1; i >= 0; i--)
                {
                    yield return (i, start.Y);
                }
                break;
            case 4: // North-East
                for (long i = 1; start.X + i < Width && start.Y - i >= 0; i++)
                {
                    yield return (start.X + i, start.Y - i);
                }
                break;
            case 5: // South-East
                for (long i = 1; start.X + i < Width && start.Y + i < Height; i++)
                {
                    yield return (start.X + i, start.Y + i);
                }
                break;
            case 6: // South-West
                for (long i = 1; start.X - i >= 0 && start.Y + i < Height; i++)
                {
                    yield return (start.X - i, start.Y + i);
                }
                break;
            case 7: // North-West
                for (long i = 1; start.X - i >= 0 && start.Y - i >= 0; i++)
                {
                    yield return (start.X - i, start.Y - i);
                }
                break;
            default:
                throw new ArgumentException("Invalid direction, may only be 0-3 (N,E,S,W) or 4-7 (NE,SE,SW,NW)", nameof(direction));
        }
    }

    public IEnumerable<Point> LineThrough(Point target, int direction, bool inclusive = true)
    {
        if (!Contains(target)) yield break;

        switch (direction)
        {
            case 0: // North to south
                for (int i = 0; i < Height; i++)
                {
                    if (!inclusive || i != target.Y)
                        yield return (target.X, i);
                }
                break;
            case 1: // East to west
                for (int i = 0; i < Width; i++)
                {
                    if (!inclusive || i != target.X)
                        yield return (target.Y, i);
                }
                break;
            case 2: // North-West to South-East
                for (int i = 0; i < Width; i++)
                {
                    if (!inclusive || i != target.X)
                        yield return (i, i);
                }
                break;
            case 3: // North-East to South-West
                for (int i = 0; i < Width; i++)
                {
                    if (!inclusive || i != target.X)
                        yield return (Width - i - 1, i);
                }
                break;
            default:
                throw new ArgumentException("Invalid direction, may only be 0-1 (N-S,E-W) or 2-3 (NW-SE, NE, SW)", nameof(direction));
        }
    }

    public IEnumerable<T> LineTo(Point start, Point end, bool inclusive = true)
    {
        if (!(start.X == end.X || start.Y == end.Y || Math.Abs(start.X - end.X) == Math.Abs(start.Y - end.Y)))
            throw new ArgumentException("Points must be aligned horizontally, vertically, or diagonally");

        int xCmp = end.X.CompareTo(start.X);
        int yCmp = end.Y.CompareTo(start.Y);

        for (long x = start.X, y = start.Y; (x, y) != end; x += xCmp, y += yCmp)
        {
            if (Contains((x, y)))
                yield return this[(x, y)];
        }

        if (inclusive && Contains(end))
            yield return this[end];
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
    public static Point operator -(Point a) => new(-a.X, -a.Y);
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

    public readonly bool Contained(int width, int height)
        => X >= 0 && Y >= 0 && X < width && Y < height;

    public readonly bool Contained(int width)
        => Contained(width, width);

    public static readonly IEnumerable<Point> CardinalVectors = [(0, -1), (1, 0), (0, 1), (-1, 0)];
    public static readonly IEnumerable<Point> DiagonalVectors = [(1, -1), (1, 1), (-1, 1), (-1, -1)];

    public IEnumerable<Point> Adjacents(bool includeDiagonals = false, int width = -1, int height = -1)
    {
        Point here = (X, Y);
        var vectors = includeDiagonals ? CardinalVectors.Concat(DiagonalVectors) : CardinalVectors;
        var neighbours = vectors.Select(n => here + n);
        return width > 0
            ? height > 0
                ? neighbours.Where(p => p.Contained(width, height))
                : neighbours.Where(p => p.Contained(width))
            : neighbours;
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
        return GetPermutationsWithRept(list, length - 1).SelectMany(t => list, (t1, t2) => t1.Concat([t2]));
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