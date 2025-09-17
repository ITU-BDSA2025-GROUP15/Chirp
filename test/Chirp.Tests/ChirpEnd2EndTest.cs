namespace Chirp.Tests;

using System.Diagnostics;

public class ChirpEnd2EndTest
{
    [Fact]
    public void ReadCommand_ReturnsLatestCheep()
    {
        using (Process process = new Process())
        {
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = "run --project src/Chirp.csproj read 1";
            process.StartInfo.WorkingDirectory = "../../../../../";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();

            // Assert: Output should contain the latest cheep
            Assert.Contains("ropf", output);
            Assert.Contains("Cheeping cheeps on Chirp :)", output);
            Assert.Contains("08/02/23 15:04:47", output);
            process.WaitForExit();
        }
    }
    [Fact]
    public void CheepCommand_CheepsCheep() {
        using (Process process = new Process())
        {
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = "run --project src/Chirp.csproj cheep LOL --dry-run";
            process.StartInfo.WorkingDirectory = "../../../../../";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            
            process.Start();
            process.WaitForExit();
        }

        using (Process process = new Process())
        {
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = "run --project src/Chirp.csproj read 1";
            process.StartInfo.WorkingDirectory = "../../../../../";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();

            // Assert: Output should contain the latest cheep
            Assert.Contains("LOL", output);
            process.WaitForExit();
        }
    }
}
