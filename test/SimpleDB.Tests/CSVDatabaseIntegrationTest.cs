namespace SimpleDB.Tests;

[Collection("CSVDatabase")]
public class CSVDatabaseIntegrationTest
{
    [Fact]
    public void ReadWriteTest()
    {
        // Arrange
        File.Delete("chirp_cli_db.csv");
        TestRecord test = new TestRecord("Test!!!!");

        // Act
        IDatabaseRepository<TestRecord> db = CSVDatabase<TestRecord>.Instance;
        db.Store(test);
        TestRecord result = db.Read(1).ElementAt(0);

        // Assert
        Assert.Equal(test, result);
    }
}