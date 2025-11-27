using System.Diagnostics;
using System.Text.RegularExpressions;

using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;

using Xunit.Abstractions;

[Collection("Sequential")]
public class End2EndTests : IClassFixture<RazorPageFixture>
{
    readonly RazorPageFixture _fixture;
    enum Browser
    {
        Chromium,
        Firefox,
        Webkit
    }
    public End2EndTests(RazorPageFixture fixture)
    {
        _fixture = fixture;
    }

    //for debugging test
    /*
    private readonly ITestOutputHelper _output;

    public End2EndTests(ITestOutputHelper output)
    {
        _output = output;
    }
    */

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
    [InlineData((int)Browser.Chromium)]
    [InlineData((int)Browser.Firefox)]
    [InlineData((int)Browser.Webkit)]
    public async Task loginLogoutChirp(int browser)
    {
        var page = _fixture.Pages[browser];
        
        // Navigate to home page to ensure clean state
        await page.GotoAsync("http://localhost:5273/");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        //tries to login with incorrect password.
        await page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
        await page.WaitForURLAsync("**/Identity/Account/Login");
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).ClickAsync();
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("adho@itu.dk");
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).ClickAsync();
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("LetM31n!");
        await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await page.WaitForSelectorAsync("text=Invalid login attempt.", new() { Timeout = 5000 });
        Assert.Equal("Invalid login attempt.", await page.GetByText("Invalid login attempt.").InnerTextAsync());

        //Correct password
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).ClickAsync();
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("M32Want_Access");
        await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await page.WaitForURLAsync("**/");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        //My timeline is Adrian's Timeline
        await page.WaitForSelectorAsync("text=Adrian's Timeline", new() { Timeout = 5000 });
        Assert.Equal("Adrian's Timeline", await page.GetByRole(AriaRole.Heading, new() { Name = "Adrian's Timeline" }).InnerTextAsync());

        //logs out
        await page.GetByRole(AriaRole.Button, new() { Name = "logout [Adrian]" }).ClickAsync();
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    [Theory]
    [InlineData((int)Browser.Chromium)]
    [InlineData((int)Browser.Firefox)]
    [InlineData((int)Browser.Webkit)]
    public async Task PostCheep(int browser)
    {
        var page = _fixture.Pages[browser];
        
        // Navigate to home page to ensure clean state
        await page.GotoAsync("http://localhost:5273/");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        //logs in
        await page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
        await page.WaitForURLAsync("**/Identity/Account/Login");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("adho@itu.dk");
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("M32Want_Access");
        await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await page.WaitForURLAsync("**/");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        //is able to post cheep
        await page.WaitForSelectorAsync("#Message", new() { Timeout = 5000 });
        Assert.True(await page.Locator("#Message").IsVisibleAsync());

        //Shares cheep
        await page.Locator("#Message").ClickAsync();
        await page.Locator("#Message").FillAsync("PostingCheep");
        await page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        //Checks that cheep is shared
        await page.WaitForSelectorAsync("text=PostingCheep", new() { Timeout = 5000 });
        Assert.Contains("PostingCheep", await page.GetByText("Adrian PostingCheep").First.InnerTextAsync());

        //logs out
        await page.GetByRole(AriaRole.Button, new() { Name = "logout [Adrian]" }).ClickAsync();
        await page.WaitForURLAsync("**/Identity/Account/Logout");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        //back to public timeline
        await page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();
        await page.WaitForURLAsync("**/");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        //can no longer post cheep 
        Assert.False(await page.Locator("#Message").IsVisibleAsync());
    }
    
    [Theory]
    [InlineData((int)Browser.Chromium)]
    [InlineData((int)Browser.Firefox)]
    [InlineData((int)Browser.Webkit)]
    public async Task PageButtonsAndEdit(int browser)
    {
        var page = _fixture.Pages[browser];
        
        // Navigate to home page to ensure clean state
        await page.GotoAsync("http://localhost:5273/");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        //resets to public timeline
        await page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        //Clicks next button
        await page.GetByRole(AriaRole.Link, new() { Name = "Next" }).ClickAsync();
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        //Edit field agrees and we are on next page.
        await page.WaitForSelectorAsync("text=Luanna Muro But now, tell me", new() { Timeout = 5000 });
        Assert.Equal("2", await page.GetByRole(AriaRole.Spinbutton).InputValueAsync());
        Assert.True(await page.GetByText("Luanna Muro But now, tell me").IsVisibleAsync());

        //Click previous button
        await page.GetByRole(AriaRole.Link, new() { Name = "Previous" }).ClickAsync();
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        //Page one agian
        Assert.Equal("1", await page.GetByRole(AriaRole.Spinbutton).InputValueAsync());

        //Use field edit to go to page 3
        await page.GetByRole(AriaRole.Spinbutton).ClickAsync();
        await page.GetByRole(AriaRole.Spinbutton).FillAsync("3");
        await page.GetByRole(AriaRole.Spinbutton).PressAsync("Enter");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        //Field edit says 3 and you can click on Previous button 2 times. (page 1)
        Assert.Equal("3", await page.GetByRole(AriaRole.Spinbutton).InputValueAsync());
        await page.GetByRole(AriaRole.Link, new() { Name = "Previous" }).ClickAsync();
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await page.GetByRole(AriaRole.Link, new() { Name = "Previous" }).ClickAsync();
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    [Theory]
    [InlineData((int)Browser.Chromium)]
    [InlineData((int)Browser.Firefox)]
    [InlineData((int)Browser.Webkit)]
    public async Task Security_XSS_UrlRedirectsInName(int browser)
    {
        // Random page number without browsers overlapping
        int pageNo = (browser * 100) + new Random().Next(99);

        var page = _fixture.Pages[browser];
        
        // Navigate to home page to ensure clean state
        await page.GotoAsync("http://localhost:5273/");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Register account
        await page.GetByRole(AriaRole.Link, new() { Name = "register" }).ClickAsync();
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Name" }).FillAsync($"?page={pageNo}");
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync($"page{pageNo}@test.xss");
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Password", Exact = true }).FillAsync("Test1!");
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Confirm Password" }).FillAsync("Test1!");
        await page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
        await page.GetByRole(AriaRole.Link, new() { Name = "Click here to confirm your" }).ClickAsync();

        // Login to account
        await page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync($"page{pageNo}@test.xss");
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("Test1!");
        await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();

        // Assert links are properly encoded
        // On 'my timeline' link
        var myTimeline = page.GetByRole(AriaRole.Link, new() { Name = "my timeline" });
        await Assertions.Expect(myTimeline).ToHaveAttributeAsync("href", $"/%3Fpage%3D{pageNo}");
        await myTimeline.ClickAsync();

        // On new cheep
        await page.Locator("#Message").FillAsync("Testing name with URL redirect.");
        await page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();
        await Assertions.Expect(page).ToHaveURLAsync(new Regex($".*/%3Fpage%3D{pageNo}"));

        // On cheep author link
        var cheepAuthor = page.GetByRole(AriaRole.Link, new() { Name = $"?page={pageNo}", Exact = true });
        await Assertions.Expect(cheepAuthor).ToHaveAttributeAsync("href", $"/%3Fpage%3D{pageNo}");

        // Logout
        await page.GetByRole(AriaRole.Button, new() { Name = "logout [" }).ClickAsync();
        await page.WaitForURLAsync("**/Identity/Account/Logout");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    [Theory]
    [InlineData((int)Browser.Chromium)]
    [InlineData((int)Browser.Firefox)]
    [InlineData((int)Browser.Webkit)]
    public async Task Security_XSS_ScriptTagsInNameOrCheep(int browser)
    {
        // Random page number without browsers overlapping
        int randomNo = (browser * 100) + new Random().Next(99);

        string message = $"The XSS attack worked...";
        string maliciousScript = $"<script>document.title = '{message}'</script>{randomNo}";

        var page = _fixture.Pages[browser];
        
        // Navigate to home page to ensure clean state
        await page.GotoAsync("http://localhost:5273/");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        // Register account
        await page.GetByRole(AriaRole.Link, new() { Name = "register" }).ClickAsync();
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Name" }).FillAsync(maliciousScript);
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync($"script{randomNo}@test.xss");
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Password", Exact = true }).FillAsync("Test1!");
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Confirm Password" }).FillAsync("Test1!");
        await page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();
        await page.GetByRole(AriaRole.Link, new() { Name = "Click here to confirm your" }).ClickAsync();

        // Login to account
        await page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync($"script{randomNo}@test.xss");
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("Test1!");
        await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();

        // Assert no XSS injection happened
        // After login
        await Assertions.Expect(page).Not.ToHaveTitleAsync(message);

        // On new cheep
        await page.Locator("#Message").FillAsync($"Testing name and cheep with XSS script. {maliciousScript}");
        await page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();
        await Assertions.Expect(page).Not.ToHaveTitleAsync(message);

        // Logout
        await page.GetByRole(AriaRole.Button, new() { Name = "logout [" }).ClickAsync();
        await page.WaitForURLAsync("**/Identity/Account/Logout");
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }
}