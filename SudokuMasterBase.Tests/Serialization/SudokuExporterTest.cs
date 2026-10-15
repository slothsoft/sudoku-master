using SudokuMasterBase.Serialization;

namespace SudokuMasterBase.Tests.Serialization;

public class SudokuExporterTest
{
    private readonly SudokuExporter _exporter = new();

    [Test]
    public void ExportPersons()
    {
        var persons = new Person[2, 3];
        persons[0, 1] = new Person('A', "Aaron");
        persons[1, 2] = new Person('B', "Bea");
        persons[1, 0] = new Person('C', "Charlie");

        var exportString = SudokuExporter.ExportPersons(persons);
        Assert.That(exportString, Is.EqualTo("""
                                             Persons:
                                              C
                                             A 
                                              B

                                             id,displayName
                                             A,Aaron
                                             B,Bea
                                             C,Charlie

                                             """));
    }

    [Test]
    public void ExportRooms()
    {
        var room1 = new Room('s', "Stube");
        var room2 = new Room('f', "Flur");

        var rooms = new Room[3, 2];
        rooms[0, 0] = room2;
        rooms[1, 0] = room2;
        rooms[2, 0] = room1;
        rooms[2, 1] = room1;
        rooms[1, 1] = room1;

        var exportString = SudokuExporter.ExportRooms(rooms);
        Assert.That(exportString, Is.EqualTo("""
                                             Rooms:
                                             ffs
                                              ss

                                             id,displayName
                                             f,Flur
                                             s,Stube

                                             """));
    }

    [Test]
    public void Export()
    {
        var persons = new Person[2, 1];
        persons[1, 0] = new Person('O', "Oleg");

        var rooms = new Room[2, 1];
        rooms[0, 0] = new Room('z', "Zelle");
        rooms[1, 0] = new Room('z', "Zelle");

        var sudoku = new Sudoku(2, 1, rooms, persons);
        var exportString = _exporter.ExportToString(sudoku);
        
        Assert.That(exportString, Is.EqualTo("""
                                             Persons:
                                              O

                                             id,displayName
                                             O,Oleg

                                             Rooms:
                                             zz

                                             id,displayName
                                             z,Zelle

                                             """));
    }
}