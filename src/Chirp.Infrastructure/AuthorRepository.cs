public class AuthorRepository : IAuthorRepository
{
    private readonly ChirpDBContext _context;

    public AuthorRepository(ChirpDBContext context)
    {
        _context = context;
    }

    public async Task CreateAuthor(Author author)
    {
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();
    }
    public async Task<Author> FindAuthorByName(string name)
    {
        return await Task.Run(()=>_context.Authors.Where(a => a.Name.Equals(name)).First()); //Unique Names??? if not this needs to change
    }
    public async Task<Author> FindAuthorByEmail(string email)
    {
        return await Task.Run(()=>_context.Authors.Where(e => e.Email!.Equals(email)).First()); 
    }
    public async Task RemoveAuthor(Author author)
    {
        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();
    }
}