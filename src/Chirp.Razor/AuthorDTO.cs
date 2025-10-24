<<<<<<< Updated upstream
public class AuthorDTO
{
    string Name {get; set;}
=======
/// <summary>
/// A small data transfer object containing only the display name of an author.
/// </summary>
/// <remarks>
/// This DTO contains a subset of the <see cref="Author"/> properties and is intended
/// for presentation or input scenarios where only the author's name is required.
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
    /// Gets or sets the author's display name.
    /// </summary>
    string Name { get; set; }
>>>>>>> Stashed changes
}