namespace AdventOfCode2024.Utilities.Matrices;

public static class MatrixUtils
{
    static (double val, int row, int col) MinValue(Matrix m, HashSet<int> availableRows, HashSet<int> availableCols)
    {
        double val = double.MaxValue;
        int row = -1, col = -1;
        foreach (int i in availableRows)
        {
            foreach (int j in availableCols)
            {
                if (m[i, j] < val)
                {
                    val = m[i, j];
                    row = i;
                    col = j;
                }
            }
        }

        return (val, row, col);
    }

    /// <summary>
    /// Performs Prim's algorithm to generate the minimum spanning tree of a graph.
    /// </summary>
    /// <param name="adjacency">Adjacency Matrix describing edges of the graph.</param>
    /// <returns>Array of row & column indices representing the selected edges.</returns>
    public static (int row, int col)[] Prims(Matrix adjacency)
    {
        HashSet<int> availableRows = new(Enumerable.Range(1, adjacency.Rows - 1));
        HashSet<int> availableCols = new(adjacency.Columns);

        (int, int)[] selectedNodes = new (int, int)[adjacency.Columns - 1];
        int nodesSelected = 0, row = 0, col;

        while (nodesSelected < selectedNodes.Length)
        {
            availableCols.Add(row);
            (_, row, col) = MinValue(adjacency, availableRows, availableCols);
            availableRows.Remove(row);// prevent cycles
            selectedNodes[nodesSelected++] = (row, col);
        }

        return selectedNodes;
    }
}