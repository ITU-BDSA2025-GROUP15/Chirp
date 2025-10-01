using Microsoft.Data.Sqlite;

namespace Chirp.Razor;

public class DBFacade {
    readonly string sqlDBFilePath = "/tmp/chirp.db";

    public DBFacade() {
        
    }

    private SqliteConnection GetConnection() {
        return new SqliteConnection($"Data Source={sqlDBFilePath}");
    }

    public IEnumerable<CheepViewModel> ReadMessages() {
        var sqlQuery = @"SELECT * FROM message ORDER by message.pub_date desc";

        using (var connection = GetConnection())
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            var result = new List<CheepViewModel>();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {    
                var l = reader.GetValue(0);

                // See https://learn.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqldatareader.getvalues?view=dotnet-plat-ext-8.0
                // for documentation on how to retrieve complete columns from query results
                Object[] values = new Object[reader.FieldCount];
                int fieldCount = reader.GetValues(values);
                for (int i = 0; i < fieldCount; i++)
                    Console.WriteLine($"{reader.GetName(i)}: {values[i]}");
                var cheep = new CheepViewModel(
                    values[0].ToString(), 
                    values[1].ToString(), 
                    values[2].ToString()
                );
                result.Add(cheep);
            }
            return result;
        }
    }
}