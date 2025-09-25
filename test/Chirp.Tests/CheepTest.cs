namespace Chirp.Tests;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
public class ChirpTest
{
    [Fact]
    public void Cheep_InstanceTest()
    {
        var c = new Cheep("user", "swag", 1000);

        Assert.Equal("user", c.Author);
        Assert.Equal("swag", c.Message);
        Assert.Equal(1000, c.Timestamp);
    }

    [Theory]
    [InlineData("user", "Dolp", 0)]
    [InlineData("test", "whatever", 5)]
    public void TimeTest(string a, string b, int c)
    {
        var newcheep = new Cheep(a, b, c);

        var ConversionTest = UserInterface.ToLocalTimeString(newcheep.Timestamp);
        var Answer = DateTimeOffset.FromUnixTimeSeconds(c)
                                .ToLocalTime()
                                .ToString("MM/dd/yy HH:mm:ss");

        Assert.Equal(Answer, ConversionTest);
    }
}
// Add unit tests for suitable functionality. 
// For example, conversion of UNIX timestamps to user readable times
// and similar functionality are good candidates for unit testing.
// what should we test for:
