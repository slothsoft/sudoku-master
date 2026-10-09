using SudokuMasterBase.Utils;

namespace SudokuMasterBase;

public record Sudoku
{
    public int Width { get; }
    public int Height { get; }
    
    public Room[,] Rooms { get; }
    public Person?[,] Persons { get; }

    public Sudoku(int width, int height, Room[,]? rooms = null, Person?[,]? persons = null)
    {
        Width = width;
        Height = height;
        
        Rooms = rooms ?? new Room[width,height];
        Persons = persons ?? new Person[width,height];
    }
    
    public override string ToString()
    {
        return Persons.Stringify(p => p?.Name);
    }
}


