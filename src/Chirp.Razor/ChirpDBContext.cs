using Microsoft.EntityFrameworkCore;

/// <summary>
/// Entity Framework Core (EF Core) database context for the Chirp application.
/// </summary>
/// <remarks>
/// Provides access to the main entities through DbSet properties.
/// The context should be configured in Dependency Injection to use a SQLite database.
/// </remarks>
public class ChirpDBContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of <see cref="ChirpDBContext"/> with the provided options.
    /// </summary>
    /// <param name="options">The options to configure the context (provider, connection string, etc.).</param>
    public ChirpDBContext(DbContextOptions options) : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the collection of authors in the database.
    /// </summary>
    /// <value>The authors</value>
    /// <seealso cref="Author"/>
    public DbSet<Author> Authors { get; set; }

    /// <summary>
    /// Gets or sets the collection of cheeps in the database.
    /// </summary>
    /// <value>The cheeps</value>
    /// <seealso cref="Cheep"/>
    public DbSet<Cheep> Cheeps { get; set; }
}
