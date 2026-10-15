namespace SudokuMasterBase.Serialization;

public class SudokuExporter
{
    public string ExportToString(Sudoku sudoku)
    {
        using var writer = new StringWriter();
        Export(sudoku, writer);
        return writer.ToString().ReplaceLineEndings("\n");
    }

    public void Export(Sudoku sudoku, TextWriter writer)
    {
        ArgumentNullException.ThrowIfNull(sudoku);
        ArgumentNullException.ThrowIfNull(writer);

        writer.Write(ExportPersons(sudoku.Persons));
        writer.WriteLine();
        writer.Write(ExportRooms(sudoku.Rooms));
    }

    internal static string ExportPersons(Person?[,] grid)
    {
        Dictionary<string, Func<Person, string>> properties = new()
        {
            { "displayName", p => p.DisplayName },
        };
        return "Persons:\n" + new GridExporter<Person>(properties).ExportToString(grid).ReplaceLineEndings("\n");
    }
    
    internal static string ExportRooms(Room?[,] grid)
    {
        Dictionary<string, Func<Room, string>> properties = new()
        {
            { "displayName", p => p.DisplayName },
        };
        return "Rooms:\n"  + new GridExporter<Room>(properties).ExportToString(grid).ReplaceLineEndings("\n");
    }
}