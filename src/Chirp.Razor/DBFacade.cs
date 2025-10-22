using System.Reflection;

using Microsoft.Data.Sqlite;

namespace Chirp.Razor;

public class DBFacade
{
    private readonly static string sqlDBFilePath = Path.Combine(Path.GetTempPath(), "chirp.db");
    private readonly static int defaultLimit = 32;
    public readonly ICheepRepository _repository;

    public DBFacade(ICheepRepository repository)
    {
        _repository = repository;
    }

    public static SqliteConnection GetConnection()
    {
        var path = Environment.GetEnvironmentVariable("CHIRPDBPATH") ?? sqlDBFilePath;
        var connectionString = $"Data Source={path}";

        return new SqliteConnection($"Data Source={path}");
    }

    private static void CreateDb()
    {
        ExecuteSqlFromEmbeddedResource("Chirp.Razor.data.schema.sql");
        ExecuteSqlFromEmbeddedResource("Chirp.Razor.data.dump.sql");
    }

    private static void ExecuteSqlFromEmbeddedResource(string path)
    {
        // get the commands
        var assembly = Assembly.GetEntryAssembly()!;
        var stream = assembly.GetManifestResourceStream(path)!;
        var reader = new StreamReader(stream);

        var cmds = reader.ReadToEnd();


        using (var connection = GetConnection())
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = cmds;

            command.ExecuteNonQuery();
        }
    }

    public List<CheepDTO> ReadMessages()
    {
        return ReadMessages(null, null, defaultLimit);
    }

    public List<CheepDTO> ReadMessages(int? pages)
    {
        return ReadMessages(null, pages, defaultLimit);
    }
    public List<CheepDTO> ReadMessages(int? pages, int? limit)
    {
        return ReadMessages(null, pages, limit);
    }

    public List<CheepDTO> ReadMessages(string? author)
    {
        return ReadMessages(author, null, defaultLimit);
    }

    public List<CheepDTO> ReadMessages(string? author, int? pages)
    {
        return ReadMessages(author, pages, defaultLimit);
    }

    public List<CheepDTO> ReadMessages(string? author, int? pages, int? limit)
    {
        return _repository.ReadMessages(author, pages, limit).GetAwaiter().GetResult();
    }
}
//public record Cheep(string author, string message, long timestamp);