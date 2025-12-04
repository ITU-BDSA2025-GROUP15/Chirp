public interface IAuthorService
{
    public Task<AuthorDTO> GetAuthorByName(string name);
    public Task<string[]> GetFollowingByName(string name);
    public Task<bool> IsAuthorFollowingAuthor(AuthorDTO fan, AuthorDTO idol);
    public Task FollowAuthor(AuthorDTO fan, AuthorDTO idol);
    public Task UnFollowAuthor(AuthorDTO fan, AuthorDTO idol);
}