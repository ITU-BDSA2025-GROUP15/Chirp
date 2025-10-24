using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Represents a short user message (a "cheep").
/// </summary>
/// <remarks>
/// Each <see cref="Cheep"/> is persisted in the Cheeps table and is associated with an
/// <see cref="Author"/> via <see cref="AuthorId"/>. In this sample app cheeps are simple and
/// intentionally lack extra metadata (likes, replies, etc.).
/// </remarks>
/// <example>
/// Showing object construction (not persisted until saved via DbContext):
/// <code>
/// var c = new Cheep { Text = "Hello world", TimeStamp = DateTime.UtcNow, AuthorId = 1 };
/// db.Cheeps.Add(c);
/// db.SaveChanges();
/// </code>
/// </example>
public class Cheep
{
    /// <summary>
    /// Primary key for the cheep.
    /// </summary>
    /// <value>The unique integer identifier for the cheep.</value>
    public int CheepId { get; set; }

    /// <summary>
    /// The text body of the cheep.
    /// </summary>
    /// <value>A short string up to the application-implied length (no validation in this sample).</value>
    public string Text { get; set; }

    /// <summary>
    /// When the cheep was created.
    /// </summary>
    /// <value>A UTC <see cref="DateTime"/> representing the creation time.</value>
    public DateTime TimeStamp { get; set; }

    /// <summary>
    /// Foreign key pointing to the author who created the cheep.
    /// </summary>
    /// <remarks>Marked required because a cheep must always have an author in this model.</remarks>
    [Required]
    public required int AuthorId { get; set; }

    /// <summary>
    /// Navigation property for the cheep's author.
    /// </summary>
    /// <value>An <see cref="Author"/> instance when loaded; may be <c>null</c> if not loaded.</value>
    /// <remarks>
    /// When mapping to a DTO for the UI, prefer to project only the values you need (e.g. author's name)
    /// rather than passing the full entity graph.
    /// </remarks>
    public required Author Author { get; set; }
}