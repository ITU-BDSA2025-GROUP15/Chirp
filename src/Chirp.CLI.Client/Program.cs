// See https://aka.ms/new-console-template for more information

using System.ComponentModel;
using System.Globalization;
using System.Reflection.Metadata;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using DocoptNet;

// Create an HTTP client object
var baseURL = "http://localhost:5086";
using HttpClient client = new();
client.DefaultRequestHeaders.Accept.Clear();
client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
client.BaseAddress = new Uri(baseURL);

// Adds cheep to database file.
async Task cheep(string message)
{
    var username = Environment.UserName;
    var timeOfCheep = new DateTimeOffset(DateTime.UtcNow, TimeSpan.Zero);

    var cheep = new Cheep(username, message, timeOfCheep.ToUnixTimeSeconds());
    await client.PostAsJsonAsync<Cheep>(baseURL + "/cheep", cheep);
}

// Reads all posts from database file. 
async Task read(int? limit)
{
    // Send an asynchronous HTTP GET request and automatically construct a Cheep object from the
    // JSON object in the body of the response
    var cheep = await client.GetFromJsonAsync<IEnumerable<Cheep>>("cheeps");

    if (cheep == null) throw new Exception("No cheep!");

    UserInterface.PrintCheeps(cheep);
}


const string usage = @"Chirp CLI.

Usage:
    Chirp cheep <message>
    Chirp read [<limit>]

Options:
";

static int ShowHelp(string help) { Console.WriteLine(help); return 0; }
static int ShowVersion(string version) { Console.WriteLine(version); return 0; }
static int OnError(string usage) { Console.Error.WriteLine(usage); return 1; }

async Task<int> Run(IDictionary<string, ArgValue> arguments)
{
    foreach (var (key, value) in arguments)
    {
        if (key == "cheep" && (bool)value)
        {
            await cheep((string)arguments["<message>"]);
        }

        if (key == "read" && (bool)value)
        {
            var limit = arguments["<limit>"];
            if (limit.IsString)
            {
                bool check = int.TryParse((string)limit, out int arg);
                await read(check ? arg : null);
            }
            else await read(null);
        }
    }
    return 0;
}

return await Docopt.CreateParser(usage)
             .WithVersion("1.0")
             .Parse(args)
             .Match(Run,
                    result => Task.FromResult(ShowHelp(result.Help)),
                    result => Task.FromResult(ShowVersion(result.Version)),
                    result => Task.FromResult(OnError(result.Usage)));