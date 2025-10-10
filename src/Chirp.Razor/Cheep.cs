using System;
using System.ComponentModel.DataAnnotations;

public class Cheep
{
    public string text { get; set; }
    public DateTime timestamp { get; set; }
    [Required]
    public required Author author { get; set; }
}