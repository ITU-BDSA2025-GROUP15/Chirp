using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

/// <summary>
/// Page model for the public timeline page which shows paginated cheeps.
/// </summary>
public class PublicModel : PageModel
{
    private readonly ICheepService _service;

    /// <summary>
    /// Gets or sets the list of cheeps to display on the page.
    /// </summary>
    public List<CheepDTO> Cheeps { get; set; }

    /// <summary>
    /// Initializes a new instance of <see cref="PublicModel"/>.
    /// </summary>
    /// <param name="service">Service used to retrieve cheeps.</param>
    public PublicModel(ICheepService service)
    {
        _service = service;
    }

    /// <summary>
    /// Handles GET requests for the public timeline. Accepts an optional page query parameter.
    /// </summary>
    /// <param name="page">1-based page number to display.</param>
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
    public ActionResult OnGet([FromQuery] int page)
    {
        Cheeps = _service.GetCheeps(page);
        return Page();
    }
}