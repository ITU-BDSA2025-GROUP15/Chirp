namespace Chirp.CSVDBService;

class CSVDBService {
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.MapGet("/cheeps", GetCheeps);
        app.MapPost("/cheep", StoreCheep);

        app.Run();
    }

    static IEnumerable<Cheep> GetCheeps(int? n) {
        return CSVDatabase<Cheep>.Instance.Read(n);
    }

    static void StoreCheep(Cheep cheep) {
        var db = CSVDatabase<Cheep>.Instance;
        db.Store(cheep);
    }
}