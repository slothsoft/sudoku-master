using SudokuMasterGenerator.Generator;

namespace SudokuMasterGenerator.Tests.Generator;

public class SudokuGeneratorTest
{
    private readonly SudokuGenerator _generator = new();
    
    [Test]
    public void TestGenerateSize1x1()
    {
        _generator.Size = 1;
        
        var sudoku = _generator.Generate();
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(sudoku.Width, Is.EqualTo(1));
            Assert.That(sudoku.Height, Is.EqualTo(1));
            
            var singlePerson = sudoku.Persons[0, 0];
            Assert.That(singlePerson, Is.Not.Null);
            Assert.That(singlePerson.Name, Is.EqualTo("A"));
        }
    }
    
    [Test]
    public void TestGenerateSize1x2()
    {
        _generator.Width = 1;
        _generator.Height = 2;
        
        var sudoku = _generator.Generate();
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(sudoku.Width, Is.EqualTo(1));
            Assert.That(sudoku.Height, Is.EqualTo(2));
            
            var personOrNull = sudoku.Persons[0, 0];
            var nullOrPerson = sudoku.Persons[0, 1];
            
            Assert.That(personOrNull is null, Is.Not.EqualTo(nullOrPerson is null));
            
            var person = personOrNull ?? nullOrPerson;
            Assert.That(person, Is.Not.Null);
            Assert.That(person.Name, Is.EqualTo("A"));
        }
    }
    
    [Test]
    public void TestGenerateSize2x1()
    {
        _generator.Width = 2;
        _generator.Height = 1;
        
        var sudoku = _generator.Generate();
        
        using (Assert.EnterMultipleScope())
        {
            Assert.That(sudoku.Width, Is.EqualTo(2));
            Assert.That(sudoku.Height, Is.EqualTo(1));
            
            var personOrNull = sudoku.Persons[0, 0];
            var nullOrPerson = sudoku.Persons[1, 0];
            
            Assert.That(personOrNull is null, Is.Not.EqualTo(nullOrPerson is null));
            
            var person = personOrNull ?? nullOrPerson;
            Assert.That(person, Is.Not.Null);
            Assert.That(person.Name, Is.EqualTo("A"));
        }
    }
}