using Chirp.Razor;

namespace Razor.Tests;

[Collection("Sequential")]
public class CheepServiceTest
{

    [Theory]
    [InlineData(100)]
    public void TimeConversionTest(int a)
    {
        // Arrange
        var answer = DateTimeOffset.FromUnixTimeSeconds(a)
                                .ToString("MM/dd/yy H:mm:ss");

        // Act
        var result = CheepService.UnixTimeStampToDateTimeString(a);

        // Assert
        Assert.Equal(answer, result);
    }

    [Theory]
    [InlineData("test", "testing", 100)]
    public void CheepListToCheepDTOListTest(string a, string b, int c)
    {
        //Arrange
        List<Cheep> cheeps = [
            new Cheep() {
                Author = new Author() {Name = a},
                AuthorId = 0,
                Text = b,
                TimeStamp = DateTimeOffset.FromUnixTimeSeconds(c).UtcDateTime
            },
            new Cheep() {
                Author = new Author() {Name = b},
                AuthorId = 0,
                Text = a,
                TimeStamp = DateTimeOffset.FromUnixTimeSeconds(c).UtcDateTime
            }];
        List<CheepDTO> expectedDTO = [
            new CheepDTO() {
                Author = a,
                Message = b,
                Timestamp = CheepService.UnixTimeStampToDateTimeString(c)
            },
            new CheepDTO() {
                Author = b,
                Message = a,
                Timestamp = CheepService.UnixTimeStampToDateTimeString(c)
            }
            ];

        // Act
        List<CheepDTO> actualDTO = CheepService.CheepListToCheepDTOList(cheeps);

        //Assert
        TestUtils.AssertCheepDTOListsEqual(expectedDTO, actualDTO);
    }

    [Fact]
    public void ReadMessages_ReturnsFirstPage()
    {
        // Arrange
        TestUtils.SetupTestDb();
        var provider = TestUtils.SetupDIContainer();

        var service = new CheepService(provider);

        // Act
        var messages = service.GetCheeps();
        var messagesPage1 = service.GetCheeps(1);

        // Assert
        Assert.NotNull(messages);
        Assert.Equal(32, messages.Count());
        TestUtils.AssertCheepDTOListsEqual(messagesPage1, messages);
    }

    [Fact]
    public void ReadMessages_SpecificPage()
    {
        // Arrange
        TestUtils.SetupTestDb();
        var provider = TestUtils.SetupDIContainer();

        var service = new CheepService(provider);

        // Act
        var messagePage1 = service.GetCheeps(1);
        var messagesPage2 = service.GetCheeps(2);

        // Assert
        Assert.NotNull(messagePage1);
        Assert.Equal(32, messagesPage2.Count());
        Assert.NotEqual(messagePage1, messagesPage2);
    }

    [Fact]
    public void ReadMessages_SpecificUser()
    {
        // Arrange
        TestUtils.SetupTestDb();
        var provider = TestUtils.SetupDIContainer();

        var service = new CheepService(provider);

        // Act
        var messagesUser = service.GetCheepsFromAuthor("Jacqualine Gilcoine");
        var messages = service.GetCheeps();

        // Assert
        Assert.NotNull(messagesUser);
        Assert.Equal(32, messagesUser.Count());
        Assert.NotEqual(messagesUser, messages);
        foreach (CheepDTO cheep in messagesUser)
        {
            Assert.Equal("Jacqualine Gilcoine", cheep.Author);
        }

    }

    [Fact]
    public void ReadMessages_SpecificUserAndPage()
    {
        // Arrange
        TestUtils.SetupTestDb();
        var provider = TestUtils.SetupDIContainer();

        var service = new CheepService(provider);

        // Act
        var messagesUser = service.GetCheepsFromAuthor("Jacqualine Gilcoine");
        var messagesUserPage2 = service.GetCheepsFromAuthor("Jacqualine Gilcoine", 2);

        // Assert
        Assert.NotNull(messagesUserPage2);
        Assert.Equal(32, messagesUserPage2.Count());
        Assert.NotEqual(messagesUserPage2, messagesUser);

        foreach (CheepDTO cheep in messagesUserPage2)
        {
            Assert.Equal("Jacqualine Gilcoine", cheep.Author);
        }
    }

    [Fact]
    public void ReadMessages_NonExistingUser()
    {
        // Arrange
        TestUtils.SetupTestDb();
        var provider = TestUtils.SetupDIContainer();

        var service = new CheepService(provider);

        // Act
        var messagesUser = service.GetCheepsFromAuthor("This user does not exist");

        // Assert
        Assert.Empty(messagesUser);
    }

    [Fact]
    public void ReadMessages_NonExistingPage()
    {
        // Arrange
        TestUtils.SetupTestDb();
        var provider = TestUtils.SetupDIContainer();

        var service = new CheepService(provider);

        // Act
        var messagesUser = service.GetCheeps(1000000000);

        // Assert
        Assert.Empty(messagesUser);
    }

}