/// <include file="../../docs/ICheepServiceDocs.xml" path="/doc/members/member[@name='T:ICheepService']/*" />
public interface ICheepService 
{
    /// <include file="../../docs/ICheepServiceDocs.xml" path="/doc/members/member[@name='M:ICheepService.GetCheeps']/*" />
    public List<CheepDTO> GetCheeps();

    /// <include file="../../docs/ICheepServiceDocs.xml" path="/doc/members/member[@name='M:ICheepService.GetCheeps(System.Int32)']/*" />
    public List<CheepDTO> GetCheeps(int page, string sorting);

    /// <include file="../../docs/ICheepServiceDocs.xml" path="/doc/members/member[@name='M:ICheepService.GetCheepsFromAuthor(System.String)']/*" />
    public List<CheepDTO> GetCheepsFromAuthor(string author); //Do we need this is it not just a helper method for the method below

    /// <include file="../../docs/ICheepServiceDocs.xml" path="/doc/members/member[@name='M:ICheepService.GetCheepsFromAuthor(System.String,System.Int32)']/*" />
    public List<CheepDTO> GetCheepsFromAuthor(string author, int page, string sorting);

    public CheepDTO GetCheepFromID(int id);

    public Task<int> UpdateCheep(int id, string? message, bool HasLiked); //add dislikes?

    public Task<int> Likes(int authorId, int cheepId);

    public Task<bool> HasUserLiked(int authorId, int cheepId);
    public void PostCheep(Author author, string message);
}