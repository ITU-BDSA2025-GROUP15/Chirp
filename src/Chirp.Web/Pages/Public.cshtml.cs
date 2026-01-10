using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using SQLitePCL;

namespace Chirp.Razor.Pages;

public class PublicModel(ICheepService cheepService, IAuthorService authorService, UserManager<Author> userManager) : PaginationModel(cheepService, authorService)
{
    private const int cheepLength = 160;
    private readonly UserManager<Author> _userManager = userManager;
    [BindProperty]
    [Required]
    [StringLength(cheepLength, ErrorMessage = "Maximum length is {1}")]
    public required string Message { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Sorting { get; set; }

    public async Task<IActionResult> OnGet(string author, [FromQuery] string page)
    {
        var authorObj = await _userManager.GetUserAsync(User);
        if (string.IsNullOrEmpty(Sorting))
            Sorting = "Newest"; // default
        
        if (page?.ToLower() == "wii")
    {
        page = null;
    }
        int _page = 1;
        if (page != null)
        {
            try { _page = int.Parse(page); }
            catch (Exception) { return RedirectToPage(); }

            if (_page <= 0) return RedirectToPage();
        }

        if (authorObj != null && author != null && author.Equals(authorObj.Name, StringComparison.OrdinalIgnoreCase))
        {
            Cheeps = await LoadCheepsMyTimeline(author, _page, Sorting);
        }
        else
        {
            Cheeps = LoadCheeps(author!, _page, Sorting);
        }
        if (authorObj != null)
        {
            foreach (var cheep in Cheeps)
            {
                cheep.UserHasLiked = await _cheepservice.HasUserLiked(authorObj.Id, cheep.CheepId);
            }
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
            LoadCheeps(routeName!, CurrentPage, "Newest");
            ViewData["Error"] = "Account not found";
            return Page();
        }

        _cheepservice.PostCheep(author, Message);
        string authorUrl = Uri.EscapeDataString(author.Name);
        return Redirect("/" + (authorUrl ?? "NameNotFound"));
    }
    public async Task<IActionResult> OnPostFollowAsync([FromQuery] string? user)
    {
        var author = await _userManager.GetUserAsync(User);
        // converting the Author to an AuthorDTO for some reason
        var authorDTO = await _authorservice.GetAuthorByName(author!.Name);
        AuthorDTO idol = await _authorservice.GetAuthorByName(RouteData.Values["author"]!.ToString()!);

        if (!await _authorservice.IsAuthorFollowingAuthor(authorDTO, idol))
        {
            await _authorservice.FollowAuthor(authorDTO, idol);
        }
        else
        {
            await _authorservice.UnFollowAuthor(authorDTO, idol);
        }
        string authorUrl = Uri.EscapeDataString(idol.Name);
        return Redirect("/" + authorUrl ?? "NameNotFound");
    }

    public async Task<List<CheepDTO>> LoadCheepsMyTimeline(string author, int page, string sorting)
    {
        CurrentPage = page == 0 ? 1 : page;
        var authorAndFollowing = await _authorservice.GetFollowingByName(author);
        authorAndFollowing = [.. authorAndFollowing, author];
        if (author != null)
        {
            Cheeps = _cheepservice.GetCheepsFromAuthors(authorAndFollowing, page, sorting);
        }
        else
        {
            Cheeps = _cheepservice.GetCheeps(page, sorting);
        }
        return Cheeps;
    }
    public async Task<IActionResult> OnPostLike(int id, bool json)
    {
        Author author = (await _userManager.GetUserAsync(User))!;
        var updatedCount = await _cheepservice.Likes(author.Id, id);
        bool hasLiked = await _cheepservice.HasUserLiked(author.Id, id);

        if (json) return new JsonResult(new { hasLiked = hasLiked, likeCount = updatedCount });
        else return RedirectToPage();
    }

    public List<CheepDTO> LoadCheeps(string author, int page, string? sorting)
    {
        CurrentPage = page == 0 ? 1 : page;
        if (author != null)
        {
            Cheeps = _cheepservice.GetCheepsFromAuthor(author, page, sorting!);
        }
        else
        {
            Cheeps = _cheepservice.GetCheeps(page, sorting!);
        }
        return Cheeps;
    }

    public async Task<IActionResult> OnPostToggleFollowAsync(string idol, string? sorting)
    {
        var author = await _userManager.GetUserAsync(User);
        if (author == null)
            return RedirectToPage("/Login");

        var authorDTO = await _authorservice.GetAuthorByName(author.Name);
        var idolDTO = await _authorservice.GetAuthorByName(idol);

        if (idolDTO == null)
            return RedirectToPage(); // user not found

        var isFollowing = await IsFollowing(authorDTO.Name, idolDTO.Name);

        if (isFollowing)
        {
            await _authorservice.UnFollowAuthor(authorDTO, idolDTO);
        }
        else
        {
            await _authorservice.FollowAuthor(authorDTO, idolDTO);
        }

        if (sorting != null) return RedirectToPage("", new { sorting });
        else return RedirectToPage();
    }

    public async Task<bool> IsFollowing(string author, string idol)
    {
        var authorDTO = await _authorservice.GetAuthorByName(author);
        var idolDTO = await _authorservice.GetAuthorByName(idol);

        return await _authorservice.IsAuthorFollowingAuthor(authorDTO, idolDTO);
    }
}