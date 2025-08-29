// See https://aka.ms/new-console-template for more information

using System.ComponentModel;
using System.Reflection.Metadata;

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
    reader.ReadLine(); // to not read the header
    
    while (!reader.EndOfStream)
    {
        var line = reader.ReadLine();
        if (line == null) continue;

        var firstComma = line.IndexOf(",");
        var lastComma = line.LastIndexOf(",");

        var author = line.Substring(0, firstComma);
        var message = line.Substring(firstComma + 2, lastComma - firstComma - 3);
        var tempTimestamp = line.Substring(lastComma + 1);
        var timestamp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(tempTimestamp));

        var timezone = DateTime.Now - DateTime.UtcNow;
        timestamp += timezone;

        Console.WriteLine(author + " @ " + timestamp.ToString("MM/dd/yy HH:mm:ss") + ": " + message);
    }
}