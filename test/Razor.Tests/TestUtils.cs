using System.Diagnostics;

public static class TestUtils
{
    public static readonly string RazorPath = "src/Chirp.Razor/Chirp.Razor.csproj";

    public static async Task<Process> StartRazorPage()
    {
        Process process = new Process();
        process.StartInfo.FileName = "dotnet";
        process.StartInfo.Arguments = $"run --project ../../../../../{RazorPath}";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.Start();

        var baseURL = "http://localhost:5273";
        using HttpClient client = new();
        client.BaseAddress = new Uri(baseURL);

        int maxRetries = 10;
        for (int i = 0; i < maxRetries; i++)
        {
            try
            {
                var HTTPResponse = await client.GetAsync("");
                break;
            }
            catch (Exception)
            {
                Thread.Sleep(10000);
            }
        }

        return process;
    }

    public static void SetupTestDb()
    {
        var tempFilePath = Path.GetTempFileName();
        var testDb = File.ReadAllBytes("../../../chirp-test.db");
        File.WriteAllBytes(tempFilePath, testDb);
        Environment.SetEnvironmentVariable("CHIRPDBPATH", tempFilePath);
    }

    public static void AssertCheepListsEqual(List<Cheep> expected, List<Cheep> actual)
    {
        Assert.Equal(expected.Count, actual.Count);
        for (int i = 0; i < expected.Count; i++)
        {
            Assert.Equal(expected[i].Author.Name, actual[i].Author.Name);
            Assert.Equal(expected[i].AuthorId, actual[i].AuthorId);
            Assert.Equal(expected[i].Text, actual[i].Text);
            Assert.Equal(expected[i].TimeStamp.ToString(), actual[i].TimeStamp.ToString());
        }
    }

    public static void AssertCheepDTOListsEqual(List<CheepDTO> expected, List<CheepDTO> actual)
    {
        Assert.Equal(expected.Count, actual.Count);
        for (int i = 0; i < expected.Count; i++)
        {
            Assert.Equal(expected[i].Author, actual[i].Author);
            Assert.Equal(expected[i].Message, actual[i].Message);
            Assert.Equal(expected[i].Timestamp, actual[i].Timestamp);
        }
    }
}