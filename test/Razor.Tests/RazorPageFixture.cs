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