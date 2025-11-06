using System.ComponentModel.DataAnnotations;

/// <include file="../../docs/AuthorDocs.xml" path="/doc/members/class[@name='T:Author']/*" />
public class Author
{
    /// <include file="../../docs/AuthorDocs.xml" path="/doc/members/class/member[@name='P:Author.AuthorId']/*" />
    public int AuthorId { get; set; }

    /// <include file="../../docs/AuthorDocs.xml" path="/doc/members/class/member[@name='P:Author.Name']/*" />
    public required string Name { get; set; }

    /// <include file="../../docs/AuthorDocs.xml" path="/doc/members/class/member[@name='P:Author.Email']/*" />
    public string? Email { get; set; }

    /// <include file="../../docs/AuthorDocs.xml" path="/doc/members/class/member[@name='P:Author.Cheeps']/*" />
    public List<Cheep>? Cheeps { get; set; }
}