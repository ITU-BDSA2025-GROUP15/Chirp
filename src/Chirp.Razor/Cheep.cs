using System;
using System.ComponentModel.DataAnnotations;

<<<<<<< Updated upstream
public class Cheep
{
    public int CheepId { get; set; }
    public string Text { get; set; }
    public DateTime TimeStamp { get; set; }
    [Required]
    public required int AuthorId { get; set; }
=======
/// <summary>
/// Represents a short user message (a "cheep").
/// </summary>
/// <remarks>
/// Instances map to the Cheeps table in the database and are associated with an <see cref="Author"/>.
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
    /// Gets or sets the primary key for the cheep.
    /// </summary>
    /// <value>The unique integer identifier for the cheep.</value>
    public int CheepId { get; set; }

    /// <summary>
    /// Gets or Sets the text body of the cheep.
    /// </summary>
    /// <value>The body text of the cheep.</value>
    public string Text { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the cheep was created.
    /// </summary>
    /// <value>The UTC timestamp when the cheep was recorded.</value>
    public DateTime TimeStamp { get; set; }

    /// <summary>
    /// Gets or sets the foreign key referencing the <see cref="Author"/> who created the cheep.
    /// </summary>
    /// <remarks>
    /// this requires an <see cref="Author"/> to be present in the database.
    /// </remarks>
    /// <value>The integer foreign key of the cheep's author.</value>
    [Required]
    public required int AuthorId { get; set; }

    /// <summary>
    /// Gets or sets the navigation property to the <see cref="Author"/> who wrote the cheep.
    /// </summary>
    /// <value>An <see cref="Author"/> instance representing the cheep's author.</value>
    /// <remarks>
    /// This property is required and must be set when creating a new cheep.
    /// </remarks>
    [Required]
>>>>>>> Stashed changes
    public required Author Author { get; set; }
}