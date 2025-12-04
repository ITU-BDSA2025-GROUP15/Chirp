using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

/// <include file="../../docs/AuthorDocs.xml" path="/doc/members/class[@name='T:Author']/*" />
[Index(nameof(Name), IsUnique=true)]
public class Author : IdentityUser<int>
{
    /// <include file="../../docs/AuthorDocs.xml" path="/doc/members/class/member[@name='P:Author.AuthorId']/*" />
    [NotMapped]
    public int AuthorId { get { return Id; } set { Id = value; } }

    /// <include file="../../docs/AuthorDocs.xml" path="/doc/members/class/member[@name='P:Author.Name']/*" />
    public required string Name { get; set; }

    /// <include file="../../docs/AuthorDocs.xml" path="/doc/members/class/member[@name='P:Author.Email']/*" />
    //public string? Email { get; set; }

    /// <include file="../../docs/AuthorDocs.xml" path="/doc/members/class/member[@name='P:Author.Cheeps']/*" />
    public List<Cheep>? Cheeps { get; set; }

    public List<Author>? Follows { get; set; }
}