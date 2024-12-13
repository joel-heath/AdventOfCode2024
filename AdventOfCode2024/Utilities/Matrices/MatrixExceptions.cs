namespace AdventOfCode2024.Utilities.Matrices;

public class DimensionLessThanOneException : Exception
{
    public DimensionLessThanOneException(string? message, Exception? innerException) : base(message, innerException) { }
    public DimensionLessThanOneException(string? message) : base(message) { }
    public DimensionLessThanOneException() :base() { }
}

public class MatrixNotSquareException : Exception
{
    public MatrixNotSquareException(string? message, Exception? innerException) : base(message, innerException) { }
    public MatrixNotSquareException(string? message) : base(message) { }
    public MatrixNotSquareException() : base() { }
}

public class MatrixSingularException : Exception
{
    public MatrixSingularException(string? message, Exception? innerException) : base(message, innerException) { }
    public MatrixSingularException(string? message) : base(message) { }
    public MatrixSingularException() : base() { }
}