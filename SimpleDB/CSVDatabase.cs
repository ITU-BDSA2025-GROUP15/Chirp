using SimpleDB;
using CsvHelper;
using System.Globalization;
sealed class CSVDatabase<T> : IDatabaseRepository<T>
{
    readonly string filename = "./chirp_cli_db.csv";
    
    // Adds cheep to database file.
    public void Store(T record)
    {
        var username = Environment.UserName;
        var timeOfCheep = new DateTimeOffset(DateTime.UtcNow, TimeSpan.Zero);
        using (var file = new StreamWriter(filename, true))
        using (var csv = new CsvWriter(file, CultureInfo.InvariantCulture))
        {
            csv.WriteRecord(record);
            csv.NextRecord();
        }
    }


    // Reads all posts from database file. 
    public IEnumerable<T> Read(int? limit)
    {
        var reader = new StreamReader(filename);
        var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        
        var records = csv.GetRecords<T>();
        if (limit == null) return records;

        List<T> records_new = new List<T>();

        int i = 0;
        foreach (var record in records) {
            if (i++ >= limit) break;
        
            records_new.Add(record);
        }

        return records_new;
    }
}