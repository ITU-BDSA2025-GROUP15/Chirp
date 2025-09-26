namespace SimpleDB.Tests;

public class CSVDatabaseIntegrationTest
{
    [Fact]
    public void ReadWriteTest()
    {
        // Arrange
        TestRecord test = new TestRecord("Test!!!!");

        // Act
        IDatabaseRepository<TestRecord> db = CSVDatabase<TestRecord>.Instance;
        db.Store(test);
        TestRecord result = db.Read(1).ElementAt(0);

        // Assert
        Assert.Equal(test, result);
    }
}