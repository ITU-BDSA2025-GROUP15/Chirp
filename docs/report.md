---
title: _Chirp!_ Project Report
subtitle: ITU BDSA 2025 Group `<15>`
author:
- "Frederik Haraszuk Stein <frhs@itu.dk>"
- "Hjalte Krohn Frandsen <hjfr@itu.dk>"
- "Kenny Izaak Egelund <keeg@itu.dk>"
- "Lucas Tambour Fabricius <lufa@itu.dk>"
- "Omar Osama Al Hassan <omal@itu.dk>"
- "Tobias Mondrup Holm <tmho@itu.dk>"
numbersections: true
toc: true
---
\newpage

# Design and Architecture of _Chirp!_

## Domain model

![domain](./images/domain.svg)

Here is the domain model for our Chirp! project. The model extends the IdentityUser class for the Author, which allows us to use ASP.NET Identity for handling user authentication. We have also added Follows to track who an author is following, and LikeCounter to track likes on a cheep.

## Architecture — In the small

The Figure below shows an illustration of how we implemented the Onion Architecture to our web application Chirp. This was done with the intention of keeping easy maintainability and testability throughout the project, such that the Dependency Inversion Principle is kept. High-level modules like our Chirp.Core, doesn't depend on Low-level modules like Chirp.Web, which as a developer ensures changes within the application can be easily adjusted, so modules doesn't interfere with the logic and functionality across the whole program.
As such our application has been split up in different layers of dependency.

The Domain layer: The center, Chirp.Core, that consist of our domain model and DTO's which are fully independent.

A second and third layer, that is made of the Infrastructure.Chirp. Chirp Repository in the second layer, which access and manages the data. And Chirp Services in the third layer, that serve as a flow of data between the repositories and the outer layer.

And finally the most outer layer, Chirp.Web, that handles, User interface, User interactions, page rendering and testing, which is fully dependent across all the inner layers.

![Illustration of the _Chirp!_ onion architecture](./images/Onion.png)

Our Chirp! application follows the onion architecture. In the onion architecture, we ensure that outer layers depend only on the next inner layer, which ensures loose coupling. Loose coupling leads to better maintainability and testability.

The inner core layer contains our Data Transfer Objects (DTOs), which are simplified versions of our actual domain model objects.

We then have our infrastructure class containing the repository and service layer. The repository layer handles communication with the database, while the service layer prepares data for the UI layer.

The outer layer is the UI/Web layer. It contains all the components for our Razor pages, which are used for running our web service.

Finally, we have our test suite that covers all layers of the onion.

## Architecture of deployed application

![Illustration of the _Chirp!_ deployment architecture](./images/deployment.svg)

This is the architecture of the deployed Chirp! application. The server is deployed to Azure App Service, where the Chirp! software stack (structured after the onion architecture) is running. 

Clients connect to the Chirp.Web service via HTTPS through their browser. The Chirp.Web service then serves a rendered page via Razor Pages.

To support external logins in Chirp!, we also maintain OAuth connections to the login providers. For our project, we have added both GitHub and Google as OAuth login providers.

## User activities
![Illustration of User activities](./images/User_act.svg)
Here is an activity diagram. This diagram shows the difference in what an authorized and unauthorized user can in the application.  

## Sequence of functionality/calls through _Chirp!_

![Illustration of call sequence when new user accesses the Chirp! web page.](./images/sequence.svg)

Here is the call sequence for when a non-authenticated user accesses the front page of Chirp!. 

The web layer processes the request in the OnGet() method, which first checks whether the user is logged in. This is used to display the correct header.

Then we call LoadCheeps(), a helper method which queries the service layer and retrieves a list of CheepDTOs. The service calls the repository, which in turn calls the SQLite database using EFCore. 

Finally, the page is rendered in Razor Pages and sent to the user.

# Process

## Build, test, release, and deployment
![Activity diagram showing deployments via GitHub Actions workflows.](./images/github_workflow.svg)


Chirp! is built, tested, released, and deployed using automated GitHub Actions workflows. We have three workflows relevant for the deployment of the Chirp! application:
- Push/PR update: Builds and tests the project, ensuring all tests pass. If anything fails, the PR is blocked.
- Push/PR merge to main: Creates a Chirp! ZIP with release configuration, which is then deployed on the Azure web app.
- Push to tag v\*.\*.\*: Creates a ZIP and adds it to release with the pushed tag. Note: this workflow runs as a matrix, creating ZIPs for Windows, macOS and Linux.

The GitHub repository also has a discord notification workflow that pings a discord channel every time a new pull request was made. This is because discord was our main platform of communication and the easiest way of getting a hold of us.  

## Team work
![The final GitHub project board for our Chirp! project.](./images/project_board.png)


All required features for the Chirp! project should be in place, with most features having been covered by either a unit test, integration test or end-to-end test. However, there are still some bug fixes, edge cases and other improvements to be made.

One thing that we didn't implement is checking for duplicate names when someone signs up through GitHub integration. Imagine that a user has the username "Bob". If another user signs up through the GitHub integration with a GitHub username that is also "Bob", the backend does not check if this username is already taken. This could result in a crash.

Here are some other bugs/features we didn't end up fixing/implementing:

- Not clicking email confirmation breaks the user. 
- More unit test to make sure our distribution of test are like a pyramid.
- Mock services for tests. 
- More expansive fuzz testing.

#### From issue to merge

![Flowchart of issues from creation to merge](<images/from issue to merge2.svg>)
This flowchart shows the process from the creation of an issue to when the related branch is merged into the main branch. When a new issue is created, our GitHub workflow automatically sets its status to "To-do" in our project board. 

## How to make _Chirp!_ work locally

### Before you run Chirp!
The Chirp! application supports environment variables to customize your setup. Be sure to register any needed environment variables before you run the server.

#### Setting up GitHub authentication
Chirp! supports GitHub integration via OAuth. Without this setup the feature to login with github does not work when running Chirp! locally. 
To enable this feature, you must first [register an OAuth application on GitHub](https://docs.github.com/en/apps/oauth-apps/building-oauth-apps/creating-an-oauth-app). 

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

#### Setting up Google authentication
Chirp! also supports Google authentication via OAuth. To enable this feature, [register an OAuth application on Google Cloud Console](https://console.cloud.google.com/).

For registering the OAuth app (assuming you are hosting on `http://localhost:5000`), add the following authorized redirect URI:
- `http://localhost:5000/signin-google`


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

#### Setting a custom database location
By default, Chirp! will store the database in your temporary directory as `chirp.db`. 

You can specify a custom location for the database using the environment variable `CHIRPDBPATH`. For example:
```
CHIRPDBPATH=/home/user/my-database-folder/chirp.db
```
### Running Chirp! from release builds
Chirp! is released for 64-bit versions of Windows, Mac and Linux. Releases can be downloaded [here.](https://github.com/ITU-BDSA2025-GROUP15/Chirp/releases)
#### How to run 
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
### Running Chirp! from source code
#### Clone source code
1. Make sure git is installed. [Installation guide](https://git-scm.com/install/).
2. To clone the source code go to your terminal and type: `git clone https://github.com/ITU-BDSA2025-GROUP15/Chirp.git`
3. You should see output similar to this:
```
Cloning into 'Chirp'...
remote: Enumerating objects: 2750, done.
remote: Counting objects: 100% (490/490), done.
remote: Compressing objects: 100% (313/313), done.
remote: Total 2750 (delta 293), reused 196 (delta 175), pack-reused 2260 (from 3)
Receiving objects: 100% (2750/2750), 110.96 MiB | 21.22 MiB/s, done.
Resolving deltas: 100% (1651/1651), done.
```
4. The folder should now be cloned into a new folder called `Chirp`.
#### How to build Chirp!
1. Make sure you have installed .NET version 8.0. You can get it [here.](https://dotnet.microsoft.com/download/dotnet/8.0)
2. Open your terminal and navigate to the root folder of the project.
3. Run the following command: `dotnet build ./src/Chirp.Web/Chirp.Web.csproj`
4. You should now successfully have built the project.
#### How to run Chirp!
1. Begin by navigating to the Chirp.Web folder found under src in the terminal.
2. Now run the command `dotnet run`. This will start the server.
3. In your Web browser open `localhost:5273`. This will also appear in the terminal.
4. You have now opened Chirp!

## How to run test suite locally
1. First make sure the project is build by navigating to the root folder of Chirp and run
      ```
      dotnet build
      ```
2. Make sure PowerShell is installed. [PowerShell installation guide](https://learn.microsoft.com/en-us/powershell/scripting/install/install-powershell?view=powershell-7.5).
3. Install playwright into the Razor.Tests project folder. 
-  Navigate to the test project in your terminal and run the following:
      ```
      pwsh bin/Debug/net8.0/playwright.ps1 install --with-deps
      ```

- For further issues with Playwright, consult [Playwright.](https://playwright.dev/dotnet/docs/intro)

4. Now the tests can be run with the command `dotnet test` (This may take a moment). 

There are three different kinds of test: unit test, integration tests and end-to-end tests. 
The unit tests are testing the individual methods to see if they work. The integration tests makes sure methods work together correctly.
The end-to-end tests the entire program running and therefore also handles tests that test security like SQL injection, XSS, and CSRF to ensure there are no security vulnerabilities. 

# Ethics

## License
For the applications license we chose the MIT license because of its simplicity and non-restrictive nature leading to more collaboration. The MIT license is also compatible with most other licenses. To see the full [license](https://github.com/ITU-BDSA2025-GROUP15/Chirp/blob/main/LICENSE). 

## LLMs, ChatGPT, Copilot, and others
During the group's work on the application LLMs were used to a limited degree. This can be seen in the contributors tab under insights where both Copilot and CodeFactor has been attributed to a total of 25 commits. ChatGPT also has some attributions to commits, but they do not show up in GitHub insights because ChatGPT does not have a GitHub account.

CodeFactor, unlike ChatGPT and Copilot, is not an LLM, but a bot that does static analysis based on rules and therefore consumes much less power than an LLM. Although the exact numbers on how much energy and water an LLM uses is hard to figure out, due to the companies not releasing official numbers, the amount is likely high and is the reason the group only used the LLMs to a limited degree. 
CodeFactor caught many common mistakes like too many newlines or other redundancies and did improve overall code quality.

Whether the LLMs sped up or slowed down our process depended on the response gotten from the LLMs. Issue [#123](https://github.com/ITU-BDSA2025-GROUP15/Chirp/pull/123) where Copilot reduced the size of our release files without us having to research how one might do so, was an example of LLMs speeding up our process. There were also instances where the LLMs slowed down our process by leading us astray or coming up with incorrect solutions.