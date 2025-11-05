using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

[Index(nameof(Name), IsUnique=true)]
public class Author : IdentityUser<int>
{
    [NotMapped]
    public int AuthorId { get { return Id; } set { Id = value; } }
    public required string Name { get; set; }
    // public string? Email { get; set; }
    public List<Cheep>? Cheeps { get; set; }
}