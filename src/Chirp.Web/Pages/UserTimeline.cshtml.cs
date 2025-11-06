using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PaginationModel
{
    private readonly UserManager<Author> _userManager;
    [BindProperty]
    public string Message { get; set; }

    public static Author CurrentAuthor;
    public UserTimelineModel(ICheepService service, UserManager<Author> userManager) : base(service)
    {
        _userManager = userManager;
    }

    public void LoadCheeps(int page, string author)
    {
        CurrentPage = page == 0 ? 1 : page;
        Cheeps = _service.GetCheepsFromAuthor(author, CurrentPage);
    }
    public ActionResult OnGet(string author, [FromQuery] int page)
    {
        CurrentPage = page == 0 ? 1 : page;
        Cheeps = _service.GetCheepsFromAuthor(author, page);
        if ((Cheeps.Count == 0 && CurrentPage != 1) || page < 0)
        {
            return RedirectToPage();
        }
        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        Author author = await _userManager.GetUserAsync(User);
        _service.PostCheep(author, Message);
        LoadCheeps(CurrentPage, author.Name);
        return Page();
    }
}
