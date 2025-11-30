using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using SQLitePCL;

namespace Chirp.Razor.Pages;

public class PublicModel(ICheepService service, UserManager<Author> userManager) : PaginationModel(service)
{
    private const int cheepLength = 160;
    private readonly UserManager<Author> _userManager = userManager;
    [BindProperty]
    [Required]
    [StringLength(cheepLength, ErrorMessage = "Maximum length is {1}")]
    public required string Message { get; set; }

    public async Task<IActionResult> OnGet(string authorName, [FromQuery] string page)
    {
        var author = await _userManager.GetUserAsync(User);
        int _page = 1;
        if (page != null)
        {
            try { _page = int.Parse(page); }
            catch (Exception) { return RedirectToPage(); }

            if (_page <= 0) return RedirectToPage();
        }

        Cheeps = LoadCheeps(authorName, _page);
        foreach (var cheep in Cheeps)
        {
            cheep.UserHasLiked = await _service.HasUserLiked(author.Id, cheep.CheepId);
        }
        if (Cheeps.Count == 0 && CurrentPage != 1) { return RedirectToPage(); }
        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        var author = await _userManager.GetUserAsync(User);
        if (author == null) // Should not happen and probably won't
        {
            var routeName = RouteData.Values["author"]?.ToString();
            LoadCheeps(routeName!, CurrentPage);
            ViewData["Error"] = "Account not found";
            return Page();
        }

        _service.PostCheep(author, Message);
        string authorUrl = Uri.EscapeDataString(author.Name);
        return Redirect("/" + authorUrl ?? "NameNotFound");
    }
    public async Task<IActionResult> OnPostLike(int id)
    {
        var author = await _userManager.GetUserAsync(User);
        var updatedCount = await _service.Likes(author.Id, id, true);

        return new JsonResult(new { likeCount = updatedCount });
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