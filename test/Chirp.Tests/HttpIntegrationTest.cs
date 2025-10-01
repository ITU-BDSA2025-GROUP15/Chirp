namespace Chirp.Tests;

using System.Net.Http.Headers;
using System.Net.Http.Json;

[Collection("Sequential")]
public class HttpIntegrationTest
{
    public HttpClient GetClient()
    {
        var baseURL = "http://localhost:5086";
        HttpClient client = new();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.BaseAddress = new Uri(baseURL);

        return client;
    }

    [Fact]
    public async void HTTPGET()
    {
        var original_file = File.ReadAllBytes(TestUtils.csv_db_path);
        var process = await TestUtils.startWebService();
        try
        {
            // Act
            var client = GetClient();
            var HTTPResponse = await client.GetAsync("cheeps");

            int statusCode = (int)HTTPResponse.StatusCode;
            var cheep = await client.GetFromJsonAsync<IEnumerable<Cheep>>("cheeps");

            // Assert
            Assert.Equal(200, statusCode);
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
            // Arrange
            var cheepjson = new Cheep("mig", "gyaatt", 0);

            // Act
            var client = GetClient();
            var cheep = await client.PostAsJsonAsync<Cheep>("cheep", cheepjson);
            
            // Assert
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