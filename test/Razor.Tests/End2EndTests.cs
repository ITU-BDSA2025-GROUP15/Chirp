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

        //tries to login with incorrect password.
        await page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).ClickAsync();
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("adho@itu.dk");
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).ClickAsync();
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("LetM31n!");
        await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();

        Assert.Equal("Invalid login attempt.", await page.GetByText("Invalid login attempt.").InnerTextAsync());

        //Correct password
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).ClickAsync();
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("M32Want_Access");
        await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();

        //My timeline is Adrian's Timeline
        Assert.Equal("Adrian's Timeline", await page.GetByRole(AriaRole.Heading, new() { Name = "Adrian's Timeline" }).InnerTextAsync());

        //logs out
        await page.GetByRole(AriaRole.Button, new() { Name = "logout [Adrian]" }).ClickAsync();
    }

    [Theory]
    [InlineData((int)Browser.Chromium)]
    [InlineData((int)Browser.Firefox)]
    [InlineData((int)Browser.Webkit)]
    public async Task PostCheep(int browser)
    {
        var page = _fixture.Pages[browser];

        //logs in
        await page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
        await page.WaitForURLAsync("**/Identity/Account/Login");

        await page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("adho@itu.dk");
        await page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("M32Want_Access");
        await page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();
        await page.WaitForURLAsync("**/");

        //is able to post cheep
        Assert.True(await page.Locator("#Message").IsVisibleAsync());

        //Shares cheep
        await page.Locator("#Message").ClickAsync();
        await page.Locator("#Message").FillAsync("PostingCheep");
        await page.GetByRole(AriaRole.Button, new() { Name = "Share" }).ClickAsync();

        //Checks that cheep is shared
        Assert.Contains("PostingCheep", await page.GetByText("Adrian PostingCheep").First.InnerTextAsync());

        //logs out
        await page.GetByRole(AriaRole.Button, new() { Name = "logout [Adrian]" }).ClickAsync();
        await page.WaitForURLAsync("**/Identity/Account/Logout");

        //back to public timeline
        await page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();
        await page.WaitForURLAsync("**/");

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

        //resets to public timeline
        await page.GetByRole(AriaRole.Link, new() { Name = "public timeline" }).ClickAsync();

        //Clicks next button
        await page.GetByRole(AriaRole.Link, new() { Name = "Next" }).ClickAsync();

        //Edit field agrees and we are on next page.
        Assert.Equal("2", await page.GetByRole(AriaRole.Spinbutton).InputValueAsync());
        Assert.True(await page.GetByText("Luanna Muro But now, tell me").IsVisibleAsync());

        //Click previous button
        await page.GetByRole(AriaRole.Link, new() { Name = "Previous" }).ClickAsync();
        //Page one agian
        Assert.Equal("1", await page.GetByRole(AriaRole.Spinbutton).InputValueAsync());

        //Use field edit to go to page 3
        await page.GetByRole(AriaRole.Spinbutton).ClickAsync();
        await page.GetByRole(AriaRole.Spinbutton).FillAsync("3");
        await page.GetByRole(AriaRole.Spinbutton).PressAsync("Enter");

        //Field edit says 3 and you can click on Previous button 2 times. (page 1)
        Assert.Equal("3", await page.GetByRole(AriaRole.Spinbutton).InputValueAsync());
        await page.GetByRole(AriaRole.Link, new() { Name = "Previous" }).ClickAsync();
        await page.GetByRole(AriaRole.Link, new() { Name = "Previous" }).ClickAsync();
    }
}