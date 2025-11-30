using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Xunit;

namespace PlaywrightTests
{
    public class CheepUITests : IAsyncLifetime
    {
        private IBrowser _browser = null!;
        private IBrowserContext _context = null!;
        private IPage _page = null!;
        private Process? _server;

        private const string BaseUrl = "http://localhost:5273";
        private const string TestEmail = "testuser@example.com";
        private const string TestPassword = "Password123!";

        public async Task InitializeAsync()
        {
            // Start ASP.NET Core server
            var psi = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "run --urls http://localhost:5273",
                WorkingDirectory = @"C:\Users\Frede\Github\Chirp\src\Chirp.Web",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };

            _server = Process.Start(psi);

            // Wait for server to respond
            var client = new HttpClient();
            bool serverUp = false;
            for (int i = 0; i < 30; i++)
            {
                try
                {
                    var response = await client.GetAsync(BaseUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        serverUp = true;
                        break;
                    }
                }
                catch { }
                await Task.Delay(1000);
            }

            if (!serverUp)
                throw new Exception("Server did not start in time.");

            // Setup Playwright
            var playwright = await Playwright.CreateAsync();
            _browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false // change to true in CI
            });
            _context = await _browser.NewContextAsync();
            _page = await _context.NewPageAsync();
        }

        public async Task DisposeAsync()
        {
            if (_context != null) await _context.CloseAsync();
            if (_browser != null) await _browser.CloseAsync();
            if (_server != null && !_server.HasExited)
            {
                _server.Kill();
                _server.WaitForExit();
            }
        }

        // --- Helpers ---

        private async Task LoginAsync()
        {
            await _page.GotoAsync($"{BaseUrl}/Identity/Account/Login");

            // Wait until login form is loaded
            await _page.WaitForSelectorAsync("form", new PageWaitForSelectorOptions { Timeout = 10000 });

            // Fill login form
            await _page.FillAsync("input[name='Input.Email']", TestEmail);
            await _page.FillAsync("input[name='Input.Password']", TestPassword);
            await _page.ClickAsync("button[type=submit]");

            // Wait for cheep input to appear after login
            try
            {
                await _page.WaitForSelectorAsync("#cheepInput", new PageWaitForSelectorOptions { Timeout = 10000 });
            }
            catch (TimeoutException)
            {
                // Take screenshot to debug
                await _page.ScreenshotAsync(new PageScreenshotOptions { Path = "login_fail.png" });
                throw new Exception("Login failed or #cheepInput not found. Screenshot saved as login_fail.png");
            }
        }

        private async Task PostCheepAsync(string message)
        {
            await _page.FillAsync("#cheepInput", message);
            await _page.ClickAsync("#cheepSubmit");
        }

        // --- Tests ---

        [Fact]
        public async Task PostInputVisibleOnlyAfterLogin()
        {
            await _page.GotoAsync(BaseUrl);

            Assert.False(await _page.IsVisibleAsync("#cheepInput"));

            await LoginAsync();

            Assert.True(await _page.IsVisibleAsync("#cheepInput"));
        }

        [Fact]
        public async Task CheepsDisplayedAfterPosting()
        {
            await LoginAsync();

            string cheepText = "Hello Playwright " + DateTime.UtcNow.Ticks;
            await PostCheepAsync(cheepText);

            var locator = _page.Locator($"#messagelist >> text={cheepText}");
            await locator.WaitForAsync(new LocatorWaitForOptions { Timeout = 5000 });
            Assert.True(await locator.IsVisibleAsync());
        }

        [Fact]
        public async Task CannotPostCheepLongerThan160Chars()
        {
            await LoginAsync();

            string longCheep = new string('x', 161);
            await PostCheepAsync(longCheep);

            Assert.True(await _page.IsVisibleAsync("text=Maximum length is 160"));

            var locator = _page.Locator($"#messagelist >> text={longCheep}");
            Assert.Equal(0, await locator.CountAsync());
        }
    }
}
