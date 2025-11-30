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
        return await _context.Authors.FirstAsync(a => a.Name.Equals(name));
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

    public async Task AddFollow(Author author, Author toFollow)
    {
        author.Follows ??= new List<Author>();
        author.Follows.Add(toFollow);
        _context.Authors.Update(author);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveFollow(Author author, Author toUnfollow)
    {
        if (author.Follows != null)
        {
            author.Follows.Remove(toUnfollow);
            _context.Authors.Update(author);
            await _context.SaveChangesAsync();
        }
    }
}