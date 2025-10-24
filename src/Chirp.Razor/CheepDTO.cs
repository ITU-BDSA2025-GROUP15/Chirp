<<<<<<< Updated upstream
public class CheepDTO
{
    public string Author { get; set; }
    public string Message { get; set; }
=======
/// <summary>
/// Presentation-friendly view of a <see cref="Cheep"/> used by UI code.
/// </summary>
/// <remarks>
/// This DTO is a flattened view of a <see cref="Cheep"/>, containing the author's name,
/// the message text and a formatted timestamp suitable for display.
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
    /// Gets or sets the display name of the author who posted the cheep.
    /// </summary>
    /// <value>Shown in the UI and used for filtering?</value>
    public string Author { get; set; }

    /// <summary>
    /// Gets or sets the cheep message text.
    /// </summary>
    /// <value>
    /// The textual content of the cheep. Not trimmed by DTO conversion.
    /// </value>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the timestamp formatted as a string.
    /// </summary>
    /// <value>
    /// A formatted timestamp produced by <see cref="CheepService.DateTimeToDateTimeString"/>.
    /// </value>
>>>>>>> Stashed changes
    public string Timestamp { get; set; }
}
