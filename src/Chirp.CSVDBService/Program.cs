namespace Chirp.CSVDBService;

class CSVDBService {
    readonly static string baseURL = "http://localhost:5086";
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.MapGet("/cheeps", GetCheeps);
        app.MapPost("/cheep", StoreCheep);

        app.Run(baseURL);
    }

    static IEnumerable<Cheep> GetCheeps(int? n) {
        return CSVDatabase<Cheep>.Instance.Read(n);
    }

    static void StoreCheep(Cheep cheep) {
        var db = CSVDatabase<Cheep>.Instance;
        db.Store(cheep);
    }
}