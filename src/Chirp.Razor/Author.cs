using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents an author of cheeps in the system.
/// </summary>
/// <remarks>
/// Instances of this class map to the Authors table in the database.
/// The <see cref="Cheeps"/> collection contains cheeps authored by this user.
/// </remarks>
public class Author
{
    /// <summary>
    /// Gets or sets the primary key for the author.
    /// </summary>
    /// <value>An integer that uniquely identifies the author in the database.</value>
    public int AuthorId { get; set; }

    /// <summary>
    /// Gets or sets the display name of the author.
    /// </summary>
    /// <value>The author display name used throughout the UI and in URLs.</value>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the email address for the author.
    /// </summary>
    /// <value>
    /// The author's email address. This may be <c>null</c> for test data.
    /// </value>
    public string? Email { get; set; }

    /// <summary>
    /// Gets or sets the collection of <see cref="Cheep"/> entities posted by this author.
    /// May be null when not loaded from the database.
    /// </summary>
    /// <value>A list of <see cref="Cheep"/> instances or <c>null</c> if not loaded by EF.</value>
    public List<Cheep>? Cheeps { get; set; }
}