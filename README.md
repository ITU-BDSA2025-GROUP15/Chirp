# Chirp!
The first online message board of all time. Chat with your friends, chat with randoms, share your opinion, anything can happen!

## Before you run Chirp!
The Chirp! application supports environment variables to customize your setup. Be sure to register any needed environment variables before you run the server.

### Setting up GitHub authentication
Chirp! supports GitHub integration via OAuth. To enable this feature, you must first [register an OAuth application on GitHub](https://docs.github.com/en/apps/oauth-apps/building-oauth-apps/creating-an-oauth-app). 

For registering the OAuth app (assuming you are hosting on `http://localhost:5000`), use the following values:
- Homepage URL: `http://localhost:5000`
- Authorization callback URL: `http://localhost:5000/signin-github`

Then you can get a client ID + client secret, which you must register in the following environment variables:
```
authentication:github:clientId=<Your Client ID>
authentication:github:clientSecret=<Your Client Secret>
```
Alternatively, if you have downloaded the Chirp! source code, you can use `dotnet user-secrets` to register them instead:
```
dotnet user-secrets set "authentication:github:clientId" <Your Client ID>
dotnet user-secrets set "authentication:github:clientSecret" <Your Client Secret>
```
Your Chirp! instance will now use your GitHub OAuth application to allow GitHub logins on the site.

### Setting up Google authentication
Chirp! also supports Google authentication via OAuth. To enable this feature, [register an OAuth application on Google Cloud Console](https://console.cloud.google.com/).

For registering the OAuth app (assuming you are hosting on `http://localhost:5000`), add the following authorized redirect URI:
- `http://localhost:5000/signin-google`

For production:
- `https://yourdomain.com/signin-google`

Then you can get a client ID + client secret, which you must register in the following environment variables:
```
authentication:google:clientId=<Your Client ID>
authentication:google:clientSecret=<Your Client Secret>
```
Alternatively, if you have downloaded the Chirp! source code, you can use `dotnet user-secrets` to register them instead:
```
dotnet user-secrets set "authentication:google:clientId" <Your Client ID>
dotnet user-secrets set "authentication:google:clientSecret" <Your Client Secret>
```
Your Chirp! instance will now use your Google OAuth application to allow Google logins on the site.

### Setting a custom database location
By default, Chirp! will store the database in your temporary directory as `chirp.db`. 

You can specify a custom location for the database using the environment variable `CHIRPDBPATH`. For example:
```
CHIRPDBPATH=/home/user/my-database-folder/chirp.db
```
## Running Chirp! from release builds
Chirp! is released for 64-bit versions of Windows, Mac and Linux. Releases can be downloaded [here.](https://github.com/ITU-BDSA2025-GROUP15/Chirp/releases)
### How to run 
Depending on your operating system, the executable will have a different name (e.g. `Chirp.Web.exe` on Windows, `Chirp.Web` on Mac/Linux). In your terminal of choice, run the following:
```
./Chirp.Web
```
The Chirp! server will start. The server is ready when you see output similar to this:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Production
info: Microsoft.Hosting.Lifetime[0]
      Content root path: /Chirp
```
You can now access the Chirp! service in your browser on the following link: http://localhost:5000
## Running Chirp! from source code
### How to build Chirp!
1. First of all, make sure you have installed .NET version 8.0. You can get it [here.](https://dotnet.microsoft.com/download/dotnet/8.0)
2. Open your terminal and navigate to the root folder of the project.
3. Run the following command: `dotnet build ./src/Chirp.Web/Chirp.Web.csproj`
4. You should now successfully have built the project.
### How to run Chirp!
1. Begin by navigating to the Chirp.Web folder found under src in the terminal.
2. Now run the command `dotnet run`. This will start the server.
3. In your Web browser open `localhost:5273`. This will also appear in the terminal.
4. You have now opened Chirp!
### How to run Chirp! tests
Install playwright into the Razor.Tests project folder. 
Navigate to the test project in your terminal and run the following:
```
pwsh bin/Debug/net8.0/playwright.ps1 install --with-deps
```

For further issues with Playwright, consult [Playwright.](https://playwright.dev/dotnet/docs/intro)

Now the tests can be run with the command `dotnet test`. 