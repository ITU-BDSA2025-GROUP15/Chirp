/// <summary>
/// A small data transfer object containing only the display name of an author.
/// </summary>
/// <remarks>
/// Use DTOs to avoid passing full entity objects (with navigation properties) into the UI
/// or across process boundaries. This DTO is intentionally minimal and used for presentation.
/// </remarks>
/// <example>
/// <code>
/// // Example mapping from an Author entity to the DTO
/// var dto = new AuthorDTO { Name = author.Name };
/// </code>
/// </example>
public class AuthorDTO
{
    /// <summary>
    /// The display name of the author.
    /// </summary>
    /// <value>A short, human-friendly name shown in the UI.</value>
    string Name { get; set; }
}