using System.Diagnostics;
using System.Text.RegularExpressions;

using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;

using Xunit.Abstractions;
public class RazorPageFixture : IAsyncLifetime
{
    Process? _razorPage;
    public required HttpClient Client;

    readonly IBrowserContext[] _contexts = new IBrowserContext[3];

    IPlaywright? _playwright;

    readonly IBrowser[] _browsers = new IBrowser[3];

    public required IPage[] Pages = new IPage[3];
    public async Task InitializeAsync()
    {
        _razorPage = await TestUtils.StartRazorPage();
        var baseURL = "http://localhost:5273/";
        Client = new();
        Client.BaseAddress = new Uri(baseURL);

        _playwright = await Playwright.CreateAsync();
        int i = 0;
        foreach (var browserName in new string[]{"Chromium", "Firefox", "Webkit"})
        {
            _browsers[i] = await _playwright[browserName].LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });
            _contexts[i] = await _browsers[i].NewContextAsync();
            Pages[i] = await _contexts[i].NewPageAsync();
            await Pages[i].GotoAsync("http://localhost:5273/");
            i++;
        }
    }
    public Task DisposeAsync()
    {
        _playwright!.Dispose();
        Client.Dispose();
        _razorPage!.Kill(true);
        _razorPage.WaitForExit();
        _razorPage.Dispose();
        return Task.CompletedTask;
    }
}

[Collection("Sequential")]
public class End2EndTests : IClassFixture<RazorPageFixture>
{
    public readonly string Razor_path = "src/Chirp.Razor/Chirp.Razor.csproj";

    readonly RazorPageFixture _fixture;

    //for debugging test
    /*
    private readonly ITestOutputHelper _output;

    public End2EndTests(ITestOutputHelper output)
    {
        _output = output;
    }
    */
    public End2EndTests(RazorPageFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Page1AndDefaultEqual()
    {
        // Act
        // Default page
        var HTTPResponsePageDefault = await _fixture.Client.GetAsync("/");
        string responseBodyPageDefault = await HTTPResponsePageDefault.Content.ReadAsStringAsync();

        // Page 1
        var HTTPResponsePage1 = await _fixture.Client.GetAsync("/?page=1");
        string responseBodyPage1 = await HTTPResponsePage1.Content.ReadAsStringAsync();

        // Assert
        // Page 1 and default page is the same
        Assert.Equal(responseBodyPageDefault, responseBodyPage1);
    }

    [Fact]
    public async Task CheepThatShouldBeOnPage1()
    {
        //Act
        var HTTPResponsePageDefault = await _fixture.Client.GetAsync("/");
        string responseBodyPageDefault = await HTTPResponsePageDefault.Content.ReadAsStringAsync();

        //Assert
        // Cheep that should be on the first page
        Assert.Contains("Starbuck now is what we hear the worst.", responseBodyPageDefault);
    }

    [Fact]
    public async Task CheepThatShouldBeOnPage2()
    {
        //Act
        //Page 2
        var HTTPResponsePage2 = await _fixture.Client.GetAsync("/?page=2");
        string responseBodyPage2 = await HTTPResponsePage2.Content.ReadAsStringAsync();

        Assert.Contains("It is asking much of it in the world.", responseBodyPage2);
        Assert.Contains("Jacqualine Gilcoine", responseBodyPage2);
    }

    [Fact]
    public async Task Page1and2AreNotTheSame()
    {
        //Act
        var HTTPResponsePageDefault = await _fixture.Client.GetAsync("/");
        string responseBodyPageDefault = await HTTPResponsePageDefault.Content.ReadAsStringAsync();
        //Page 2
        var HTTPResponsePage2 = await _fixture.Client.GetAsync("/?page=2");
        string responseBodyPage2 = await HTTPResponsePage2.Content.ReadAsStringAsync();


        //Assert
        // Page 1 and 2 not equal
        Assert.NotEqual(responseBodyPageDefault, responseBodyPage2);
    }

    [Fact]
    public async Task AdrianHtmlMessage()
    {
        // Arrange
        var expectedDateTime = DateTime.Parse("2023-08-01 13:08:28");
        var expectedDateTimeStr = expectedDateTime.ToString("MM/dd/yy H:mm:ss");
        var expectedFullStr = $"<strong><a href=\"/Adrian\">Adrian</a></strong>Hej, velkommen til kurset.<small>&mdash; {expectedDateTimeStr}";

        // User page Adrian
        var HTTPResponseUser = await _fixture.Client.GetAsync("/Adrian");
        string responseBodyUser = await HTTPResponseUser.Content.ReadAsStringAsync();

        responseBodyUser = responseBodyUser.Replace("\r\n", "\n"); // In case of Windows users
        responseBodyUser = Regex.Replace(responseBodyUser, "\n\\s*", ""); // Strip leading whitespace

        // Only Adrians posts should be on the user page
        // _output.WriteLine(responseBodyUser);
        Assert.Contains(expectedFullStr, responseBodyUser);
        Assert.DoesNotContain("Jacqualine Gilcoine", responseBodyUser);
    }

    [Theory]
    [MemberData(nameof(BrowserTypes.Number), MemberType = typeof(BrowserTypes))]
    public async Task loginLogoutChirp(int browser)
    {
        //tries to login with incorrect password.
        await _fixture.Pages[browser].GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
        await _fixture.Pages[browser].GetByRole(AriaRole.Textbox, new() { Name = "Email" }).ClickAsync();
        await _fixture.Pages[browser].GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("adho@itu.dk");
        await _fixture.Pages[browser].GetByRole(AriaRole.Textbox, new() { Name = "Password" }).ClickAsync();
        await _fixture.Pages[browser].GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("LetM31n!");
        await _fixture.Pages[browser].GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();

        Assert.Equal("Invalid login attempt.", await _fixture.Pages[browser].GetByText("Invalid login attempt.").InnerTextAsync());

        //Correct password
        await _fixture.Pages[browser].GetByRole(AriaRole.Textbox, new() { Name = "Password" }).ClickAsync();
        await _fixture.Pages[browser].GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("M32Want_Access");
        await _fixture.Pages[browser].GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await _fixture.Pages[browser].GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();

        //My timeline is Adrian's Timeline
        Assert.Equal("Adrian's Timeline", await _fixture.Pages[browser].GetByRole(AriaRole.Heading, new() { Name = "Adrian's Timeline" }).InnerTextAsync());

        //logs out
        await _fixture.Pages[browser].GetByRole(AriaRole.Button, new() { Name = "logout [Adrian]" }).ClickAsync();
    }
    [Theory]
    [MemberData(nameof(BrowserTypes.Number), MemberType = typeof(BrowserTypes))]
    public async Task PostCheep(int browser)
    {
        //logs in
        await _fixture.Pages[browser].GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
        await _fixture.Pages[browser].GetByRole(AriaRole.Textbox, new() { Name = "Email" }).ClickAsync();
        await _fixture.Pages[browser].GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("adho@itu.dk");
        await _fixture.Pages[browser].GetByRole(AriaRole.Textbox, new() { Name = "Password" }).ClickAsync();
        await _fixture.Pages[browser].GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("M32Want_Access");
        await _fixture.Pages[browser].GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();

        //is able to post cheep
        Assert.True(await _fixture.Pages[browser].Locator("#Message").IsVisibleAsync());

        //Shares cheep
        await _fixture.Pages[browser].Locator("#Message").ClickAsync();
        await _fixture.Pages[browser].Locator("#Message").FillAsync("PostingCheep");
        await _fixture.Pages[browser].GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();

        //Checks that cheep is shared
        Assert.Contains("PostingCheep", await _fixture.Pages[browser].GetByText("Adrian PostingCheep").First.InnerTextAsync());

        //logs out
        await _fixture.Pages[browser].GetByRole(AriaRole.Button, new() { Name = "logout [Adrian]" }).ClickAsync();

        //can no longer post cheep 
        Assert.False(await _fixture.Pages[browser].Locator("#Message").IsVisibleAsync());
    }
    [Theory]
    [MemberData(nameof(BrowserTypes.Number), MemberType = typeof(BrowserTypes))]
    public async Task PageButtonsAndEdit(int browser)
    {
        //resets to public timeline
        await _fixture.Pages[browser].GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();

        //Clicks next button
        await _fixture.Pages[browser].GetByRole(AriaRole.Link, new() { Name = "Next" }).ClickAsync();

        //Edit field agrees and we are on next page.
        Assert.Equal("2", await _fixture.Pages[browser].GetByRole(AriaRole.Spinbutton).InputValueAsync());
        Assert.True(await _fixture.Pages[browser].GetByText("Luanna Muro But now, tell me").IsVisibleAsync());

        //Click previous button
        await _fixture.Pages[browser].GetByRole(AriaRole.Link, new() { Name = "Previous" }).ClickAsync();
        //Page one agian
        Assert.Equal("1", await _fixture.Pages[browser].GetByRole(AriaRole.Spinbutton).InputValueAsync());

        //Use field edit to go to page 3
        await _fixture.Pages[browser].GetByRole(AriaRole.Spinbutton).ClickAsync();
        await _fixture.Pages[browser].GetByRole(AriaRole.Spinbutton).FillAsync("3");
        await _fixture.Pages[browser].GetByRole(AriaRole.Spinbutton).PressAsync("Enter");

        //Field edit says 3 and you can click on Previous button 2 times. (page 1)
        Assert.Equal("3", await _fixture.Pages[browser].GetByRole(AriaRole.Spinbutton).InputValueAsync());
        await _fixture.Pages[browser].GetByRole(AriaRole.Link, new() { Name = "Previous" }).ClickAsync();
        await _fixture.Pages[browser].GetByRole(AriaRole.Link, new() { Name = "Previous" }).ClickAsync();
    }
}
public static class BrowserTypes
{
    public static IEnumerable<object[]> Number()
    {
        yield return new object[] { 0 };
        yield return new object[] { 1 };
        yield return new object[] { 2 };
    }
}