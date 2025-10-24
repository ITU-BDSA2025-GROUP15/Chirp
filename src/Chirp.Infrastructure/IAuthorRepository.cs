public interface IAuthorRepository
{
    public Task CreateAuthor(Author author);
    public Task<Author> FindAuthorByName(string name);

    public Task RemoveAuthor(Author author);//temp till testing in memory.
}