class AuthorService : IAuthorService
{
    IAuthorRepository _repository;
    public AuthorService(IAuthorRepository repository)
    {
        _repository = repository;
    }
    public Task<Author> GetAuthorByName()
    {
        
    }
}