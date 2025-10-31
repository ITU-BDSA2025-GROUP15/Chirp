using System;
using System.ComponentModel.DataAnnotations;


 /// <include file="../../docs/CheepDocs.xml" path="/doc/members/member[@name='T:Cheep']/*" />
public class Cheep
{
    
    /// <include file="../../docs/CheepDocs.xml" path="/doc/members/member[@name='P:Cheep.CheepId']/*" />
    public int CheepId { get; set; }

    /// <include file="../../docs/CheepDocs.xml" path="/doc/members/member[@name='P:Cheep.Text']/*" />
    [Required]
    [StringLength(280)]
    public required string Text { get; set; }

    /// <include file="../../docs/CheepDocs.xml" path="/doc/members/member[@name='P:Cheep.TimeStamp']/*" />
    public DateTime TimeStamp { get; set; }

    /// <include file="../../docs/CheepDocs.xml" path="/doc/members/member[@name='P:Cheep.AuthorId']/*" />
    [Required]
    public required int AuthorId { get; set; }

    /// <include file="../../docs/CheepDocs.xml" path="/doc/members/member[@name='P:Cheep.Author']/*" />
    public required Author Author { get; set; }
}