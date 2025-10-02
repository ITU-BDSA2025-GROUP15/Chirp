using Microsoft.Data.Sqlite;

namespace Chirp.Razor;

public class DBFacade {
    readonly static string sqlDBFilePath = "/tmp/chirp.db";

    private static SqliteConnection GetConnection() {
        var path = Environment.GetEnvironmentVariable("CHIRPDBPATH") ?? sqlDBFilePath;
        
        return new SqliteConnection($"Data Source={path}");
    }

    public static List<CheepViewModel> ReadMessages() {
        var sqlQuery = @"
        SELECT us.username, me.text, me.pub_date 
        FROM message me 
        JOIN user us ON me.author_id = us.user_id 
        ORDER by me.pub_date desc";

        using (var connection = GetConnection())
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            var result = new List<CheepViewModel>();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {    
                // See https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader.getvalues?view=dotnet-plat-ext-8.0
                // for documentation on how to retrieve complete columns from query results
                var cheep = new CheepViewModel(
                    reader.GetString(0), 
                    reader.GetString(1), 
                    reader.GetString(2)
                );
                result.Add(cheep);
            }
            return result;
        }
    }
}