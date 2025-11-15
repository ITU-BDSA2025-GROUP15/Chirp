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

    public ActionResult OnGet(string author, [FromQuery] string page)
    {
        int _page = 1;
        if (page != null)
        {
            try { _page = int.Parse(page); }
            catch (Exception) { return RedirectToPage(); }

            if (_page <= 0) return RedirectToPage();
        }

        Cheeps = LoadCheeps(author, _page);
        if (Cheeps.Count == 0 && CurrentPage != 1) { return RedirectToPage(); }
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