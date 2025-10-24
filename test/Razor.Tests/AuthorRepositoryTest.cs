using Microsoft.Extensions.DependencyInjection;

namespace Razor.Tests;
[Collection("Sequential")]
public class AuthorRepositoryUnitTest
{
    [Fact]
    public async Task CreateAuthorTest()
    {
        // Arrange
        TestUtils.SetupTestDb();
        var provider = TestUtils.SetupDIContainer();
        var repository = provider.GetRequiredService<IAuthorRepository>();

        var author = new Author
        {
            AuthorId = 500,
            Name = "testing"
        };
        // Act
        await repository.CreateAuthor(author);

        // Assert
        //Don't know how to test without a find author by name method
    }
}