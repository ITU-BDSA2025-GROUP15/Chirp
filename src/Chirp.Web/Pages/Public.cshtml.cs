using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PaginationModel
{
    [BindProperty]
    public string Message { get; set; }

    public static Author CurrentAuthor;

    public PublicModel(ICheepService service) : base(service)
    {
    }

    private void LoadCheeps(int page)
    {
        CurrentPage = page == 0 ? 1 : page;
        Cheeps = _service.GetCheeps(CurrentPage);
    }
    public ActionResult OnGet([FromQuery] int page)
    {
        CurrentPage = page == 0 ? 1 : page;
        Cheeps = _service.GetCheeps(page);
        if ((Cheeps.Count == 0 && CurrentPage != 1) || page < 0)
        {
            return RedirectToPage();
        }
        return Page();
    }
    public void OnPost()
    {
        _service.PostCheep(CurrentAuthor, Message);
        LoadCheeps(CurrentPage);
    }
}