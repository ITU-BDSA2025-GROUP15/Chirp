using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PaginationModel
{
    public UserTimelineModel(ICheepService service) : base(service) {}

    public ActionResult OnGet(string author, [FromQuery] int page)
    {
        CurrentPage = page == 0 ? 1 : page;
        Cheeps = _service.GetCheepsFromAuthor(author, page);
        if ((Cheeps.Count == 0 && CurrentPage != 1)|| page < 0)
        {
            return RedirectToPage();
        }
        return Page();
    }
}
