using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ChirpDBContext : IdentityDbContext<Author, IdentityRole<int>, int>
{
    public ChirpDBContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Author> Authors { get; set; }
    public DbSet<Cheep> Cheeps { get; set; }
}