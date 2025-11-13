using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Razor.Pages;

public class PublicModel(ICheepService service, UserManager<Author> userManager) : PaginationModel(service)
{
    private const int cheepLength = 160;
    private readonly UserManager<Author> _userManager = userManager;
    [BindProperty]
    [Required]
    [StringLength(cheepLength, ErrorMessage = "Maximum length is {1}")]
    public required string Message { get; set; }

    public ActionResult OnGet(string author, [FromQuery] int page)
    {
        Cheeps = LoadCheeps(author, page);
        if ((Cheeps.Count == 0 && CurrentPage != 1) || page < 0)
        {
            return RedirectToPage();
        }
        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        var userAuthor = await _userManager.GetUserAsync(User);
        if (userAuthor == null) //Should not happen and probably won't
        {
            var routeName = RouteData.Values["author"]?.ToString();
            LoadCheeps(routeName!, CurrentPage);
            ViewData["Error"] = "Account not found";
            return Page();
        }
        _service.PostCheep(userAuthor!, Message);
        return Redirect("/" + userAuthor!.Name ?? "NameNotFound");
    }
    public List<CheepDTO> LoadCheeps(string author, int page)
    {
        CurrentPage = page == 0 ? 1 : page;
        if (author != null)
        {
            Cheeps = _service.GetCheepsFromAuthor(author, page);
        }
        else
        {
            Cheeps = _service.GetCheeps(page);
        }
        return Cheeps;
    }
}