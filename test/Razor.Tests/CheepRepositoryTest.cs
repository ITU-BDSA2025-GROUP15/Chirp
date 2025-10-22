using System.Threading.Tasks;

using Chirp.Razor;

using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Razor.Tests;

[Collection("Sequential")]
public class CheepRepositoryUnitTest
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
    public async Task ReadMessages_ReturnsFirstPage()
    {
        // Arrange
        TestUtils.SetupTestDb();
        var provider = TestUtils.SetupDIContainer();
        var repository = provider.GetRequiredService<ICheepRepository>();

        // Act
        var messages = await repository.ReadMessages(null, null, null); 

        // Assert
        Assert.NotNull(messages);
        Assert.Equal(32, messages.Count());
    }

    [Fact]
    public async Task ReadMessages_WithPageArgument_ReturnsFirstPage()
    {
        // Arrange
        TestUtils.SetupTestDb();
        var provider = TestUtils.SetupDIContainer();
        var repository = provider.GetRequiredService<ICheepRepository>();

        // Act
        var messages = await repository.ReadMessages(null, null, null); 
        var messages_arg = await repository.ReadMessages(null, 1, null); 

        // Assert
        Assert.NotNull(messages);
        Assert.NotNull(messages_arg);
        TestUtils.AssertCheepDTOListsEqual(messages, messages_arg);
    }

    [Theory]
    [MemberData(nameof(RandomNumber))]
    public async Task ReadMessages_ReturnsPage(int pageNumber) {
        // Arrange
        TestUtils.SetupTestDb();
        var provider = TestUtils.SetupDIContainer();
        var repository = provider.GetRequiredService<ICheepRepository>(); 

        // Act
        var messagesAll = await repository.ReadMessages(null, 1, int.MaxValue);
        var messagesPage = await repository.ReadMessages(null, pageNumber, null);

        var messagesAllSliced = new List<CheepDTO>();
        var startCheep = (pageNumber - 1) * 32;
        for (int i = startCheep; i < startCheep + 32 && i < messagesAll.Count(); i++)
        {
            messagesAllSliced.Add(messagesAll[i]);
        }

        // Assert
        TestUtils.AssertCheepDTOListsEqual(messagesAllSliced, messagesPage);
    }
}