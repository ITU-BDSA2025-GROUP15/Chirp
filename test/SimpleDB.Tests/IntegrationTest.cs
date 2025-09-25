namespace SimpleDB.Tests;


public class CSVDatabaseTest
{
    [Fact]
    public void IntegrationTest()
    {
        IDatabaseRepository<TestRecord> db = CSVDatabase<TestRecord>.Instance;

        TestRecord test = new TestRecord("Test!!!!");

        db.Store(test);
        Assert.Equal(test, db.Read(1).ElementAt(0));

    }
}

public record TestRecord(string name);