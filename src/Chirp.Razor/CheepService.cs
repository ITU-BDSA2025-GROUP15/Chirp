using Chirp.Razor;

//public record CheepViewModel(string Author, string Message, string Timestamp);

/// <summary>
/// Service layer that provides easy, UI-friendly access to cheeps.
/// </summary>
/// <remarks>
/// Student note: this service wraps repository calls and converts domain entities into
/// <see cref="CheepDTO"/> objects that are convenient to use in Razor pages.
/// The public methods are synchronous (they block) for simplicity; in production code
/// prefer async all the way through to avoid blocking threads.
/// </remarks>
public interface ICheepService
{
    /// <summary>
    /// Gets the default page of cheeps.
    /// </summary>
    /// <returns>A list of <see cref="CheepDTO"/> for the default page.</returns>
    public List<CheepDTO> GetCheeps();

    /// <summary>
    /// Gets a specific page of cheeps.
    /// </summary>
    /// <param name="page">The 1-based page number to retrieve.</param>
    /// <returns>A list of <see cref="CheepDTO"/> for the requested page.</returns>
    public List<CheepDTO> GetCheeps(int page);

    /// <summary>
    /// Gets the default page of cheeps for a specific author.
    /// </summary>
    /// <param name="author">The author's display name.</param>
    /// <returns>A list of <see cref="CheepDTO"/> authored by <paramref name="author"/>.</returns>
    public List<CheepDTO> GetCheepsFromAuthor(string author);

    /// <summary>
    /// Gets a specific page of cheeps for a specific author.
    /// </summary>
    /// <param name="author">The author's display name.</param>
    /// <param name="page">The 1-based page number to retrieve.</param>
    /// <returns>A list of <see cref="CheepDTO"/> authored by <paramref name="author"/> for the requested page.</returns>
    public List<CheepDTO> GetCheepsFromAuthor(string author, int page);
}

/// <summary>
/// Concrete implementation of <see cref="ICheepService"/> which delegates to a repository.
/// </summary>
public class CheepService : ICheepService
{
    private readonly ICheepRepository _repository;

    /// <summary>
    /// Initializes a new instance of <see cref="CheepService"/>.
    /// </summary>
    /// <param name="repository">Repository used to fetch cheep data. Injected by DI in the app.</param>
    public CheepService(ICheepRepository repository)
    {
        _repository = repository;
    }

    /// <inheritdoc />
    public List<CheepDTO> GetCheeps()
    {
        var messages = _repository.ReadMessages(null, null, null);
        return messages.GetAwaiter().GetResult();
    }

    /// <inheritdoc />
    public List<CheepDTO> GetCheeps(int page)
    {
        var messages = _repository.ReadMessages(null, page, null);
        return messages.GetAwaiter().GetResult();
    }

    /// <inheritdoc />
    public List<CheepDTO> GetCheepsFromAuthor(string author)
    {
        var messages = _repository.ReadMessages(author, null, null);
        return messages.GetAwaiter().GetResult();
    }

    /// <inheritdoc />
    public List<CheepDTO> GetCheepsFromAuthor(string author, int page)
    {
        var messages = _repository.ReadMessages(author, page, null);
        return messages.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Converts a list of <see cref="Cheep"/> instances into a list of corresponding <see cref="CheepDTO"/>.
    /// </summary>
    /// <param name="cheeps">The list of cheep entities to convert.</param>
    /// <returns>A list of <see cref="CheepDTO"/> ready for presentation.</returns>
    /// <exception cref="System.NullReferenceException">Thrown if any <paramref name="cheeps"/> element has a null <c>Author</c> property.</exception>
    /// <remarks>
    /// <para>
    /// This projects domain entities into DTOs. Each <paramref name="cheeps"/> item should have
    /// a populated <c>Author</c> navigation property. If an <c>Author</c> is missing a
    /// <see cref="System.NullReferenceException"/> will be thrown — this keeps the sample simple.
    /// </para>
    /// <para>
    /// Tip for students: use repository-level projection (select into DTOs in the database query)
    /// for large result sets to improve performance and avoid loading extra data.
    /// </para>
    /// </remarks>
    public static List<CheepDTO> CheepListToCheepDTOList(List<Cheep> cheeps)
    {
        var modelMessages = new List<CheepDTO>();

        foreach (var cheep in cheeps)
        {
            var modelCheep = new CheepDTO()
            {
                Author = cheep.Author.Name,
                Message = cheep.Text,
                Timestamp = DateTimeToDateTimeString(cheep.TimeStamp)
            };
            modelMessages.Add(modelCheep);
        }

        return modelMessages;
    }

    /// <summary>
    /// Converts a <see cref="DateTime"/> into a formatted string using the pattern "MM/dd/yy H:mm:ss".
    /// </summary>
    /// <param name="dateTime">Date/time value to format.</param>
    /// <returns>A formatted date/time string, e.g. "12/31/25 9:41:23".</returns>
    /// <example>
    /// Example usage:
    /// <code>
    /// var now = DateTime.UtcNow;
    /// var formatted = CheepService.DateTimeToDateTimeString(now);
    /// // formatted -> "10/24/25 14:05:07"
    /// </code>
    /// </example>
    public static string DateTimeToDateTimeString(DateTime dateTime)
    {
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

    /// <summary>
    /// Student example: how to call the service from a page model.
    /// </summary>
    /// <example>
    /// <code>
    /// // injected ICheepService _cheepService
    /// var firstPage = _cheepService.GetCheeps();
    /// foreach (var row in firstPage) Console.WriteLine(row.Message);
    /// </code>
    /// </example>
}
