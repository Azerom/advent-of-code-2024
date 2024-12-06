static class Extensions {
    public static char[] GetColumn(this char[,] matrix, int columnNumber, int? from = null, int? to = null)
    {
        from ??= 0;
        to ??= matrix.GetLength(0) - from;

        return Enumerable.Range((int)from, (int)to)
                .Select(x => matrix[x, columnNumber])
                .ToArray();
    }

    public static char[] GetRow(this char[,] matrix, int rowNumber, int? from = null, int? to = null)
    {
        from ??= 0;
        to ??= matrix.GetLength(1) - from;

        return Enumerable.Range((int)from, (int)to)
                .Select(x => matrix[rowNumber, x])
                .ToArray();
    }

public static Coordinate? IndexOf2D<T>(this T[,] array, T value)
{
    for (int i = 0; i < array.GetLength(0); i++)
    {
        for (int j = 0; j < array.GetLength(1); j++)
        {
            if (array[i, j].Equals(value))
            {
                return (i, j);
            }
        }
    }
    return null; // Value not found
}

}