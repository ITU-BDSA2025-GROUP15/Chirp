using System.Diagnostics;
using System.Text.RegularExpressions;

using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;

using Xunit.Abstractions;

public class RazorPageFixture : IAsyncLifetime
{
    Process? _razorPage;
    public required HttpClient Client;
    public readonly string BaseUrl = "http://localhost:5273/";
    IPlaywright? _playwright;
    public required IPage[] Pages = new IPage[3];
    public async Task InitializeAsync()
    {
        _razorPage = await TestUtils.StartRazorPage();
        var baseURL = "http://localhost:5273/";
        Client = new();
        Client.BaseAddress = new Uri(baseURL);

        _playwright = await Playwright.CreateAsync();

        //Name of the three browsers types we test
        string[] testBrowsers = {"Chromium", "Firefox", "Webkit"};

        //Creates each browser to test on. (Mathces with enum)
        for (int i = 0; i < testBrowsers.Length; i++)
        {
            IBrowser _browser = await _playwright[testBrowsers[i]].LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false //Set this to false to see what the test are doing.(Will open browser windows)
            });
            IBrowserContext context = await _browser.NewContextAsync();
            Pages[i] = await context.NewPageAsync();
            await Pages[i].GotoAsync("http://localhost:5273/"); //Open our page on the browser.
        }
    }
    public async Task RestartRazorPage()
    {
        _razorPage!.Kill(true);
        _razorPage.WaitForExit();
        _razorPage.Dispose();
        _razorPage = await TestUtils.StartRazorPage();
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