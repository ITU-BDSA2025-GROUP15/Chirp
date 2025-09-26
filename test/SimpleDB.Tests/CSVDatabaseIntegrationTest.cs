namespace SimpleDB.Tests;

public class CSVDatabaseIntegrationTest
{
    [Fact]
    public void ReadWriteTest()
    {
        IDatabaseRepository<TestRecord> db = CSVDatabase<TestRecord>.Instance;

        TestRecord test = new TestRecord("Test!!!!");

        db.Store(test);
        Assert.Equal(test, db.Read(1).ElementAt(0));

    }
}