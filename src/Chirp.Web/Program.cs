using Chirp.Razor;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Use environment variable or fallback path for SQLite DB
var sqlDBFilePath = Path.Combine(Path.GetTempPath(), "chirp.db"); // Default fallback path
var path = Environment.GetEnvironmentVariable("CHIRPDBPATH") ?? sqlDBFilePath;
var connectionString = $"Data Source={path}";
builder.Services.AddDbContext<ChirpDBContext>(options => options.UseSqlite(connectionString, b => b.MigrationsAssembly("Chirp.Web")));

builder.Services.AddDefaultIdentity<Author>(options => options.SignIn.RequireConfirmedAccount = true) 
                .AddEntityFrameworkStores<ChirpDBContext>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddScoped<ICheepRepository, CheepRepository>();
builder.Services.AddScoped<ICheepService, CheepService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();

string? ghClientId = builder.Configuration["authentication:github:clientId"];
string? ghClientSecret = builder.Configuration["authentication:github:clientSecret"];
string? googleClientId = builder.Configuration["authentication:google:clientId"];
string? googleClientSecret = builder.Configuration["authentication:google:clientSecret"];
bool ghConfigured = false;
bool googleConfigured = false;

if (ghClientId != null && ghClientSecret != null || googleClientId != null && googleClientSecret != null)
{
    var authBuilder = builder.Services.AddAuthentication(options =>
    {
        if (ghClientId != null && ghClientSecret != null)
        {
            options.DefaultChallengeScheme = "GitHub";
        }
        else if (googleClientId != null && googleClientSecret != null)
        {
            options.DefaultChallengeScheme = "Google";
        }
    })
    .AddCookie();

    if (ghClientId != null && ghClientSecret != null)
    {
        authBuilder.AddGitHub(o =>
        {
            o.ClientId = ghClientId;
            o.ClientSecret = ghClientSecret;
            o.CallbackPath = "/signin-github";
            o.Scope.Add("user:email");
        });
        ghConfigured = true;
    }

    if (googleClientId != null && googleClientSecret != null)
    {
        authBuilder.AddGoogle(o =>
        {
            o.ClientId = googleClientId;
            o.ClientSecret = googleClientSecret;
            o.CallbackPath = "/signin-google";
        });
        googleConfigured = true;
    }
}

var app = builder.Build();

if (!ghConfigured && !googleConfigured) app.Logger.LogWarning("Authentication providers not configured. GitHub or Google client id or secret missing!");

// Seed the database with example data and initial accounts
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ChirpDBContext>();
    db.Database.Migrate();
    DbInitializer.SeedDatabase(db);

    var userManager = scope.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<Author>>();
    var userStore = scope.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.IUserStore<Author>>();
    var emailStore = (IUserEmailStore<Author>)userStore;
    new AccountsInitializer(userManager, userStore, emailStore, db).SeedAccounts();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization(); 

app.MapRazorPages();

app.Run();
