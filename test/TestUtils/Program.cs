
using System.Diagnostics;
class StartWebService{
    static Process startWebService(string pathToDBproj)
    {
        Process process = new Process();
        process.StartInfo.FileName = "dotnet";
        process.StartInfo.Arguments = $"run --project {pathToDBproj}";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.Start();
        return process;
    }
}