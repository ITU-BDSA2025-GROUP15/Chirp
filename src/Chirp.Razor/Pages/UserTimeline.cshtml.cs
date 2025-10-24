using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

/// <summary>
/// Page model for showing the timeline of a single user (author).
/// </summary>
public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;

    /// <summary>
    /// Gets or sets the cheeps to display for the requested user.
    /// </summary>
    public List<CheepDTO> Cheeps { get; set; }

    /// <summary>
    /// Initializes a new instance of <see cref="UserTimelineModel"/>.
    /// </summary>
    /// <param name="service">Service used to retrieve cheeps.</param>
    public UserTimelineModel(ICheepService service)
    {
        _service = service;
    }

    /// <summary>
    /// Handles GET requests to the user timeline. The <paramref name="author"/> parameter
    /// identifies which user's cheeps to show.
    /// </summary>
    /// <param name="author">Author display name whose cheeps will be shown.</param>
    /// <param name="page">The page number to display  (starting from 1) (optional via query string).</param>
    /// <returns>A <see cref="PageResult"/> representing the rendered page.</returns>
    /// <example>
    /// URL examples:
    /// <code>
    /// // Show Adrian's timeline default page
    /// GET /Adrian
    /// // Show Adrian's page 2
    /// GET /Adrian?page=2
    /// </code>
    /// </example>
    public ActionResult OnGet(string author, [FromQuery] int page)
    {
        Cheeps = _service.GetCheepsFromAuthor(author, page);