using Chirp.Razor;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Use environment variable or fallback path for SQLite DB
var sqlDBFilePath = Path.Combine(Path.GetTempPath(), "chirp.db"); // Default fallback path
var path = Environment.GetEnvironmentVariable("CHIRPDBPATH") ?? sqlDBFilePath;
var connectionString = $"Data Source={path}";
builder.Services.AddDbContext<ChirpDBContext>(options => options.UseSqlite(connectionString, b => b.MigrationsAssembly("Chirp.Web")));

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<ICheepRepository, CheepRepository>();
builder.Services.AddScoped<ICheepService, CheepService>();


var app = builder.Build();

// Seed the database with example data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ChirpDBContext>();
    db.Database.Migrate();
    DbInitializer.SeedDatabase(db);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();
