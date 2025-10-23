public class AuthorRepository : IAuthorRepository
{
    private readonly ChirpDBContext _context;

    public AuthorRepository(ChirpDBContext context)
    {
        _context = context;
    }

        public Task CreateAuthor(CheepDTO newMessage)
    {
        return Task.Run(() => 0); //does nothing
    }
}