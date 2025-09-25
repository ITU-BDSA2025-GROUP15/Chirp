
using System.Diagnostics;
public static class TestUtils {
    public static readonly string project_path = "src/Chirp.CLI.Client/Chirp.CLI.Client.csproj";
    public static readonly string web_service_path = "src/Chirp.CSVDBService/Chirp.CSVDBService.csproj";
    public static readonly string csv_db_path = "../../../../../src/Chirp.CSVDBService/chirp_cli_db.csv";

    public static async Task<Process> startWebService()
    {
        Process process = new Process();
        process.StartInfo.FileName = "dotnet";
        process.StartInfo.Arguments = $"run --project ../../../../../{web_service_path}";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.Start();
        
        // Wait for HTTP to start
        var baseURL = "http://localhost:5086";
        using HttpClient client = new();
        client.BaseAddress = new Uri(baseURL);
        
        int max_retries = 10;
        for (int i = 0; i < max_retries; i++)
        {
            try {
                var HTTPResponse = await client.GetAsync("");
                break;
            } catch (Exception) {
                System.Console.WriteLine("L");
                i++;
            }   
        }

        return process;
    }
    
}