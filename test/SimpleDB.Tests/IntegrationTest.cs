namespace SimpleDB.Tests;

using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;

public class CSVDatabaseTest
{
    [Fact]
    public void IntegrationTest()
    {
        IDatabaseRepository<TestRecord> db = CSVDatabase<TestRecord>.Instance;

        TestRecord test = new TestRecord("Test!!!!");

        db.Store(test);
        Assert.Equal(test, db.Read(1).ElementAt(0));

    }


    [Fact]
    public async void HTTPGET()
    {
        using (Process process = new Process())
        {
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = $"run --project ../../../../../src/Chirp.CSVDBService/Chirp.CSVDBService.csproj";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            var baseURL = "http://localhost:5086";
            using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri(baseURL);
            var HTTPResponse = await client.GetAsync("cheeps");
            Console.WriteLine(HTTPResponse);
            Assert.True(HTTPResponse.IsSuccessStatusCode);

            var text = HTTPResponse.Content; //todo does the content contain the right things 
            Console.Write(text);

            process.Kill(true);
            process.WaitForExit();
            process.Dispose();

        }
    }
    [Fact]
    public async void HTTPPOST()
    {
        using (Process process = new Process())
        {
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = $"run --project ../../../../../src/Chirp.CSVDBService/Chirp.CSVDBService.csproj";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            var baseURL = "http://localhost:5086";
            using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri(baseURL);

            var cheepjson = new Cheep("mig", "gyaatt", 0);

            var cheep = await client.PostAsJsonAsync<Cheep>(baseURL + "/cheep", cheepjson);
            Console.WriteLine("The cheep: " + cheep);
            Assert.Equal(200,(int)cheep.StatusCode);


            process.Kill(true);
            process.WaitForExit();
            process.Dispose();

        }
    }
}

public record TestRecord(string name);