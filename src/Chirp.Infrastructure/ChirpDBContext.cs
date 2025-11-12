using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

/// <include file="../../docs/ChirpDBContextDocs.xml" path="/doc/members/member[@name='T:ChirpDBContext']/*" />
public class ChirpDBContext : IdentityDbContext<Author, IdentityRole<int>, int>
{
    /// <include file="../../docs/ChirpDBContextDocs.xml" path="/doc/members/member[@name='M:ChirpDBContext.#ctor(Microsoft.EntityFrameworkCore.DbContextOptions)']/*" />
    public ChirpDBContext(DbContextOptions options) : base(options)
    {
    }

    /// <include file="../../docs/ChirpDBContextDocs.xml" path="/doc/members/member[@name='P:ChirpDBContext.Authors']/*" />
    public DbSet<Author> Authors { get; set; }

    /// <include file="../../docs/ChirpDBContextDocs.xml" path="/doc/members/member[@name='P:ChirpDBContext.Cheeps']/*" />
    public DbSet<Cheep> Cheeps { get; set; }
}