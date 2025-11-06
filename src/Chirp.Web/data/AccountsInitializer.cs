using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

public class AccountsInitializer
{
    private readonly UserManager<Author> _userManager;
    private readonly IUserStore<Author> _userStore;
    private readonly IUserEmailStore<Author> _emailStore;
    private readonly ChirpDBContext _chirpContext;

    public AccountsInitializer(
        UserManager<Author> userManager,
        IUserStore<Author> userStore,
        IUserEmailStore<Author> emailStore,
        ChirpDBContext chirpContext
    )
    {
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = emailStore;
        _chirpContext = chirpContext;
    }

    private async Task<Author?> CreateAccount(int id, string name, string email, string password)
    {
        var user = new Author { Id = id, Name = name };

        await _userStore.SetUserNameAsync(user, email, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, email, CancellationToken.None);

        try
        {
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await _userManager.ConfirmEmailAsync(user, token);

                return user;
            }
            else
            {
                return null;
            }
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }
    
    public async void SeedAccounts()
    {
        var helge = await CreateAccount(11, "Helge", "ropf@itu.dk", "LetM31n!");
        var adrian = await CreateAccount(12, "Adrian", "adho@itu.dk", "M32Want_Access");

        var cheeps = new List<Cheep>();

        if (helge != null) cheeps.Add(new Cheep() { CheepId = 656, AuthorId = helge.AuthorId, Author = helge, Text = "Hello, BDSA students!", TimeStamp = DateTime.Parse("2023-08-01 12:16:48") });
        if (adrian != null) cheeps.Add(new Cheep() { CheepId = 657, AuthorId = adrian.AuthorId, Author = adrian, Text = "Hej, velkommen til kurset.", TimeStamp = DateTime.Parse("2023-08-01 13:08:28") });
  
        _chirpContext.Cheeps.AddRange(cheeps);
        _chirpContext.SaveChanges();
    }
}