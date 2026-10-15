namespace SudokuMasterBase.Serialization;

public class GridExporter<TElement>(IDictionary<string, Func<TElement, string>> properties)
    where TElement : class, IGridElement
{
    public char EmptyElementChar { get; set; } = ' ';
    
    public string ExportToString(TElement?[,] grid)
    {
        using var output = new StringWriter();
        var elements = new SortedSet<TElement>(Comparer<TElement>.Create((a, b) => a.Id.CompareTo(b.Id)));

        var width = grid.GetLength(0);
        var height = grid.GetLength(1);

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var element = grid[x, y];
                output.Write(element?.Id ?? EmptyElementChar);
                if (element != null) elements.Add(element);
            }
            output.WriteLine();
        }

        output.WriteLine();
        
        var propertyKeys = properties.Keys.ToArray(); 
        output.WriteLine("id," + string.Join(",", propertyKeys));
        foreach (var element in elements)
        {
            output.Write(element.Id);
            foreach (var propertyKey in propertyKeys)
            {
                output.Write(',');
                output.Write(Csv.Escape(properties[propertyKey](element)));
            }
            output.WriteLine();
        }

        return output.ToString();
    }
}