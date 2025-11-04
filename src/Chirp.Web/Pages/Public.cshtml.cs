using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    public required List<CheepDTO> Cheeps { get; set; }


    public int CurrentPage { get; set; }

    public int PreviousPage
    {
        get
        {
            return CurrentPage - 1;
        }
    }
    public int NextPage
    {
        get
        {
            if (!HasNextPage)
            {
                return CurrentPage;
            }
            else
            {
            return CurrentPage + 1;
            }
        }
    }
    private readonly static int defaultPageLimit = 32;
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }

    public PublicModel(ICheepService service)
    {
        _service = service;
    }

    public ActionResult OnGet([FromQuery] int page)
    {
        CurrentPage = page == 0 ? 1 : page;
        Cheeps = _service.GetCheeps(page);
        HasNextPage = Cheeps.Count == defaultPageLimit ? true : false;
        HasPreviousPage = CurrentPage != 1;
        if (Cheeps.Count == 0)
        {
            return RedirectToPage("/Public");
        }
        return Page();
    }
}