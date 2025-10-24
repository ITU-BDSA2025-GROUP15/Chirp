using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using SQLitePCL;

using System.Linq;

/// <summary>
/// A simple data access abstraction for reading and writing cheeps.
/// </summary>
/// <remarks>
/// Student-friendly notes:
/// <list type="bullet">
///   <item><description>Call the repository from a service or page model when you need database data.</description></item>
///   <item><description>Most read methods return <see cref="CheepDTO"/> so the UI gets a ready-to-display object.</description></item>
///   <item><description>The default implementation uses Entity Framework Core and SQLite.</description></item>
/// </list>
/// Example: reading the first page of all cheeps
/// <code>
/// var page = await repository.ReadMessages(null, 1, 32);
/// foreach (var m in page) Console.WriteLine(m.Message);
/// </code>
/// </remarks>
public interface ICheepRepository
{
    /// <summary>
    /// Creates a new cheep using data from the provided <see cref="CheepDTO"/>.
    /// </summary>
    /// <param name="newMessage">The cheep data to persist.</param>
    /// <returns>A task that completes when the operation has finished.</returns>
    public Task CreateMessage(CheepDTO newMessage);

    /// <summary>
    /// Reads cheeps from the database with optional filtering and pagination.
    /// </summary>
    /// <param name="author">If specified, only cheeps by this author are returned; otherwise all authors.</param>
    /// <param name="page">Page number (1-based). If null the repository defaults to page 1.</param>
    /// <param name="limit">Maximum items per page. If null the repository uses a reasonable default.</param>
    /// <returns>A task that resolves to a list of <see cref="CheepDTO"/> representing the requested page.</returns>
    /// <remarks>
    /// <para>Student tips:</para>
    /// <list type="bullet">
    ///   <item><description>Pass <c>null</c> for <paramref name="author"/> to read all authors.</description></item>
    ///   <item><description>If <paramref name="page"/> is less than 1 the caller may receive an empty page—use 1-based pages.</description></item>
    ///   <item><description>Default limits are applied when <paramref name="limit"/> is null.</description></item>
    /// </list>
    /// </remarks>
    public Task<List<CheepDTO>> ReadMessages(string? author, int? page, int? limit);

    /// <summary>
    /// Updates an existing cheep using values from the supplied <see cref="CheepDTO"/>.
    /// </summary>
    /// <param name="alteredMessage">The updated cheep data.</param>
    /// <returns>A task that completes when the update has finished.</returns>
    public Task UpdateMessage(CheepDTO alteredMessage);
}

/// <summary>
/// Default implementation of <see cref="ICheepRepository"/> backed by Entity Framework Core.
/// </summary>
public class CheepRepository : ICheepRepository
{
    private readonly int defaultLimit = 32;
    private readonly ChirpDBContext _context;

    /// <summary>
    /// Initializes a new instance of <see cref="CheepRepository"/>.
    /// </summary>
    /// <param name="context">The EF Core DB context used for queries and persistence.</param>
    public CheepRepository(ChirpDBContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public Task CreateMessage(CheepDTO newMessage)
    {
        // Intentionally a no-op in this exercise/sample implementation.
        return Task.Run(() => 0); //does nothing
    }

    /// <summary>
    /// Reads messages from database.
    /// </summary>
    /// <param name="author">Author of messages. If null, all authors are returned.</param>
    /// <param name="page">Page number to read. If null, defaults to 1.</param>
    /// <param name="limit">Messages per page. If null, defaults to 32.</param>
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
    ///     <description>Order results by timestamp (descending), apply pagination and project into <see cref="CheepDTO"/>.</description>
    ///   </item>
    /// </list>
    /// <para>
    /// Default values are applied when <paramref name="page"/> or <paramref name="limit"/> are null: <c>page = 1</c> and
    /// <c>limit = 32</c> (the repository's defaultLimit).
    /// </para>
    /// </remarks>
    /// <exception cref="System.Exception">May throw an exception if the underlying database operation fails (EF Core / SQLite errors).</exception>
    /// <example>
    /// Simple call showing pagination:
    /// <code>
    /// var firstPage = await repository.ReadMessages(null, 1, 32);
    /// var adrianFirstPage = await repository.ReadMessages("Adrian", 1, 32);
    /// </code>
    /// </example>
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

    /// <inheritdoc />
    public Task UpdateMessage(CheepDTO alteredMessage)
    {
        // Intentionally a no-op in this exercise/sample implementation.
        return Task.Run(() => 0); //does nothing
    }

}