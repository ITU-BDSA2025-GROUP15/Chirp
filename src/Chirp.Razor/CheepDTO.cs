/// <summary>
/// Presentation-friendly view of a <see cref="Cheep"/> used by UI code.
/// </summary>
/// <remarks>
/// The DTO flattens the entity graph (e.g. only the author's name is included) and formats
/// the timestamp for display. This keeps Razor pages and views simple and focused on rendering.
/// </remarks>
/// <example>
/// <code>
/// // Typical DTO returned to a Razor page
/// new CheepDTO { Author = "Ada", Message = "Hi!", Timestamp = "10/24/25 14:05:07" };
/// </code>
/// </example>
public class CheepDTO
{
    /// <summary>
    /// The display name of the author who posted the cheep.
    /// </summary>
    /// <value>Shown in the UI and used for basic filtering.</value>
    public string Author { get; set; }

    /// <summary>
    /// The message text of the cheep.
    /// </summary>
    /// <value>Untrimmed cheep content as stored in the application layer.</value>
    public string Message { get; set; }

    /// <summary>
    /// The timestamp formatted as a display string.
    /// </summary>
    /// <value>Produced by <see cref="CheepService.DateTimeToDateTimeString"/>.</value>
    public string Timestamp { get; set; }
}
