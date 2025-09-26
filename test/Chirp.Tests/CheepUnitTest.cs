namespace Chirp.Tests;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

[Collection("Sequential")]
public class ChirpUnitTest
{
    public static IEnumerable<object[]> Cheeps {
        get
        {
            int cheeps = 10;

            var result = new List<object[]>();

            for (int j = 0; j < cheeps; j++)
            {
                var rand = new Random();

                var cheep = new Cheep(
                    GenRandomString(rand.Next(1,100)), 
                    GenRandomString(rand.Next(1,200)),
                    rand.Next(100000000)
                );
                result.Add(new object[] {cheep});
            }  
            
            return result;
        }    
    }
    public static IEnumerable<object[]> CheepLists
    {
        get
        {
            int list_count = 5;
            int cheeps_per_list = 10;

            var result = new List<object[]>();

            for (int i = 0; i < list_count; i++)
            {
                var list = new List<Cheep>();
                for (int j = 0; j < cheeps_per_list; j++)
                {
                    var rand = new Random();

                    var cheep = new Cheep(
                        GenRandomString(rand.Next(1,100)), 
                        GenRandomString(rand.Next(1,200)),
                        rand.Next(100000000)
                    );
                    list.Add(cheep);
                }  

                result.Add(new object[] {list});  
            }
            
            return result;
        }
    }

    public static string GenRandomString(int length) {
        StringBuilder strB = new StringBuilder();
        Random rand = new Random();
        for (int i = 0; i < length; i++)
        {
            int num = rand.Next(100);
            char letter = Convert.ToChar(num);
            strB.Append(letter);
        }

        return strB.ToString();
    }

    [Fact]
    public void Cheep_InstanceTest()
    {
        var c = new Cheep("user", "swag", 1000);

        Assert.Equal("user", c.Author);
        Assert.Equal("swag", c.Message);
        Assert.Equal(1000, c.Timestamp);
    }

    [Theory]
    [MemberData(nameof(CheepLists))]
    public void PrintCheepsTest(IEnumerable<Cheep> cheepList)
    {
        var sw = new StringWriter();
        Console.SetOut(sw);

        UserInterface.PrintCheeps(cheepList);
        var result = sw.ToString();
        StringBuilder strB = new StringBuilder();
        StringBuilder strC = new StringBuilder();
        foreach (var cheep in cheepList)
        {
            strB.Append(cheep.Author + " @ " + UserInterface.ToLocalTimeString(cheep.Timestamp) + ": " + cheep.Message + "\r\n");
            strC.Append(cheep.Author + " @ " + UserInterface.ToLocalTimeString(cheep.Timestamp) + ": " + cheep.Message + "\n");
        }
        Assert.True(strB.ToString()==result || strC.ToString()==result);
    }

    [Theory]
    [MemberData(nameof(Cheeps))]
    public void TimeTest(Cheep c)
    {

        var ConversionTest = UserInterface.ToLocalTimeString(c.Timestamp);
        var Answer = DateTimeOffset.FromUnixTimeSeconds(c.Timestamp)
                                .ToLocalTime()
                                .ToString("MM/dd/yy HH:mm:ss");

        Assert.Equal(Answer, ConversionTest);
    }
}
// Add unit tests for suitable functionality. 
// For example, conversion of UNIX timestamps to user readable times
// and similar functionality are good candidates for unit testing.
// what should we test for:
