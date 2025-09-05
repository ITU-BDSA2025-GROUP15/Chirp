// See https://aka.ms/new-console-template for more information

using System.ComponentModel;
using System.Globalization;
using System.Reflection.Metadata;
using CsvHelper;
using SimpleDB;

if (args.Length == 0)
{
    System.Console.WriteLine("No args given!");
    return;
}

IDatabaseRepository<Cheep> database = new CSVDatabase<Cheep>();

switch (args[0])
{
    case "cheep":
        cheep(args[1]);
        break;
    case "read":
        if (args.Length == 1) read(null);
        else {
            int arg;
            bool check = int.TryParse(args[1], out arg);
            read(check ? arg : null);
        };
        break;
    default:
        break;
}

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