using Microsoft.Playwright;
using Xunit;
using System.Threading.Tasks;

namespace PlaywrightTests;

public class CheepUITests : IClassFixture<TestServerFixture>
{
    private readonly TestServerFixture _fixture;

    public CheepUITests(TestServerFixture fixture)
    {
        _fixture = fixture;
    }

    private async Task LoginAsync(IPage page)
    {
        await page.GotoAsync($"{_fixture.BaseUrl}/login");

        await page.FillAsync("input[name='Input.UserName']", "test");
        await page.FillAsync("input[name='Input.Password']", "Password123!");
        await page.ClickAsync("button[type='submit']");
    }

    [Fact]
    public async Task PostInputVisibleOnlyAfterLogin()
    {
        var page = await _fixture.Browser.NewPageAsync();
        await page.GotoAsync(_fixture.BaseUrl);

        Assert.False(await page.IsVisibleAsync("#post-input"));

        await LoginAsync(page);

        Assert.True(await page.IsVisibleAsync("#post-input"));
    }

    [Fact]
    public async Task CheepsDisplayedAfterPosting()
    {
        var page = await _fixture.Browser.NewPageAsync();
        await LoginAsync(page);

        await page.FillAsync("#post-input", "Hello from Playwright!");
        await page.ClickAsync("#post-button");

        Assert.True(await page.IsVisibleAsync("text=Hello from Playwright!"));
    }

    [Fact]
    public async Task CannotPostCheepLongerThan160Chars()
    {
        var page = await _fixture.Browser.NewPageAsync();
        await LoginAsync(page);

        string longText = new string('a', 200);
        await page.FillAsync("#post-input", longText);
        await page.ClickAsync("#post-button");

        Assert.True(await page.IsVisibleAsync("text=Cheep cannot be longer than 160 characters"));
    }
}
