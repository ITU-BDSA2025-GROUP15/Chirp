using Chirp.Razor;

//public record CheepViewModel(string Author, string Message, string Timestamp);

<<<<<<< Updated upstream
=======
/// <summary>
/// A service-layer abstraction that handles reading cheeps for the app.
/// </summary>
>>>>>>> Stashed changes
public interface ICheepService
{
    public List<CheepDTO> GetCheeps();
<<<<<<< Updated upstream
=======

    /// <summary>
    /// Gets a specific page of cheeps.
    /// </summary>
    /// <param name="page">The page number to get (starting from 1).</param>
    /// <returns>A list of <see cref="CheepDTO"/> for the requested page.</returns>
>>>>>>> Stashed changes
    public List<CheepDTO> GetCheeps(int page);
    public List<CheepDTO> GetCheepsFromAuthor(string author);
<<<<<<< Updated upstream
    public List<CheepDTO> GetCheepsFromAuthor(string author, int page);
}

=======

    //why is GetCheepsFromAuthor here twice. there must be a smarter way to do this. shouldnt it just defoult to page 1 if no page is given?

    /// <summary>
    /// Gets a specific page of cheeps for a specific author.
    /// </summary>
    /// <param name="author">The author's display name.</param>
    /// <param name="page">The page number to get (starting from 1).</param>
    /// <returns>A list of <see cref="CheepDTO"/> authored by <paramref name="author"/> for the requested page.</returns>
    public List<CheepDTO> GetCheepsFromAuthor(string author, int page);
}

/// <summary>
/// Implementation of <see cref="ICheepService"/> which delegates to a repository.
/// </summary>
>>>>>>> Stashed changes
public class CheepService : ICheepService
{
    private readonly ICheepRepository _repository;

<<<<<<< Updated upstream
=======
    /// <summary>
    /// Initializes a new instance of <see cref="CheepService"/>.
    /// </summary>
    /// <param name="repository">Repository used to fetch cheep data.</param>
>>>>>>> Stashed changes
    public CheepService(ICheepRepository repository)
    {
        _repository = repository;
    }
    public List<CheepDTO> GetCheeps()
    {
        var messages = _repository.ReadMessages(null, null, null);
        return messages.GetAwaiter().GetResult();
    }

    public List<CheepDTO> GetCheeps(int page)
    {
        var messages = _repository.ReadMessages(null, page, null);
        return messages.GetAwaiter().GetResult();
    }

    public List<CheepDTO> GetCheepsFromAuthor(string author)
    {
        var messages = _repository.ReadMessages(author, null, null);
        return messages.GetAwaiter().GetResult();
    }

    public List<CheepDTO> GetCheepsFromAuthor(string author, int page)
    {
        var messages = _repository.ReadMessages(author, page, null);
        return messages.GetAwaiter().GetResult();
    }

    /// <summary>
<<<<<<< Updated upstream
    /// Converts a list of Cheeps into a list of corresponding CheepDTOs.
    /// </summary>
=======
    /// Converts a list of <see cref="Cheep"/>'s instances into a list of <see cref="CheepDTO"/>'s.
    /// </summary>
    /// <param name="cheeps">The list of cheep entities to convert.</param>
    /// <returns>A list of <see cref="CheepDTO"/></returns>
    /// <remarks>
    /// <para>
    /// It expects each <paramref name="cheeps"/> item to have a populated <c>Author</c>.
    /// </para>
    /// </remarks>
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
    /// Converts a DateTime object into a formatted string, in the format MM/dd/yy H:mm:ss.
    /// </summary>
    /// <returns>A formatted date/time string, i.e. 12/31/25 9:41:23.</returns>
=======
    /// Converts <see cref="DateTime"/> into a formatted string using the pattern "MM/dd/yy H:mm:ss".
    /// </summary>
    /// <param name="dateTime">Date/time value to format.</param>
    /// <returns>A formatted date/time string, FX "12/13/19 9:41:23".</returns>
    /// <example>
    /// Example usage:
    /// <code>
    /// var now = DateTime.UtcNow;
    /// var formatted = CheepService.DateTimeToDateTimeString(now);
    /// </code>
    /// </example>
>>>>>>> Stashed changes
    public static string DateTimeToDateTimeString(DateTime dateTime)
    {
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
}
