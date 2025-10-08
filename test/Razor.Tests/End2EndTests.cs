using System.Diagnostics;

public class End2EndTests
{
    public readonly string Razor_path = "src/Chirp.Razor/Chirp.Razor.csproj";

    public async Task<Process> startRazorPage()
    {
        Process process = new Process();
        process.StartInfo.FileName = "dotnet";
        process.StartInfo.Arguments = $"run --project ../../../../../{Razor_path}";
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;
        process.Start();

        var baseURL = "http://localhost:5273";
        using HttpClient client = new();
        client.BaseAddress = new Uri(baseURL);

        int max_retries = 10;
        for (int i = 0; i < max_retries; i++)
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

    [Fact]
    public async void End2End()
    {
        Process razorPage = await startRazorPage();

        using (Process process = new Process())
        {
            try
            {
                //Arrange
                var baseURL = "http://localhost:5273/";
                using HttpClient client = new();
                client.BaseAddress = new Uri(baseURL);

                //Act

                //Default page
                var HTTPResponsePageDefault = await client.GetAsync("/");
                string responseBodyPageDefault = await HTTPResponsePageDefault.Content.ReadAsStringAsync();

                //Page1
                var HTTPResponsePage1 = await client.GetAsync("/?page=1");
                string responsebodyPage1 = await HTTPResponsePage1.Content.ReadAsStringAsync();

                //Page2
                var HTTPResponsePage2 = await client.GetAsync("/?page=2");
                string responseBodyPage2 = await HTTPResponsePage2.Content.ReadAsStringAsync();

                //user page Adrian
                var HTTPResponseUser = await client.GetAsync("/Adrian");
                string responseBodyUser = await HTTPResponseUser.Content.ReadAsStringAsync();


                //Assert

                Assert.Contains("Starbuck now is what we hear the worst.", responseBodyPageDefault); //cheep that should be on the first page

                Assert.Equal(responseBodyPageDefault, responsebodyPage1);//page 1 and default is the same

                Assert.Contains("It is asking much of it in the world.", responseBodyPage2); //cheep on page 2
                Assert.Contains("Jacqualine Gilcoine", responseBodyPage2);
                Assert.NotEqual(responseBodyPageDefault, responseBodyPage2); //page 1 and 2 not equal

                Assert.Contains(@"<strong>
                            <a href=""/Adrian"">Adrian</a>
                        </strong>
                        Hej, velkommen til kurset.
                        <small>&mdash; 08/01/23 13:08:28</small>"
                        , responseBodyUser);
                Assert.DoesNotContain("Jacqualine Gilcoine", responseBodyUser);

            }
            finally
            {
                razorPage.Kill(true);
                razorPage.WaitForExit();
                razorPage.Dispose();
            }
        }
        
    }

}