using System.Text;

namespace SudokuMasterBase.Utils;

public static class ArrayStringUtils
{
    public static string Stringify<TElement>(this TElement[,] array, Func<TElement, string?> toString)
    {
        var width = array.GetLength(0);
        var height = array.GetLength(1);
        
        var builder = new StringBuilder();
        builder.AppendBorder(width);

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                builder.Append("| ");
                builder.Append(toString(array[x, y]) ?? " ");
                builder.Append(' ');
            }
            builder.AppendLine("|");
            builder.AppendBorder(width);
        }

        return builder.ToString();
    }
    
    private static void AppendBorder(this StringBuilder builder, int width)
    {
        for (var x = 0; x < width; x++)
        {
            builder.Append("+---");
        }
        builder.AppendLine("+");
    }
}