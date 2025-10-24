using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents a person who can post cheeps (short messages) in the application.
/// </summary>
/// <remarks>
/// <para>
/// Each <see cref="Author"/> maps to a row in the database Authors table. Students: think of this
/// as the "user" who writes cheeps. The class is intentionally small so the sample app stays
/// focused on the data flow (entity &rarr; repository &rarr; service &rarr; UI).
/// </para>
/// <para>
/// The <see cref="Cheeps"/> navigation property may be <c>null</c> if EF Core did not load it.
/// Use eager loading (Include) or explicit loading in production code when you need related data.
/// </para>
/// </remarks>
/// <example>
/// Example: creating an author in code (not persisted until saved via the DbContext):
/// <code>
/// var a = new Author { Name = "Ada", Email = "ada@example.org" };
/// db.Authors.Add(a);
/// db.SaveChanges();
/// </code>
/// </example>
public class Author
{
    /// <summary>
    /// Primary key for this author.
    /// </summary>
    /// <value>An integer that uniquely identifies the author in the database.</value>
    public int AuthorId { get; set; }

    /// <summary>
    /// The display name for the author.
    /// </summary>
    /// <value>A short name shown in the UI and used in URLs and filters.</value>
    /// <remarks>
    /// Keep names short and URL-friendly in this sample app; in a real system you'd add validation
    /// and possibly a separate username field.
    /// </remarks>
    public string Name { get; set; }

    /// <summary>
    /// Contact email for the author.
    /// </summary>
    /// <value>The author's email address, or <c>null</c> for seeded/test accounts.</value>
    public string? Email { get; set; }

    /// <summary>
    /// Navigation property containing the cheeps posted by this author.
    /// </summary>
    /// <value>A list of <see cref="Cheep"/> instances or <c>null</c> if not loaded by EF.</value>
    /// <remarks>
    /// Use <c>Include(a =&gt; a.Cheeps)</c> when querying authors if you need their messages.
    /// </remarks>
    public List<Cheep>? Cheeps { get; set; }
}