namespace SudokuMasterBase.Serialization;

public class GridImporter<TElement>(string title, IDictionary<string, Action<TElement, string>> properties)
    where TElement : class, IGridElement
{
    public char EmptyElementChar { get; set; } = ' ';
    
    public string ImportFromString(string grid)
    {
        return null;
    }
}