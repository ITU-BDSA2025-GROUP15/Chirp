using Microsoft.Data.Sqlite;

namespace Chirp.Razor;

public class DBFacade
{
    readonly static string sqlDBFilePath = "/tmp/chirp.db";

    private static SqliteConnection GetConnection()
    {
        var path = Environment.GetEnvironmentVariable("CHIRPDBPATH") ?? sqlDBFilePath;

        return new SqliteConnection($"Data Source={path}");
    }

    public static List<Cheep> ReadMessages()
    {
        return ReadMessages(null);
    }

    public static List<Cheep> ReadMessages(string? author)
    {
        string sqlQuery;
        if (author == null)
        {
            sqlQuery = @"
            SELECT us.username, me.text, me.pub_date 
            FROM message me 
            JOIN user us ON me.author_id = us.user_id 
            ORDER by me.pub_date desc";
        }
        else
        {
            sqlQuery = $@"
            SELECT us.username, me.text, me.pub_date 
            FROM message me 
            JOIN user us ON me.author_id = us.user_id 
            WHERE us.username = '{author}'
            ORDER by me.pub_date desc";
        }

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