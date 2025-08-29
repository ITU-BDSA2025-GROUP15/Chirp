// See https://aka.ms/new-console-template for more information

using System.ComponentModel;

var filename = "./chirp_cli_db.csv";
var reader = new StreamReader(filename);
reader.ReadLine(); // to not read the header
while (!reader.EndOfStream)
{
    var line = reader.ReadLine();

    var firstComma = line.IndexOf(",");
    var lastComma = line.LastIndexOf(",");
    
    var author = line.Substring(0,  firstComma);
    var message = line.Substring(firstComma+2, lastComma - firstComma - 3);
    var tempTimestamp = line.Substring(lastComma + 1);
    var timestamp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(tempTimestamp));
    Console.WriteLine(author + " @ " + timestamp.ToString("MM/dd/yy HH:mm:ss") + ": " + message);
    
}