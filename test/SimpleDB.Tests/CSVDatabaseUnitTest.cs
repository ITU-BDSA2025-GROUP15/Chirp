namespace SimpleDB.Tests;

[Collection("CSVDatabase")]
public class CSVDatabaseUnitTest
{
    static readonly string db_filename = "chirp_cli_db.csv";
    static void CreateTestFile()
    {
        File.WriteAllText(db_filename, @"Author,Message,Timestamp
ropf,""Hello, BDSA students!"",1690891760
adho,""Welcome to the course!"",1690978778
adho,""I hope you had a good summer."",1690979858
ropf,""Cheeping cheeps on Chirp :)"",1690981487
");
    }

    [Fact]
    public void Instance_IsSingleton()
    {
        // Act
        var db1 = CSVDatabase<TestRecord>.Instance;
        var db2 = CSVDatabase<TestRecord>.Instance;

        // Assert
        Assert.NotNull(db1);
        Assert.NotNull(db2);
        Assert.Same(db1, db2);
    }

    [Fact]
    public void ReadNull_ReturnsAllCheeps()
    {
        // Arrange
        CreateTestFile();
        var expected = new List<Cheep> {
            new Cheep("ropf", "Hello, BDSA students!", 1690891760),
            new Cheep("adho", "Welcome to the course!", 1690978778),
            new Cheep("adho", "I hope you had a good summer.", 1690979858),
            new Cheep("ropf", "Cheeping cheeps on Chirp :)", 1690981487)
        };

        // Act
        var db = CSVDatabase<Cheep>.Instance;
        var result = db.Read(null);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ReadOne_ReturnsLastCheep()
    {
        // Arrange
        CreateTestFile();
        var expected = new List<Cheep> {
            new Cheep("ropf", "Cheeping cheeps on Chirp :)", 1690981487)
        };

        // Act
        var db = CSVDatabase<Cheep>.Instance;
        var result = db.Read(1);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("demo", "Testing write to an existing file.", 1234321)]
    public void Write_ToExistingFile(string author, string message, int timestamp)
    {
        // Arrange
        CreateTestFile();
        var cheep = new Cheep(author, message, timestamp);
        var acceptableLines = new List<string> {
            $"{author},{message},{timestamp}",
            $"\"{author}\",{message},{timestamp}",
            $"{author},\"{message}\",{timestamp}",
            $"\"{author}\",\"{message}\",{timestamp}"
        };

        // Act
        var db = CSVDatabase<Cheep>.Instance;
        db.Store(cheep);
        var lastLine = File.ReadLines(db_filename).Last();

        // Assert
        Assert.Contains(lastLine, acceptableLines);
    }

    [Theory]
    [InlineData("demo", "Testing write without an existing file.", 4321234)]
    public void Write_CreatesNewFile(string author, string message, int timestamp)
    {
        // Arrange
        File.Delete(db_filename);
        var cheep = new Cheep(author, message, timestamp);
        var expectedHeader = "author,message,timestamp";
        var acceptableLines = new List<string> {
            $"{author},{message},{timestamp}",
            $"\"{author}\",{message},{timestamp}",
            $"{author},\"{message}\",{timestamp}",
            $"\"{author}\",\"{message}\",{timestamp}"
        };

        // Act
        var db = CSVDatabase<Cheep>.Instance;
        db.Store(cheep);
        var firstLine = File.ReadLines(db_filename).First().ToLower();
        var lastLine = File.ReadLines(db_filename).Last();

        // Assert
        Assert.True(File.Exists(db_filename));
        Assert.Equal(expectedHeader, firstLine);
        Assert.Contains(lastLine, acceptableLines);
    }
}