// using SudokuMasterBase.Serialization;
//
// namespace SudokuMasterBase.Tests.Serialization;
//
// public class SudokuSerializationTest
// {
//     private readonly SudokuImporter _importer = new();
//     private readonly SudokuExporter _exporter = new();
//
//     private const string Example = """
//         Persons:
//         E....
//         ..D..
//         ...A.
//         ....C
//         .B...
//
//         id,name
//         A,arthur
//         B,bea
//         C,charlie
//         D,dora
//         E,erik
//
//         Rooms:
//         aabbb
//         aabbb
//         cccbb
//         cccdd
//         cccdd
//
//         id,name
//         a,wohnzimmer
//         b,flur
//         c,küche
//         d,bad
//         """;
//
//     [Test]
//     public void ImportExample()
//     {
//         var sudoku = _importer.Import(Example);
//         Assert.That(sudoku.Width, Is.EqualTo(5));
//         Assert.That(sudoku.Height, Is.EqualTo(5));
//         Assert.That(sudoku.Persons[3, 2]?.Name, Is.EqualTo("arthur"));
//         Assert.That(sudoku.Persons[1, 4]?.Name, Is.EqualTo("bea"));
//         Assert.That(sudoku.Persons[0, 1], Is.Null);
//         Assert.That(sudoku.Rooms[0, 0].Name, Is.EqualTo("wohnzimmer"));
//         Assert.That(sudoku.Rooms[1, 1], Is.SameAs(sudoku.Rooms[0, 0]));
//     }
//
//     [Test]
//     public void RoundTripPreservesAllCells()
//     {
//         var original = _importer.Import(Example);
//         var copy = _importer.Import(_exporter.ExportToString(original));
//         for (var y = 0; y < original.Height; y++)
//         for (var x = 0; x < original.Width; x++)
//         {
//             Assert.That(copy.Persons[x, y], Is.EqualTo(original.Persons[x, y]));
//             Assert.That(copy.Rooms[x, y], Is.EqualTo(original.Rooms[x, y]));
//         }
//         Assert.That(_exporter.ExportToString(copy), Is.EqualTo(_exporter.ExportToString(original)));
//     }
//
//     [TestCase("")]
//     [TestCase(" leading and trailing ")]
//     [TestCase("bea, arthur")]
//     [TestCase("a \"quoted\" name")]
//     [TestCase("first\nsecond")]
//     [TestCase("first\r\nsecond")]
//     [TestCase("küche 🦥")]
//     public void RoundTripCsvNames(string name)
//     {
//         var sudoku = new Sudoku(1, 1);
//         sudoku.Persons[0, 0] = new Person(name);
//         sudoku.Rooms[0, 0] = new Room(name);
//         var copy = _importer.Import(_exporter.ExportToString(sudoku));
//         Assert.That(copy.Persons[0, 0]?.Name, Is.EqualTo(name));
//         Assert.That(copy.Rooms[0, 0].Name, Is.EqualTo(name));
//     }
//
//     [Test]
//     public void RectangularGridAndEmptyCells()
//     {
//         var sudoku = new Sudoku(3, 2);
//         sudoku.Persons[2, 1] = new Person("bea");
//         var copy = _importer.Import(_exporter.ExportToString(sudoku));
//         Assert.That(copy.Width, Is.EqualTo(3));
//         Assert.That(copy.Height, Is.EqualTo(2));
//         Assert.That(copy.Persons[2, 1]?.Name, Is.EqualTo("bea"));
//         Assert.That(copy.Persons[1, 0], Is.Null);
//         Assert.That(copy.Rooms[0, 0], Is.Null);
//     }
//
//     [Test]
//     public void DistinctEqualObjectsRemainDistinctAndSharedObjectsRemainShared()
//     {
//         var sudoku = new Sudoku(3, 1);
//         var room = new Room("same");
//         sudoku.Rooms[0, 0] = room;
//         sudoku.Rooms[1, 0] = new Room("same");
//         sudoku.Rooms[2, 0] = room;
//         var copy = _importer.Import(_exporter.ExportToString(sudoku));
//         Assert.That(copy.Rooms[0, 0], Is.Not.SameAs(copy.Rooms[1, 0]));
//         Assert.That(copy.Rooms[2, 0], Is.SameAs(copy.Rooms[0, 0]));
//     }
//
//     [TestCase("\n")]
//     [TestCase("\r\n")]
//     public void AcceptsBomAndLineEndings(string newline)
//     {
//         var text = "\uFEFF" + Example.Replace("\r\n", "\n").Replace("\n", newline);
//         Assert.That(_importer.Import(text).Width, Is.EqualTo(5));
//     }
//
//     [Test]
//     public void RejectsUnknownId() =>
//         Assert.Throws<FormatException>(() => _importer.Import(Example.Replace("A,arthur", "Z,arthur")));
//
//     [Test]
//     public void RejectsDuplicateId() =>
//         Assert.Throws<FormatException>(() => _importer.Import(Example.Replace("B,bea", "A,bea")));
//
//     [Test]
//     public void RejectsRaggedGrid() =>
//         Assert.Throws<FormatException>(() => _importer.Import(Example.Replace("E....", "E...")));
//
//     [Test]
//     public void RejectsDifferentDimensions() =>
//         Assert.Throws<FormatException>(() => _importer.Import(Example.Replace("aabbb\n", "")));
//
//     [TestCase("id,title")]
//     [TestCase("id,name,extra")]
//     public void RejectsInvalidHeader(string header) =>
//         Assert.Throws<FormatException>(() => _importer.Import(Example.Replace("id,name", header)));
//
//     [TestCase("A,\"unterminated")]
//     [TestCase("A,ar\"thur")]
//     [TestCase("A,\"arthur\"x")]
//     [TestCase("A,arthur,extra")]
//     public void RejectsMalformedCsv(string row) =>
//         Assert.Throws<FormatException>(() => _importer.Import(Example.Replace("A,arthur", row)));
//
//     [TestCase("")]
//     [TestCase("Persons:\n\nid,name\n\nRooms:\n.\n\nid,name")]
//     public void RejectsEmptyInputOrGrid(string text) =>
//         Assert.Throws<FormatException>(() => _importer.Import(text));
//
//     [Test]
//     public void RejectsTooManyObjects()
//     {
//         var sudoku = new Sudoku(63, 1);
//         for (var x = 0; x < 63; x++) sudoku.Persons[x, 0] = new Person(x.ToString());
//         Assert.Throws<ArgumentException>(() => _exporter.ExportToString(sudoku));
//     }
//
//     [Test]
//     public void RejectsMismatchedArrayDimensions() =>
//         Assert.Throws<ArgumentException>(() => _exporter.ExportToString(new Sudoku(2, 1, persons: new Person?[1, 1])));
//
//     [Test]
//     public void ReaderAndWriterRemainOpen()
//     {
//         using var reader = new StringReader(Example);
//         var sudoku = _importer.Import(reader);
//         Assert.That(reader.Peek(), Is.EqualTo(-1));
//         using var writer = new StringWriter();
//         _exporter.Export(sudoku, writer);
//         Assert.DoesNotThrow(() => writer.Write("still open"));
//     }
// }
