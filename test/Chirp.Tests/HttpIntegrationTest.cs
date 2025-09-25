using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;

[Collection("Sequential")]
public class HttpIntegrationTest
{
    [Fact]
    public async void HTTPGET()
    {
        var original_file = File.ReadAllBytes(TestUtils.csv_db_path);
        var process = await TestUtils.startWebService();
        try
        {
            var baseURL = "http://localhost:5086";
            using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri(baseURL);
            var HTTPResponse = await client.GetAsync("cheeps");

            Console.WriteLine(HTTPResponse);
            Assert.Equal(200, (int)HTTPResponse.StatusCode);

            var cheep = await client.GetFromJsonAsync<IEnumerable<Cheep>>("cheeps");
            Assert.NotNull(cheep);
        }
        finally
        {
            process.Kill(true);
            process.WaitForExit();
            process.Dispose();

            // restore original file
            File.WriteAllBytes(TestUtils.csv_db_path, original_file);
        }
        Thread.Sleep(1000);
    }
    [Fact]
    public async void HTTPPOST()
    {
        var original_file = File.ReadAllBytes(TestUtils.csv_db_path);
        var process = await TestUtils.startWebService();
        try
        {
            var baseURL = "http://localhost:5086";
            using HttpClient client = new();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri(baseURL);

            var cheepjson = new Cheep("mig", "gyaatt", 0);

            var cheep = await client.PostAsJsonAsync<Cheep>(baseURL + "/cheep", cheepjson);
            Console.WriteLine("The cheep: " + cheep);
            Assert.Equal(200, (int)cheep.StatusCode);
        }
        finally
        {
            process.Kill(true);
            process.WaitForExit();
            process.Dispose();

            // restore original file
            File.WriteAllBytes(TestUtils.csv_db_path, original_file);
        }
        Thread.Sleep(1000);
    }
}