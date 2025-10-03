using Microsoft.Data.Sqlite;

namespace Chirp.Razor;

public class DBFacade
{
    private readonly static string sqlDBFilePath = Path.Combine(Path.GetTempPath(), "chirp.db");

    private static SqliteConnection GetConnection()
    {
        var path = Environment.GetEnvironmentVariable("CHIRPDBPATH") ?? sqlDBFilePath;

        return new SqliteConnection($"Data Source={path}");
    }

    public static List<Cheep> ReadMessages()
    {
        return ReadMessages(null, null);
    }

    public static List<Cheep> ReadMessages(int? pages)
    {
        return ReadMessages(null, pages);
    }

    public static List<Cheep> ReadMessages(string? author)
    {
        return ReadMessages(author, null);
    }

    public static List<Cheep> ReadMessages(string? author, int? pages)
    {
        int postsPerPage = 32;

        string sqlQuery = $@"
        SELECT us.username, me.text, me.pub_date 
        FROM message me 
        JOIN user us ON me.author_id = us.user_id 
        {(author != null ? $"WHERE us.username = '{author}'" : "")}
        ORDER by me.pub_date desc
        LIMIT {postsPerPage} 
        {(pages != null ? $"OFFSET {(pages - 1) * postsPerPage}" : "")}";


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
                var cheep = new Cheep(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetInt64(2)
                );
                result.Add(cheep);
            }
            return result;
        }
    }
}

public record Cheep(string author, string message, long timestamp);