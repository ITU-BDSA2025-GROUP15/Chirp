// See https://aka.ms/new-console-template for more information

using System.ComponentModel;
using System.Globalization;
using System.Reflection.Metadata;

using CsvHelper;

var filename = "./chirp_cli_db.csv";

if (args.Length == 0)
{
    System.Console.WriteLine("No args given!");
    return;
}

switch (args[0])
{
    case "cheep":
        cheep(args[1]);
        break;
    case "read":
        read();
        break;
    default:
        break;
}

// Adds cheep to database file.
void cheep(string cheep)
{
    var username = Environment.UserName;
    var timeOfCheep = new DateTimeOffset(DateTime.UtcNow, TimeSpan.Zero);

    using (var file = new StreamWriter(filename, true))
    {
        file.WriteLine(username + ",\"" + cheep + "\"," + timeOfCheep.ToUnixTimeSeconds());
    }
}

// Reads all posts from database file. 
void read()
{
    var reader = new StreamReader(filename);
    var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

    var records = csv.GetRecords<Cheep>();

    foreach (var cheep in records)
    {
        var timestamp = DateTimeOffset.FromUnixTimeSeconds(cheep.Timestamp);
        timestamp = TimeZoneInfo.ConvertTime(timestamp, TimeZoneInfo.Local);

        Console.WriteLine(cheep.Author + " @ " + timestamp.ToString("MM/dd/yy HH:mm:ss") + ": " + cheep.Message);   
    }
}