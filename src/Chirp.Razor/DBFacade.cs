using System.Reflection;

using Microsoft.Data.Sqlite;

namespace Chirp.Razor;

public class DBFacade
{
    private readonly static string sqlDBFilePath = Path.Combine(Path.GetTempPath(), "chirp.db");
    private readonly static int defaultLimit = 32;

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

    public static List<Cheep> ReadMessages()
    {
        return ReadMessages(null, null, defaultLimit);
    }

    public static List<Cheep> ReadMessages(int? pages)
    {
        return ReadMessages(null, pages, defaultLimit);
    }
    public static List<Cheep> ReadMessages(int? pages, int? limit)
    {
        return ReadMessages(null, pages, limit);
    }

    public static List<Cheep> ReadMessages(string? author)
    {
        return ReadMessages(author, null, defaultLimit);
    }

    public static List<Cheep> ReadMessages(string? author, int? pages)
    {
        return ReadMessages(author, pages, defaultLimit);
    }

    public static List<Cheep> ReadMessages(string? author, int? pages, int? limit)
    {
        var path = Environment.GetEnvironmentVariable("CHIRPDBPATH") ?? sqlDBFilePath;
        if (!File.Exists(path)) CreateDb();

        string sqlQuery = $@"
        SELECT us.user_id, us.username, me.text, me.pub_date 
        FROM message me 
        JOIN user us ON me.author_id = us.user_id 
        {(author != null ? $"WHERE us.username = '{author}'" : "")}
        ORDER by me.pub_date desc
        {(limit != null ? $"LIMIT {limit}" : "")} 
        {(pages != null && limit != null ? $"OFFSET {(pages - 1) * limit}" : "")}";


        using (var connection = GetConnection())
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            var result = new List<Cheep>();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                // See https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader.getvalues?view=dotnet-plat-ext-8.0
                // for documentation on how to retrieve complete columns from query results
                var cheep = new Cheep()
                {
                    AuthorId = reader.GetInt32(0),
                    Author = new Author() { Name = reader.GetString(1) },
                    Text = reader.GetString(2),
                    TimeStamp = Convert.ToDateTime(CheepService.UnixTimeStampToDateTimeString(reader.GetInt64(3)))
                };
                result.Add(cheep);
            }
            return result;
        }
    }
}
//public record Cheep(string author, string message, long timestamp);