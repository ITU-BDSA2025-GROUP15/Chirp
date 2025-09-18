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
            Assert.True(output.Contains("08/02/23 15:04:47") || output.Contains("08/02/23 15.04.47"));
            process.WaitForExit();
        }
    }
    [Fact]
    public void CheepCommand_CheepsCheep()
    {
        File.WriteAllText("chirp_cli_db.csv", @"Author,Message,Timestamp
            ropf,""Hello, BDSA students!"",1690891760
            adho,""Welcome to the course!"",1690978778
            adho,""I hope you had a good summer."",1690979858
            ropf,""Cheeping cheeps on Chirp :)"",1690981487
        ");

        using (Process process = new Process())
        {
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = "run --project ../../../../../src/Chirp.csproj cheep LOL";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;

            process.Start();
            process.WaitForExit();
        }
        using (Process process2 = new Process())
        {
            process2.StartInfo.FileName = "dotnet";
            process2.StartInfo.Arguments = "run --project ../../../../../src/Chirp.csproj read 1";
            process2.StartInfo.UseShellExecute = false;
            process2.StartInfo.RedirectStandardOutput = true;
            process2.Start();
            string output = process2.StandardOutput.ReadToEnd();

            // Assert: Output should contain the latest cheep
            Assert.Contains("LOL", output);
                process2.WaitForExit();
            }
    }
}
