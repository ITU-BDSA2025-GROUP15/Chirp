// See https://aka.ms/new-console-template for more information

using System.ComponentModel;
using System.Globalization;
using System.Reflection.Metadata;
using CsvHelper;
using SimpleDB;
using DocoptNet;

IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>();

// Adds cheep to database file.
void cheep(string message)
{
    var username = Environment.UserName;
    var timeOfCheep = new DateTimeOffset(DateTime.UtcNow, TimeSpan.Zero);

    var cheep = new Cheep(username, message, timeOfCheep.ToUnixTimeSeconds());
    database.Store(cheep);
}

// Reads all posts from database file. 
void read(int? limit)
{
    UserInterface.PrintCheeps(database.Read(limit));
}


const string usage = @"Chirp CLI.

Usage:
    Chirp cheep <message>
    Chirp read <limit>

Options:
";

static int ShowHelp(string help) { Console.WriteLine(help); return 0; }
static int ShowVersion(string version) { Console.WriteLine(version); return 0; }
static int OnError(string usage) { Console.Error.WriteLine(usage); return 1; }

int Run(IDictionary<string, ArgValue> arguments)
{
    foreach (var (key, value) in arguments) {
        if (key == "cheep" && (bool)value) {
            cheep((string)arguments["<message>"]);
        };

        if (key == "read" && (bool)value) {
            bool check = int.TryParse((string)arguments["<limit>"], out int arg);
            read(check ? arg : null);
        };
    }
    return 0;
}

return Docopt.CreateParser(usage)
             .WithVersion("1.0")
             .Parse(args)
             .Match(Run,
                    result => ShowHelp(result.Help),
                    result => ShowVersion(result.Version),
                    result => OnError(result.Usage));