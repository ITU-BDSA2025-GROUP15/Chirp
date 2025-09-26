namespace Chirp.Tests;

using System.Diagnostics;
[Collection("Sequential")]
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
                // Arrange
                var author = "ropf";
                var message = "Cheeping cheeps on Chirp :)";
                var t = DateTimeOffset.FromUnixTimeSeconds(1690981487);

                t = TimeZoneInfo.ConvertTime(t, TimeZoneInfo.Local);
                var timeString = t.ToString("MM/dd/yy HH:mm:ss");

                // Act
                process.StartInfo.FileName = "dotnet";
                process.StartInfo.Arguments = $"run --project ../../../../../{TestUtils.project_path} read 1";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();

                string output = process.StandardOutput.ReadToEnd();
                var outputLines = output.Split("\n", StringSplitOptions.RemoveEmptyEntries);

                // Assert
                // Output should contain the latest cheep
                Assert.Contains(author, output);
                Assert.Contains(message, output);

                // Should contain correct time format
                Assert.Contains(timeString, output);

                // Only one cheep returned
                Assert.Single(outputLines);
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
            // Arrange
            var inputMessage = "LOL";
            string output;

            // Act
            using (Process process = new Process())
            {
                process.StartInfo.FileName = "dotnet";
                process.StartInfo.Arguments = $"run --project ../../../../../{TestUtils.project_path} cheep {inputMessage}";
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
                process2.WaitForExit();

                output = process2.StandardOutput.ReadToEnd();
            }

            // Assert: Output should contain the latest cheep
            Assert.Contains(inputMessage, output);
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
}
