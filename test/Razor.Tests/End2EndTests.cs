using System.Diagnostics;
using System.Text.RegularExpressions;

using Xunit.Abstractions;

[Collection("Sequential")]
public class End2EndTests
{
    public readonly string Razor_path = "src/Chirp.Razor/Chirp.Razor.csproj";

    //for debugging test
    /*
    private readonly ITestOutputHelper _output;

    public End2EndTests(ITestOutputHelper output)
    {
        _output = output;
    }
    */
    [Fact]
    public async void End2End()
    {
        Process razorPage = await TestUtils.StartRazorPage();

        using (Process process = new Process())
        {
            try
            {
                // Arrange
                var expectedDateTime = DateTime.Parse("2023-08-01 13:08:28");
                var expectedDateTimeStr = expectedDateTime.ToString("MM/dd/yy H:mm:ss");
                var expectedFullStr = $"<strong><a href=\"/Adrian\">Adrian</a></strong>Hej, velkommen til kurset.<small>&mdash; {expectedDateTimeStr}";

                var baseURL = "http://localhost:5273/";
                using HttpClient client = new();
                client.BaseAddress = new Uri(baseURL);

                // Act
                // Default page
                var HTTPResponsePageDefault = await client.GetAsync("/");
                string responseBodyPageDefault = await HTTPResponsePageDefault.Content.ReadAsStringAsync();

                // Page 1
                var HTTPResponsePage1 = await client.GetAsync("/?page=1");
                string responseBodyPage1 = await HTTPResponsePage1.Content.ReadAsStringAsync();

                // Page 2
                var HTTPResponsePage2 = await client.GetAsync("/?page=2");
                string responseBodyPage2 = await HTTPResponsePage2.Content.ReadAsStringAsync();

                // User page Adrian
                var HTTPResponseUser = await client.GetAsync("/Adrian");
                string responseBodyUser = await HTTPResponseUser.Content.ReadAsStringAsync();

                responseBodyUser = responseBodyUser.Replace("\r\n", "\n"); // In case of Windows users
                responseBodyUser = Regex.Replace(responseBodyUser, "\n\\s*", ""); // Strip leading whitespace

                // Assert
                // Page 1 and default page is the same
                Assert.Equal(responseBodyPageDefault, responseBodyPage1);

                // Cheep that should be on the first page
                Assert.Contains("Starbuck now is what we hear the worst.", responseBodyPageDefault);

                // Cheep on page 2
                Assert.Contains("It is asking much of it in the world.", responseBodyPage2);
                Assert.Contains("Jacqualine Gilcoine", responseBodyPage2);

                // Page 1 and 2 not equal
                Assert.NotEqual(responseBodyPageDefault, responseBodyPage2);

                // Only Adrians posts should be on the user page
                // _output.WriteLine(responseBodyUser);
                Assert.Contains(expectedFullStr, responseBodyUser);
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