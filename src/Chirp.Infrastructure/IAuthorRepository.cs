/// <include file="../../docs/IAuthorRepositoryDocs.xml" path="/doc/members/member[@name='T:IAuthorRepository']/*" />
public interface IAuthorRepository
{
    /// <include file="../../docs/IAuthorRepositoryDocs.xml" path="/doc/members/member[@name='M:IAuthorRepository.CreateAuthor(Author)']/*" />
    public Task CreateAuthor(Author author);

    /// <include file="../../docs/IAuthorRepositoryDocs.xml" path="/doc/members/member[@name='M:IAuthorRepository.FindAuthorByName(System.String)']/*" />
    public Task<Author> FindAuthorByName(string name);

    /// <include file="../../docs/IAuthorRepositoryDocs.xml" path="/doc/members/member[@name='M:IAuthorRepository.FindAuthorByEmail(System.String)']/*" />
    public Task<Author> FindAuthorByEmail(string email);

    /// <include file="../../docs/IAuthorRepositoryDocs.xml" path="/doc/members/member[@name='M:IAuthorRepository.RemoveAuthor(Author)']/*" />
    public Task RemoveAuthor(Author author);
    public Task AddFollow(Author author, Author toFollow);
    public Task RemoveFollow(Author author, Author toUnfollow);
}