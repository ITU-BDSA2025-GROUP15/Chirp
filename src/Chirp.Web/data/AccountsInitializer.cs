using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

public class AccountsInitializer
{
    private readonly UserManager<Author> _userManager;
    private readonly IUserStore<Author> _userStore;
    private readonly IUserEmailStore<Author> _emailStore;

    public AccountsInitializer(
        UserManager<Author> userManager,
        IUserStore<Author> userStore,
        IUserEmailStore<Author> emailStore
    )
    {
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = emailStore;
    }

    private async Task<bool> CreateAccount(string name, string email, string password)
    {
        var user = new Author { Name = name };

        await _userStore.SetUserNameAsync(user, email, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, email, CancellationToken.None);
        var result = await _userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _userManager.ConfirmEmailAsync(user, token);

            return true;
        }
        else
        {
            return false;
        }
    }
    
    public async void SeedAccounts()
    {
        await CreateAccount("Helge", "ropf@itu.dk", "LetM31n!");
        await CreateAccount("Adrian", "adho@itu.dk", "M32Want_Access");
    }
}