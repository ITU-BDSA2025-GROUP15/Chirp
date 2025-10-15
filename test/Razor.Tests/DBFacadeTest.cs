using Chirp.Razor;

using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Razor.Tests;

[Collection("Sequential")]
public class DBFacadeUnitTest
{
    public static IEnumerable<object[]> RandomNumber
    {
        get
        {
            Random rnd = new Random();
            var number_list = new List<object[]>();

            var count = 100;
            for (int i = 0; i < count; i++)
            {
                number_list.Add([rnd.Next(1, 100)]);
            }

            return number_list;
        }
    }
    
    [Fact]
    public void GetConnection_UsesDefaultDataSource()
    {
        // Arrange
        Environment.SetEnvironmentVariable("CHIRPDBPATH", null);
        var tempdir = Path.Combine(Path.GetTempPath(), "chirp.db");
        var correctDataSource = $"Data Source={tempdir}";

        // Act 
        var connection = DBFacade.GetConnection();
        var connectionString = connection.ConnectionString;

        // Assert
        Assert.Contains(connectionString, correctDataSource);
    }

    [Theory]
    [InlineData("/test/test.db")]
    public void GetConnection_UsesEnvironmentVariableDataSource(string path)
    {
        // Arrange
        Environment.SetEnvironmentVariable("CHIRPDBPATH", path);
        var correctDataSource = $"Data Source={path}";

        // Act 
        var connection = DBFacade.GetConnection();
        var connectionString = connection.ConnectionString;

        // Assert
        Assert.Contains(connectionString, correctDataSource);
    }

    [Fact]
    public void ReadMessages_ReturnsFirstPage()
    {
        // Arrange
        TestUtils.SetupTestDb();

        // Act
        var messages = DBFacade.ReadMessages();

        // Assert
        Assert.NotNull(messages);
        Assert.Equal(32, messages.Count());
    }

    [Fact]
    public void ReadMessages_WithPageArgument_ReturnsFirstPage()
    {
        // Arrange
        TestUtils.SetupTestDb();

        // Act
        var messages = DBFacade.ReadMessages();
        var messages_arg = DBFacade.ReadMessages(1);

        // Assert
        Assert.NotNull(messages);
        Assert.NotNull(messages_arg);
        for (int i = 0; i < messages.Count; i++)
        {
            Assert.Equal(messages[i].Author.Name, messages_arg[i].Author.Name);
            Assert.Equal(messages[i].Text, messages_arg[i].Text);
            Assert.Equal(messages[i].TimeStamp.ToString(), messages_arg[i].TimeStamp.ToString());
        }
    }

    [Theory]
    [MemberData(nameof(RandomNumber))]
    private void ReadMessages_ReturnsPage(int pageNumber) {
        // Arrange
        TestUtils.SetupTestDb();

        // Act
        var messages = DBFacade.ReadMessages(1,null);
        var messages_pages = DBFacade.ReadMessages(pageNumber);
        var messages_slice = new List<Cheep>();

        var startCheep = (pageNumber - 1) * 32;
        for (int i = startCheep; i < startCheep + 32 && i < messages.Count(); i++)
        {
            messages_slice.Add(messages[i]);
        }

        // Assert
        Assert.Equal(messages_slice,messages_pages);
    }
}