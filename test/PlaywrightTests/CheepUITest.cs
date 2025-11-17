using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Xunit;

namespace PlaywrightTests
{
    public class CheepUITests : IAsyncLifetime
    {
        private IBrowser _browser;
        private IBrowserContext _context;
        private IPage _page;

        // Setup / Teardown
        public async Task InitializeAsync()
        {
            var playwright = await Playwright.CreateAsync();
            _browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true // Set false to see browser actions
            });
            _context = await _browser.NewContextAsync();
            _page = await _context.NewPageAsync();
        }

        public async Task DisposeAsync()
        {
            await _context.CloseAsync();
            await _browser.CloseAsync();
        }

        // Helpers
        private async Task LoginAsync(string username, string password)
        {
            await _page.GotoAsync("http://localhost:5000/login");
            await _page.FillAsync("#username", username);
            await _page.FillAsync("#password", password);
            await _page.ClickAsync("#loginBtn");

            // Wait for cheep input to appear after login
            await _page.WaitForSelectorAsync("#cheepInput");
        }

        private async Task PostCheepAsync(string content)
        {
            await _page.FillAsync("#cheepInput", content);
            await _page.ClickAsync("#postCheepBtn");
        }

        // Tests

        [Fact]
        public async Task PostInputVisibleOnlyAfterLogin()
        {
            await _page.GotoAsync("http://localhost:5000/");

            // Input should NOT be visible before login
            Assert.False(await _page.IsVisibleAsync("#cheepInput"));

            // Log in
            await LoginAsync("testuser", "password123");

            // Input should now be visible
            Assert.True(await _page.IsVisibleAsync("#cheepInput"));
        }

        [Fact]
        public async Task CheepsDisplayedAfterPosting()
        {
            await LoginAsync("testuser", "password123");

            string cheepText = "Hello, this is a test cheep! " + DateTime.UtcNow.Ticks;
            await PostCheepAsync(cheepText);

            // Wait for the new cheep to appear in the list
            await _page.Locator("#cheepList >> text=" + cheepText).WaitForAsync();

            Assert.True(await _page.Locator("#cheepList >> text=" + cheepText).IsVisibleAsync());
        }

        [Fact]
        public async Task CannotPostCheepLongerThan160Chars()
        {
            await LoginAsync("testuser", "password123");

            string longCheep = new string('x', 161);
            await PostCheepAsync(longCheep);

            // Check for error message
            Assert.True(await _page.IsVisibleAsync("#cheepError"));
            var errorText = await _page.TextContentAsync("#cheepError");
            Assert.Contains("160 characters", errorText);

            // Ensure it does NOT appear in cheep list
            var count = await _page.Locator("#cheepList >> text=" + longCheep).CountAsync();
            Assert.Equal(0, count);
        }
    }
}
