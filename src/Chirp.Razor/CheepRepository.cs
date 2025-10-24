using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using SQLitePCL;

using System.Linq;

<<<<<<< Updated upstream
public interface ICheepRepository
{
    public Task CreateMessage(CheepDTO newMessage);
    public Task<List<CheepDTO>> ReadMessages(string? author, int? page, int? limit);
    public Task UpdateMessage(CheepDTO alteredMessage);
}

=======
/// <summary>
/// Repository abstraction for storing and retrieving cheeps.
/// </summary>
/// <remarks>
/// Implementations are responsible for interacting with the underlying database and returning
/// data in the <see cref="CheepDTO"/> shape used by services and UI.
/// </remarks>
public interface ICheepRepository
{
    /// <summary>
    /// Creates a new cheep using data from the provided <see cref="CheepDTO"/>.
    /// </summary>
    /// <param name="newMessage">The cheep data </param>
    /// <returns>A task that completes when the operation has finished.</returns>
    public Task CreateMessage(CheepDTO newMessage);

    /// <summary>
    /// Reads cheeps from the database with optional filtering and Pagelisting.
    /// </summary>
    /// <param name="author">If specified, only cheeps by this author are returned; otherwise all authors.</param>
    /// <param name="page">Page number (1-based). If null the repository may default to page 1.</param>
    /// <param name="limit">Maximum cheeps per page.</param>
    /// <returns>A task that resolves to a list of <see cref="CheepDTO"/> representing the requested page.</returns>
    public Task<List<CheepDTO>> ReadMessages(string? author, int? page, int? limit);

    /// <summary>
    /// Updates an existing cheep using values from <see cref="CheepDTO"/>.
    /// </summary>
    /// <param name="alteredMessage">The updated cheep data.</param>
    /// <returns>A task that completes when the update has finished.</returns>
    public Task UpdateMessage(CheepDTO alteredMessage);
}

/// <summary>
/// Implementation of <see cref="ICheepRepository"/> backed by EF Core.
/// </summary>
>>>>>>> Stashed changes
public class CheepRepository : ICheepRepository
{
    private readonly int defaultLimit = 32;
    private readonly ChirpDBContext _context;

<<<<<<< Updated upstream
=======
    /// <summary>
    /// Initializes a new instance of <see cref="CheepRepository"/>.
    /// </summary>
    /// <param name="context">The EF Core DB context used for queries</param>
>>>>>>> Stashed changes
    public CheepRepository(ChirpDBContext context)
    {
        _context = context;
    }

    public Task CreateMessage(CheepDTO newMessage)
    {
        return Task.Run(() => 0); //does nothing
    }

    /// <summary>
    /// Reads messages from database.
    /// </summary>
    /// <param name="author">Author of messages. If null, all authors are returned.</param>
    /// <param name="page">Page number to read. If null, defaults to 1.</param>
    /// <param name="limit">Messages per page. If null, defaults to 32.</param>
<<<<<<< Updated upstream
    /// <returns>List of messages converted to CheepDTO.</returns>
=======
    /// <returns>List of messages converted to <see cref="CheepDTO"/>.</returns>
    /// <remarks>
    /// <para>
    /// The method executes these steps:
    /// </para>
    /// <list type="bullet">
    ///   <item>
    ///     <description>Join the <c>Cheeps</c> and <c>Authors</c> tables to obtain the author's name.</description>
    ///   </item>
    ///   <item>
    ///     <description>Optionally filter by <paramref name="author"/>.</description>
    ///   </item>
    ///   <item>
    ///     <description>Order results by timestamp (descending), Split the results into pages and convert them to <see cref="CheepDTO"/>.</description>
    ///   </item>
    /// </list>
    /// </remarks>
    /// <example>
    /// Simple call splitting by page and limit:
    /// <code>
    /// var firstPage = await repository.ReadMessages(null, 1, 32);
    /// var adrianFirstPage = await repository.ReadMessages("Adrian", 1, 32);
    /// </code>
    /// </example>
>>>>>>> Stashed changes
    public async Task<List<CheepDTO>> ReadMessages(string? author, int? page, int? limit)
    {
        var query = _context.Cheeps
            .Join(_context.Authors,
                Cheeps => Cheeps.AuthorId,
                Authors => Authors.AuthorId,
                (Cheeps, Authors) => new
                {
                    Author = Authors.Name,
                    Message = Cheeps.Text,
                    Timestamp = Cheeps.TimeStamp
                });
        if (author != null)
        {
            query = query.Where(Cheep => Cheep.Author == author);
        }

        query = query.OrderByDescending(Cheep => Cheep.Timestamp);

        // Use default values for limit/page if any null
        int _limit = limit ?? defaultLimit;
        int _page = page ?? 1;

        query = query.Skip((_page - 1) * _limit);
        query = query.Take(_limit);
    
        var cheeps = await query.ToListAsync();
        var cheepdtos = new List<CheepDTO>();

        foreach (var cheep in cheeps)
        {
            cheepdtos.Add(new CheepDTO
            {
                Author = cheep.Author,
                Message = cheep.Message,
                Timestamp = CheepService.DateTimeToDateTimeString(cheep.Timestamp)
            });
        }

        return cheepdtos;
    }
    public Task UpdateMessage(CheepDTO alteredMessage)
    {
        return Task.Run(() => 0); //does nothing
    }

}