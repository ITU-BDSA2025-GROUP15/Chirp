namespace Chirp.Tests;

public class CheepTest
{
    [Fact]
    public void Cheep_InstanceTest()
    {
        var c = new Cheep("user", "swag", 1000);

        Assert.Equal("user", c.Author);
        Assert.Equal("swag", c.Message);
        Assert.Equal(1000, c.Timestamp);
    }
}