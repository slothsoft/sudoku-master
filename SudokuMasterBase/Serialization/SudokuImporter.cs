namespace SudokuMasterBase.Serialization;

public sealed class SudokuImporter
{
    public Sudoku ImportFromString(string text)
    {
        ArgumentNullException.ThrowIfNull(text);
        using var reader = new StringReader(text);
        return Import(reader);
    }

    public Sudoku Import(TextReader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);
        
        var records = Csv.Read(reader);
        var index = 0;
        SkipBlank();
        Expect("Persons:");
        var persons = ReadSection(name => new Person('c', name), "Rooms:");
        Expect("Rooms:");
        var rooms = ReadSection(name => new Room('c', name), null);
        if (persons.Grid.Count != rooms.Grid.Count ||
            persons.Grid[0].Length != rooms.Grid[0].Length)
            throw new FormatException("Persons and Rooms must have the same dimensions.");
        var sudoku = new Sudoku(persons.Grid[0].Length, persons.Grid.Count);
        for (var y = 0; y < sudoku.Height; y++)
        for (var x = 0; x < sudoku.Width; x++)
        {
            sudoku.Persons[x, y] = Resolve(persons.Grid[y][x], persons.Values);
            sudoku.Rooms[x, y] = Resolve(rooms.Grid[y][x], rooms.Values)!;
        }
        return sudoku;

        void SkipBlank()
        {
            while (index < records.Count && IsBlank(records[index])) index++;
        }

        void Expect(string marker)
        {
            if (index >= records.Count || records[index].Length != 1 ||
                records[index][0].TrimStart('\uFEFF') != marker)
                throw new FormatException($"Expected '{marker}' at record {index + 1}.");
            index++;
        }

        (List<string> Grid, Dictionary<char, T> Values) ReadSection<T>(
            Func<string, T> create, string? nextMarker) where T : class
        {
            var grid = new List<string>();
            while (index < records.Count && !IsBlank(records[index]) &&
                   !(records[index].Length == 2 && records[index][0] == "id"))
            {
                var row = records[index++];
                if (row.Length != 1 || row[0].Length == 0 ||
                    row[0].Any(c => c != '.' && !IsId(c)))
                    throw new FormatException("Grid rows may contain only ASCII letters, digits and '.'.");
                if (grid.Count > 0 && grid[0].Length != row[0].Length)
                    throw new FormatException("Grid rows must have equal widths.");
                grid.Add(row[0]);
            }
            if (grid.Count == 0) throw new FormatException("A grid must not be empty.");
            SkipBlank();
            if (index >= records.Count || !records[index].SequenceEqual(new[] { "id", "name" }))
                throw new FormatException("Expected CSV header 'id,name'.");
            index++;
            var values = new Dictionary<char, T>();
            while (index < records.Count)
            {
                if (nextMarker != null && records[index].Length == 1 &&
                    records[index][0] == nextMarker) break;
                var row = records[index++];
                if (IsBlank(row)) continue;
                if (row.Length != 2 || row[0].Length != 1 || !IsId(row[0][0]))
                    throw new FormatException("Expected a CSV row with a single-character id and name.");
                if (!values.TryAdd(row[0][0], create(row[1])))
                    throw new FormatException($"Duplicate id '{row[0]}'.");
            }
            foreach (var id in grid.SelectMany(row => row).Where(c => c != '.'))
                if (!values.ContainsKey(id))
                    throw new FormatException($"Grid references undefined id '{id}'.");
            return (grid, values);
        }
    }

    private static T? Resolve<T>(char id, Dictionary<char, T> values) where T : class =>
        id == '.' ? null : values[id];

    private static bool IsBlank(string[] row) => row.Length == 1 && row[0].Length == 0;
    private static bool IsId(char c) =>
        c is >= 'A' and <= 'Z' or >= 'a' and <= 'z' or >= '0' and <= '9';
}
