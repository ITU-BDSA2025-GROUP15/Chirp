using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Chirp.Infrastructure;

public class TestServerFixture : IAsyncLifetime
{
    private SqliteConnection _connection;
    private WebApplicationFactory<Program> _factory;

    public HttpClient Client { get; private set; }
    public ChirpDBContext DbContext { get; private set; }

    public async Task InitializeAsync()
    {
        // 1️⃣ Create an in-memory SQLite connection
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        // 2️⃣ Configure WebApplicationFactory to override DbContext
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove existing DbContext registration
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ChirpDBContext>));
                    if (descriptor != null)
                        services.Remove(descriptor);

                    // Register in-memory SQLite DbContext
                    services.AddDbContext<ChirpDBContext>(options =>
                    {
                        options.UseSqlite(_connection);
                    });

                    // Build the service provider to initialize DB
                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<ChirpDBContext>();
                    db.Database.EnsureCreated(); // Creates tables in-memory
                });
            });

        // 3️⃣ Create HttpClient for Playwright tests
        Client = _factory.CreateClient();

        // 4️⃣ Create a DbContext instance for direct access in tests
        var scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
        var scope2 = scopeFactory.CreateScope();
        DbContext = scope2.ServiceProvider.GetRequiredService<ChirpDBContext>();
    }

    public async Task DisposeAsync()
    {
        // Dispose resources safely
        Client?.Dispose();
        DbContext?.Dispose();
        _factory?.Dispose();
        _connection?.Close();
        _connection?.Dispose();

        Client = null;
        DbContext = null;
        _factory = null;
        _connection = null;
    }
}
