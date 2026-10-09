using SudokuMasterBase;

namespace SudokuMasterGenerator.Generator;

public class SudokuGenerator
{
    private static readonly string Letters = "abcdefghijklmnopqrstuvwxyz";
    
    public int Width { get; set; } = 5;
    public int Height { get; set; } = 5;
    public int Size {
        get => (Width + Height) / 2;
        set {
            Width = value;
            Height = value;
        }
    }
    
    public int RoomCount { get; set; } = 5;
    
    private readonly Random _random = new();

    public Sudoku Generate()
    {
        return new Sudoku(Width, Height, GenerateRooms(), GeneratePersons());
    }

    private Room[,] GenerateRooms()
    {
        var result = new Room[Width, Height];

        return result;
    }

    private Person[,] GeneratePersons()
    {
        var result = new Person[Width, Height];  
        var columns = Enumerable.Range(0, Width).ToList();
        var rows = Enumerable.Range(0, Height).ToList();

        var personCount = Math.Min(columns.Count, rows.Count);
        for (var i = 0; i < personCount; i++)
        {
            var personName = Letters[i].ToString().ToUpper();
            var personX = columns[_random.Next(0, columns.Count)];
            var personY = rows[_random.Next(0, rows.Count)];

            columns.Remove(personX);
            rows.Remove(personY);

            result[personX, personY] = new Person(personName);
        }

        return result;
    }
}