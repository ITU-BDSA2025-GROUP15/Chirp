using Microsoft.EntityFrameworkCore;

public class ChirpDBContext : DbContext
{
    public ChirpDBContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Author> Authors { get; set; }
    public DbSet<Cheep> Cheeps { get; set; }
}