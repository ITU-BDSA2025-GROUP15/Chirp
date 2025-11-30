using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

[PrimaryKey(nameof(CheepId), nameof(AuthorId))]
public class PostOpinions
{
    public int CheepId { get; set; }
    public int AuthorId { get; set; }
}