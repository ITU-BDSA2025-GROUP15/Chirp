using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

/// <include file="../../docs/AuthorRepositoryDocs.xml" path="/doc/members/member[@name='T:AuthorRepository']/*" />
public class AuthorRepository : IAuthorRepository
{
    /// <include file="../../docs/AuthorRepositoryDocs.xml" path="/doc/members/member[@name='F:AuthorRepository._context']/*" />
    private readonly ChirpDBContext _context;

    /// <include file="../../docs/AuthorRepositoryDocs.xml" path="/doc/members/member[@name='M:AuthorRepository.#ctor(ChirpDBContext)']/*" />
    public AuthorRepository(ChirpDBContext context)
    {
        _context = context;
    }

    /// <include file="../../docs/AuthorRepositoryDocs.xml" path="/doc/members/member[@name='M:AuthorRepository.CreateAuthor(Author)']/*" />
    public async Task CreateAuthor(Author author)
    {
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();
    }

    /// <include file="../../docs/AuthorRepositoryDocs.xml" path="/doc/members/member[@name='M:AuthorRepository.FindAuthorByName(System.String)']/*" />
    public async Task<Author> FindAuthorByName(string name)
    {
        // Use EF Core async query to avoid blocking a thread pool thread.
        // This preserves the original behaviour (throws if no match) using FirstAsync.
        return await _context.Authors.FirstAsync(a => a.Name.Equals(name)); // Note: equality/case-sensitivity depends on DB collation
    }

    /// <include file="../../docs/AuthorRepositoryDocs.xml" path="/doc/members/member[@name='M:AuthorRepository.FindAuthorByEmail(System.String)']/*" />
    public async Task<Author> FindAuthorByEmail(string email)
    {
        return await Task.Run(()=>_context.Authors.Where(e => e.Email!.Equals(email)).First()); 
    }

    /// <include file="../../docs/AuthorRepositoryDocs.xml" path="/doc/members/member[@name='M:AuthorRepository.RemoveAuthor(Author)']/*" />
    public async Task RemoveAuthor(Author author)
    {
        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();
    }
}