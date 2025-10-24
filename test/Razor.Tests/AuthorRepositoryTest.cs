using Microsoft.Extensions.DependencyInjection;

namespace Razor.Tests;

[Collection("Sequential")]
public class AuthorRepositoryUnitTest
{
    [Fact]
    public async Task FindAuthorByNameTest()
    {
        // Arrange
        TestUtils.SetupTestDb();
        var provider = TestUtils.SetupDIContainer();
        var repository = provider.GetRequiredService<IAuthorRepository>();

        //Act
        Author author = await repository.FindAuthorByName("Jacqualine Gilcoine");

        // Assert
        Assert.Equal("Jacqualine Gilcoine", author.Name);
        Assert.Equal(10, author.AuthorId);
    }

    [Fact]
    public async Task FindAuthorByEmailTest()
    {
        // Arrange
        TestUtils.SetupTestDb();
        var provider = TestUtils.SetupDIContainer();
        var repository = provider.GetRequiredService<IAuthorRepository>();

        //Act
        Author author = await repository.FindAuthorByEmail("Jacqualine.Gilcoine@gmail.com");

        // Assert
        Assert.Equal("Jacqualine Gilcoine", author.Name);
        Assert.Equal("Jacqualine.Gilcoine@gmail.com", author.Email);
        Assert.Equal(10, author.AuthorId);
    }
    [Fact]
    public async Task CreateAuthorTest()
    {
        // Arrange
        TestUtils.SetupTestDb();
        var provider = TestUtils.SetupDIContainer();
        var repository = provider.GetRequiredService<IAuthorRepository>();

        var authorExpected = new Author
        {
            AuthorId = 500,
            Name = "testing"
        };
        // Act
        await repository.CreateAuthor(authorExpected);
        Author authorFound = await repository.FindAuthorByName("testing");

        // Assert
        Assert.Equal(authorExpected, authorFound);

    }
}