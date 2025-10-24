using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepDTO> Cheeps { get; set; }

    public PublicModel(ICheepService service)
    {
        _service = service;
    }

<<<<<<< Updated upstream
=======
    /// <summary>
    /// Handles GET requests for the public timeline. Accepts an optional page query parameter.
    /// </summary>
    /// <param name="page">The page number to get page number to display (starting from 1).</param>
    /// <returns>A <see cref="PageResult"/> representing the rendered page.</returns>
    /// <example>
    /// Request examples:
    /// <code>
    /// // Default page (same as page=1)
    /// GET /
    /// // Explicit page
    /// GET /?page=2
    /// </code>
    /// </example>
>>>>>>> Stashed changes
    public ActionResult OnGet([FromQuery] int page)
    {
        Cheeps = _service.GetCheeps(page);
        return Page();
    }
}