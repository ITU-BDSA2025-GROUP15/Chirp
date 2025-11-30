using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Infrastructure;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _repository;
    public AuthorService(IAuthorRepository repository)
    {
        _repository = repository;
    }
    public async Task<AuthorDTO> GetAuthorByName(string name)
    {
        return AuthorToDTO(await _repository.FindAuthorByName(name));
    }
    public async Task<bool> IsAuthorFollowingAuthor(AuthorDTO fan, AuthorDTO idol)
    {
        var fanA = await DTOToAuthor(fan);
        var idolA =  await DTOToAuthor(idol);
        return fanA.Follows == null ? false : fanA.Follows.Contains(idolA);
    }
    public async Task FollowAuthor(AuthorDTO fan, AuthorDTO idol)
    {
        await _repository.AddFollow(await DTOToAuthor(fan),await DTOToAuthor(idol));
    }

    public async Task UnFollowAuthor(AuthorDTO fan, AuthorDTO idol)
    {
        var fanA = await DTOToAuthor(fan);
        var idolA =  await DTOToAuthor(idol);
        if (fanA.Follows != null && idolA != null)
        {
            await _repository.RemoveFollow(fanA,await DTOToAuthor(idol));
        }
    }
    public async Task<string[]> GetFollowingByName(string name)
    {
        Author author = await _repository.FindAuthorByName(name);
        if (author.Follows == null)
        {
            return [];
        }
        string[] follows = [];
        foreach (Author item in author.Follows)
        {
            follows = (string[])follows.Append(item.Name);
        }
        return follows;
    }
    private AuthorDTO AuthorToDTO(Author author)
    {
        return new AuthorDTO()
        {
            Name = author.Name
        };
    }
    private async Task<Author> DTOToAuthor(AuthorDTO authorDTO)
    {
        return await _repository.FindAuthorByName(authorDTO.Name);
    }
}