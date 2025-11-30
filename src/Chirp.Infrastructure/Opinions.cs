using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
public class Opinions
{
    public int CheepId { get; set; }
    public int AuthorId { get; set; }
}