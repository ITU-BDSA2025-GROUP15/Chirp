using System;
using System.ComponentModel.DataAnnotations;

public class Cheep
{
    public int CheepId;
    public string Text { get; set; }
    public DateTime TimeStamp { get; set; }
    [Required]
    public required int AuthorId { get; set; }
    public required Author Author { get; set; }
}