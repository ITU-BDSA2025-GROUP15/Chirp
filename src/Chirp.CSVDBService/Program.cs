var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/cheeps", () => {  });
app.MapPost("/cheep", (Cheep cheep) => {  });

app.Run();
