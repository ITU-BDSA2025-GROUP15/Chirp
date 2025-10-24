public interface IAuthorRepository
{
    public Task CreateAuthor(Author author);
    public Task<Author> FindAuthorByName(string name);
    public Task<Author> FindAuthorByEmail(string email);

    public Task RemoveAuthor(Author author);//temp till testing in memory.
}