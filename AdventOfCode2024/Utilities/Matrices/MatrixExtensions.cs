namespace AdventOfCode2024.Utilities.Matrices;

public static class MatrixExtensions
{
    /// <summary>
    /// Performs a row switch, swapping elements from row1 into row2.
    /// </summary>
    /// <param name="input">Matrix input</param>
    /// <param name="row1">Row index identifying the row to be swapped with row2</param>
    /// <param name="row2">Row index identifying the row to be swapped with row1</param>
    /// <returns>A new matrix with row1 and row2 swapped.</returns>
    public static Matrix RowSwitch(this Matrix input, int row1, int row2)
    {
        var rows = new IEnumerable<double>[input.Rows];
        for (int i = 0; i < input.Rows; i++)
        {
            rows[i] = true switch
            {
                var _ when i == row1 => input.GetRow(row2),
                var _ when i == row2 => input.GetRow(row1),
                _ => input.GetRow(i)
            };
        }

        return new Matrix(input.Rows, input.Columns, rows);
    }

    /// <summary>
    /// Performs a row multiplication, multiplying all elements in a given row by a scalar.
    /// </summary>
    /// <param name="input">Matrix input</param>
    /// <param name="row">Row index identifying which row will be scaled.</param>
    /// <param name="scalar">The quantity to multiply each element by.</param>
    /// <returns>A new matrix with the row scaled.</returns>
    public static Matrix RowMultiplication(this Matrix input, int row, int scalar)
    {
        var rows = new IEnumerable<double>[input.Rows];
        for (int i = 0; i < input.Rows; i++)
        {
            rows[i] = (i == row) ? input.GetRow(row).Select(e => scalar * e) : input.GetRow(i);
        }

        return new Matrix(input.Rows, input.Columns, rows);
    }

    /// <summary>
    /// Performs a row addition, taking elements from rowSource, multiplying them by the scalar, and adding them to thr respective elements of rowDestination.
    /// </summary>
    /// <param name="input">Matrix input.</param>
    /// <param name="rowSource">Row index to be multiplied and added to rowDestination.</param>
    /// <param name="rowDestination">Row index to be added to.</param>
    /// <param name="scalar">The quantity elements from rowSource are scaled by, defaults to 1.</param>
    /// <returns>A new matrix with the row addition applied.</returns>
    public static Matrix RowAddition(this Matrix input, int rowSource, int rowDestination, int scalar = 1)
    {
        var output = input.ToMatrix();

        for (int i = 0; i < input.Columns; i++)
        {
            output[rowDestination, i] += output[rowSource, i] * scalar;
        }

        return output;        
    }

    /// <summary>
    /// Performs a column addition, taking elements from colSource, multiplying them by the scalar, and adding them to the respective elements of colDestination.
    /// </summary>
    /// <param name="input">Matrix input.</param>
    /// <param name="colSource">Column index to be multiplied and added to colDestination.</param>
    /// <param name="colDestination">Column index to be added to.</param>
    /// <param name="scalar">The quantity elements from colSource are scaled by, defaults to 1.</param>
    /// <returns>A new matrix with the column addition applied.</returns>
    public static Matrix ColumnAddition(this Matrix input, int colSource, int colDestination, int scalar = 1)
    {
        var output = input.ToMatrix();

        for (int i = 0; i < input.Rows; i++)
        {
            output[i, colDestination] += scalar * output[i, colSource];
        }

        return output;
    }


    public static Matrix Translate(this Matrix points, params double[] units)
    {
        Matrix translation = new(points.Rows, points.Columns, units.Select(u => Enumerable.Repeat(u, points.Columns)));
        return translation + points;
    }
    public static Matrix Enlarge(this Matrix points, double factor) => factor * Matrix.Identity(points.Rows) * points;
    public static Matrix Stretch(this Matrix points, params double[] factors)
    {
        Matrix m = Matrix.Zero(points.Rows);
        for (int i = 0; i < points.Rows; i++) { m[i, i] = factors[i]; }

        return m;
    }
    public static Matrix Rescale(this Matrix points, params double[] upperBounds)
    {
        double[] oldUpperBounds = Enumerable.Repeat(double.MinValue, upperBounds.Length).ToArray();

        for (int i = 0; i < points.Columns; i++)
        {
            for (int j = 0; j < oldUpperBounds.Length; j++)
            {
                if (points[j, i] > oldUpperBounds[j]) oldUpperBounds[j] = points[j, i];
            }
        }

        return points.Stretch(upperBounds.Select((b, i) => b / oldUpperBounds[i]).ToArray()) * points;
    }

    public static Matrix EnlargeArea(this Matrix points, int factor)
    {
        Matrix enlarged = points.Enlarge(factor);

        Matrix widened = new(points.Rows, points.Columns * factor * factor);
        for (int i = 0; i < enlarged.Columns; i++)
        {
            var col = enlarged.GetCol(i).ToArray();
            for (int y = 0; y < factor; y++)
            {
                for (int x = 0; x < factor; x++)
                {
                    widened[0, i * factor * factor + y * factor + x] = col[0] + x - factor / 2;
                    widened[1, i * factor * factor + y * factor + x] = col[1] + y - factor / 2;
                }
            }
        }

        return widened;
    }

    public static Matrix ToMatrix(this Matrix input) => ToMatrix(input, (el) => el);
    public static Matrix ToMatrix(this Matrix input, Func<double, double> selector)
    {
        var output = new Matrix(input.Rows, input.Columns);

        for (int i = 0; i < output.Rows; i++)
        {
            for (int j = 0; j < output.Columns; j++)
            {
                output[i, j] = selector(input[i, j]);
            }
        }

        return output;
    }
}