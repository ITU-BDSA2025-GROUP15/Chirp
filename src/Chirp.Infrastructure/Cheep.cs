using System;
using System.ComponentModel.DataAnnotations;


 /// <include file="../../docs/CheepDocs.xml" path="/doc/members/member[@name='T:Cheep']/*" />
public class Cheep
{
    /// <include file="../../docs/CheepDocs.xml" path="/doc/members/member[@name='P:Cheep.CheepId']/*" />
    public int CheepId { get; set; }

    /// <include file="../../docs/CheepDocs.xml" path="/doc/members/member[@name='P:Cheep.Text']/*" />
    [Required]
    [StringLength(160)]
    public required string Text { get; set; }

    /// <include file="../../docs/CheepDocs.xml" path="/doc/members/member[@name='P:Cheep.TimeStamp']/*" />
    public DateTime TimeStamp { get; set; }

    public int LikeCounter { get; set; } = 0;

    /// <include file="../../docs/CheepDocs.xml" path="/doc/members/member[@name='P:Cheep.AuthorId']/*" />
    [Required]
    public required int AuthorId { get; set; }

    /// <include file="../../docs/CheepDocs.xml" path="/doc/members/member[@name='P:Cheep.Author']/*" />
    public required Author Author { get; set; }
}