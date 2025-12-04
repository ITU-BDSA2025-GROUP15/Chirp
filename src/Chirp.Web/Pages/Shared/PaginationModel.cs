using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PaginationModel : PageModel
{
    protected readonly ICheepService _cheepservice;
    protected readonly IAuthorService _authorservice;
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
            return CurrentPage + 1;
        }
    }
    protected readonly static int defaultPageLimit = 32;
    public bool HasNextPage
    {
        get
        {
            return Cheeps.Count == defaultPageLimit;
        }
    }
    public bool HasPreviousPage
    {
        get
        {
            return CurrentPage != 1;
        }
    }

    public PaginationModel(ICheepService cheepService, IAuthorService authorService)
    {
        _cheepservice = cheepService;
        _authorservice = authorService;
    }
}