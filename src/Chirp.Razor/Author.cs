using System.ComponentModel.DataAnnotations;

public class Author
{
    public int AuthorID { get; set; }
    public string Name { get; set; }
    public string? Email { get; set; }
    public List<Cheep>? Cheeps { get; set; }
}