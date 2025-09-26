namespace SimpleDB.Tests;

public class CSVDatabaseUnitTest
{
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
}