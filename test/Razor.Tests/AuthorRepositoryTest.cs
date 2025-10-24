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
        try
        {
            // Act
            await repository.CreateAuthor(authorExpected);
            Author authorFound = await repository.FindAuthorByName("testing");

            // Assert
            Assert.Equal(authorExpected, authorFound);
        }
        catch (Exception)
        {
            //clean up in case of exception
            await repository.RemoveAuthor(authorExpected);
        }
        finally
        {
            //clean up after test
            await repository.RemoveAuthor(authorExpected);
        }
    }
}