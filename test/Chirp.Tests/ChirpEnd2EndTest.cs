namespace Chirp.Tests;

using System.Diagnostics;

public class ChirpEnd2EndTest
{
    [Fact]
    public async void ReadCommand_ReturnsLatestCheep()
    {
        var original_file = File.ReadAllBytes(TestUtils.csv_db_path);
        Process WebProcess = await TestUtils.startWebService();

        using (Process process = new Process())
        {
            try
            {
                process.StartInfo.FileName = "dotnet";
                process.StartInfo.Arguments = $"run --project ../../../../../{TestUtils.project_path} read 1";
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
            }

            finally
            {
                WebProcess.Kill(true);
                WebProcess.WaitForExit();
                WebProcess.Dispose();

                // restore original file
                File.WriteAllBytes(TestUtils.csv_db_path, original_file);
            }
        }
        Thread.Sleep(1000);
    }
    [Fact]
    public async void CheepCommand_CheepsCheep()
    {
        var original_file = File.ReadAllBytes(TestUtils.csv_db_path);
        Process WebProcess = await TestUtils.startWebService();
        try
        {
            using (Process process = new Process())
            {

                process.StartInfo.FileName = "dotnet";
                process.StartInfo.Arguments = $"run --project ../../../../../{TestUtils.project_path} cheep LOL";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;

                process.Start();
                process.WaitForExit();
            }
            using (Process process2 = new Process())
            {
                process2.StartInfo.FileName = "dotnet";
                process2.StartInfo.Arguments = $"run --project ../../../../../{TestUtils.project_path} read 1";
                process2.StartInfo.UseShellExecute = false;
                process2.StartInfo.RedirectStandardOutput = true;
                process2.Start();
                string output = process2.StandardOutput.ReadToEnd();

                // Assert: Output should contain the latest cheep
                Assert.Contains("LOL", output);
                process2.WaitForExit();
            }
        }
        finally
        {
            WebProcess.Kill(true);
            WebProcess.WaitForExit();
            WebProcess.Dispose();

            // restore original file
            File.WriteAllBytes(TestUtils.csv_db_path, original_file);
        }
        Thread.Sleep(1000);
    }
}
