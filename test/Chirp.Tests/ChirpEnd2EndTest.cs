namespace Chirp.Tests;

using System.Diagnostics;

public class ChirpEnd2EndTest
{
    readonly string project_path = "src/Chirp.CLI.Client/Chirp.CLI.Client.csproj";
    readonly string web_service_path = "src/Chirp.CSVDBService/Chirp.CSVDBService.csproj";
    readonly string csv_db_path = "../../../../../src/Chirp.CSVDBService/chirp_cli_db.csv";

    async Task<Process> startWebService()
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

    [Fact]
    public async void ReadCommand_ReturnsLatestCheep()
    {
        var original_file = File.ReadAllBytes(csv_db_path);
        Process WebProcess = await startWebService();

        using (Process process = new Process())
        {
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = $"run --project ../../../../../{project_path} read 1";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            // Assert: Output should contain the latest cheep
            Assert.Contains("ropf", output);
            Assert.Contains("Cheeping cheeps on Chirp :)", output);

            //
            var t = DateTimeOffset.FromUnixTimeSeconds(1690981487);
            t = TimeZoneInfo.ConvertTime(t, TimeZoneInfo.Local);
            var tString = t.ToString("MM/dd/yy HH:mm:ss");

            Assert.Contains(tString, output);
            process.WaitForExit();

            WebProcess.Kill(true);
            WebProcess.WaitForExit();
            WebProcess.Dispose();

            // restore original file
            File.WriteAllBytes(csv_db_path, original_file);
        }
    }
    [Fact]
    public async void CheepCommand_CheepsCheep()
    {
        var original_file = File.ReadAllBytes(csv_db_path);
        Process WebProcess = await startWebService();

        using (Process process = new Process())
        {
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = $"run --project ../../../../../{project_path} cheep LOL";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;

            process.Start();
            process.WaitForExit();
        }
        using (Process process2 = new Process())
        {
            process2.StartInfo.FileName = "dotnet";
            process2.StartInfo.Arguments = $"run --project ../../../../../{project_path} read 1";
            process2.StartInfo.UseShellExecute = false;
            process2.StartInfo.RedirectStandardOutput = true;
            process2.Start();
            string output = process2.StandardOutput.ReadToEnd();

            // Assert: Output should contain the latest cheep
            Assert.Contains("LOL", output);
            process2.WaitForExit();

            WebProcess.Kill(true);
            WebProcess.WaitForExit();
            WebProcess.Dispose();

            // restore original file
            File.WriteAllBytes(csv_db_path, original_file);
        }
    }
}
