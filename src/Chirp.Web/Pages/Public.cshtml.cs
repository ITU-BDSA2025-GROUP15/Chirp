using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PaginationModel
{
    private readonly UserManager<Author> _userManager;
    [BindProperty]
    [Required]
    [StringLength(160, ErrorMessage = "Maximum length is {1}")]
    public string Message { get; set; }
    public Author CurrentAuthor { get; set; }

    public PublicModel(ICheepService service, UserManager<Author> userManager) : base(service)
    {
        _userManager = userManager;
    }

    private void LoadCheeps(int page)
    {
        CurrentPage = page == 0 ? 1 : page;
        Cheeps = _service.GetCheeps(CurrentPage);
    }
    public ActionResult OnGet(string author, [FromQuery] int page)
    {
        CurrentPage = page == 0 ? 1 : page;
        if(author != null)
        {
            Cheeps = _service.GetCheepsFromAuthor(author, page);
        } else
        {
            Cheeps = _service.GetCheeps(page);
        }
        if ((Cheeps.Count == 0 && CurrentPage != 1) || page < 0)
        {
            return RedirectToPage();
        }
        return Page();
    }
    public async Task<IActionResult> OnPostAsync()
    {
        var userAuthor = await _userManager.GetUserAsync(User);
        _service.PostCheep(userAuthor, Message);
        LoadCheeps(CurrentPage);
        return Redirect("/" + userAuthor.Name);
    }
}