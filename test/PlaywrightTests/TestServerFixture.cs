using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;              
using Microsoft.Extensions.DependencyInjection;  
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;            
using Microsoft.Playwright;
using System.Linq;                              
using System.Threading.Tasks;
using Xunit;


public class TestServerFixture : IAsyncLifetime
{
    public WebApplicationFactory<Program> Factory { get; private set; } = null!;
    public IPlaywright Playwright { get; private set; } = null!;
    public IBrowser Browser { get; private set; } = null!;
    public string BaseUrl { get; private set; } = null!;

    private SqliteConnection _connection = null!;

    public async Task InitializeAsync()
    {
        // Shared in-memory SQLite for EF Core
        _connection = new SqliteConnection("DataSource=:memory:;Cache=Shared");
        _connection.Open();

        Factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");

                builder.ConfigureServices(services =>
                {
                    // Replace ChirpDBContext
                    var descriptor = services.Single(
                        s => s.ServiceType == typeof(DbContextOptions<ChirpDBContext>));
                    services.Remove(descriptor);

                    services.AddDbContext<ChirpDBContext>(options =>
                        options.UseSqlite(_connection));

                    // Build scope + seed user
                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<ChirpDBContext>();

                    db.Database.EnsureCreated();

                    // SEED TEST USER (your Author class in global namespace)
                    if (!db.Authors.Any(a => a.Name == "test"))
                    {
                        var testUser = new Author
                        {
                            Name = "test",
                            UserName = "test",
                            Email = "test@example.com",
                            NormalizedUserName = "TEST",
                            NormalizedEmail = "TEST@EXAMPLE.COM",
                        };

                        db.Authors.Add(testUser);
                        db.SaveChanges();
                    }
                });
            });

        var client = Factory.CreateClient();
        BaseUrl = client.BaseAddress!.ToString().TrimEnd('/');

        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
    }

    public async Task DisposeAsync()
    {
        await Browser.CloseAsync();
        Playwright.Dispose();
        _connection?.Close();
        Factory.Dispose();
    }
}
