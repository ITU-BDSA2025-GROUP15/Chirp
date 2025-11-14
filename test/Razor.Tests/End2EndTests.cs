using System.Diagnostics;
using System.Text.RegularExpressions;

using Xunit.Abstractions;
public class RazorPageFixture : IAsyncLifetime
{
    Process _razorPage;
    public HttpClient client;
    public async Task InitializeAsync()
    {
        _razorPage = await TestUtils.StartRazorPage();
        var baseURL = "http://localhost:5273/";
        client = new();
        client.BaseAddress = new Uri(baseURL);
    }
    public Task DisposeAsync()
    {
        client.Dispose(); //I don't know if this is needed
        _razorPage.Kill(true);
        _razorPage.WaitForExit();
        _razorPage.Dispose();
        return Task.CompletedTask;
    }
}

[Collection("Sequential")]
public class End2EndTests : IClassFixture<RazorPageFixture>
{
    public readonly string Razor_path = "src/Chirp.Razor/Chirp.Razor.csproj";

    RazorPageFixture _fixture;

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
        var HTTPResponsePageDefault = await _fixture.client.GetAsync("/");
        string responseBodyPageDefault = await HTTPResponsePageDefault.Content.ReadAsStringAsync();

        // Page 1
        var HTTPResponsePage1 = await _fixture.client.GetAsync("/?page=1");
        string responseBodyPage1 = await HTTPResponsePage1.Content.ReadAsStringAsync();

        // Assert
        // Page 1 and default page is the same
        Assert.Equal(responseBodyPageDefault, responseBodyPage1);
    }

    [Fact]
    public async Task CheepThatShouldBeOnPage1()
    {
        //Act
        var HTTPResponsePageDefault = await _fixture.client.GetAsync("/");
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
        var HTTPResponsePage2 = await _fixture.client.GetAsync("/?page=2");
        string responseBodyPage2 = await HTTPResponsePage2.Content.ReadAsStringAsync();

        Assert.Contains("It is asking much of it in the world.", responseBodyPage2);
        Assert.Contains("Jacqualine Gilcoine", responseBodyPage2);
    }
    
    [Fact]
    public async Task Page1and2AreNotTheSame()
    {
        //Act
        var HTTPResponsePageDefault = await _fixture.client.GetAsync("/");
        string responseBodyPageDefault = await HTTPResponsePageDefault.Content.ReadAsStringAsync();
        //Page 2
        var HTTPResponsePage2 = await _fixture.client.GetAsync("/?page=2");
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
        var HTTPResponseUser = await _fixture.client.GetAsync("/Adrian");
        string responseBodyUser = await HTTPResponseUser.Content.ReadAsStringAsync();

        responseBodyUser = responseBodyUser.Replace("\r\n", "\n"); // In case of Windows users
        responseBodyUser = Regex.Replace(responseBodyUser, "\n\\s*", ""); // Strip leading whitespace

        // Only Adrians posts should be on the user page
        // _output.WriteLine(responseBodyUser);
        Assert.Contains(expectedFullStr, responseBodyUser);
        Assert.DoesNotContain("Jacqualine Gilcoine", responseBodyUser);
    }
}