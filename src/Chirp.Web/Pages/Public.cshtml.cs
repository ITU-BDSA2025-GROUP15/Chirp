using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chirp.Razor.Pages;

public class PublicModel(ICheepService cheepService, IAuthorService authorService, UserManager<Author> userManager) : PaginationModel(cheepService, authorService)
{
    private const int cheepLength = 160;
    private readonly UserManager<Author> _userManager = userManager;
    [BindProperty]
    [Required]
    [StringLength(cheepLength, ErrorMessage = "Maximum length is {1}")]
    public required string Message { get; set; }

    public async Task<ActionResult> OnGet(string author, [FromQuery] string page)
    {
        var author2 = await _userManager.GetUserAsync(User);
        int _page = 1;
        if (page != null)
        {
            try { _page = int.Parse(page); }
            catch (Exception) { return RedirectToPage(); }

            if (_page <= 0) return RedirectToPage();
        }

        if (author2 != null && author != null && author.Equals(author2.Name, StringComparison.OrdinalIgnoreCase))
        {
            Cheeps = await LoadCheepsMyTimeline(author, _page);
        } else
        {
            Cheeps = LoadCheeps(author!, _page);
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

        _cheepservice.PostCheep(author, Message);
        string authorUrl = Uri.EscapeDataString(author.Name);
        return Redirect("/" + (authorUrl ?? "NameNotFound"));
    }
    public async Task<IActionResult> OnPostFollowAsync()
    {
        var author = await _userManager.GetUserAsync(User);
        // converting the Author to an AuthorDTO for some reason
        var authorDTO = await _authorservice.GetAuthorByName(author!.Name);
        var idol   = await _authorservice.GetAuthorByName(RouteData.Values["author"]!.ToString()!);

        if (!await _authorservice.IsAuthorFollowingAuthor(authorDTO,idol))
        {
            await _authorservice.FollowAuthor(authorDTO,idol);
        } else
        {
            await _authorservice.UnFollowAuthor(authorDTO,idol);
        }
        string authorUrl = Uri.EscapeDataString(idol.Name);
        return Redirect("/" + authorUrl ?? "NameNotFound");
    }
    public async Task<List<CheepDTO>> LoadCheepsMyTimeline(string author, int page)
    {
        CurrentPage = page == 0 ? 1 : page;
        var authorAndFollowing = await _authorservice.GetFollowingByName(author);
        authorAndFollowing = [.. authorAndFollowing, author];
        if (author != null)
        {
            Cheeps = _cheepservice.GetCheepsFromAuthors(authorAndFollowing, page);
        }
        else
        {
            Cheeps = _cheepservice.GetCheeps(page);
        }
        return Cheeps;
    }
    public List<CheepDTO> LoadCheeps(string author, int page)
    {
        CurrentPage = page == 0 ? 1 : page;
        if (author != null)
        {
            Cheeps = _cheepservice.GetCheepsFromAuthor(author, page);
        }
        else
        {
            Cheeps = _cheepservice.GetCheeps(page);
        }
        return Cheeps;
    }
}