/// <include file="../../docs/ICheepServiceDocs.xml" path="/doc/members/member[@name='T:ICheepService']/*" />
public interface ICheepService 
{
    /// <include file="../../docs/ICheepServiceDocs.xml" path="/doc/members/member[@name='M:ICheepService.GetCheeps']/*" />
    public List<CheepDTO> GetCheeps();

    /// <include file="../../docs/ICheepServiceDocs.xml" path="/doc/members/member[@name='M:ICheepService.GetCheeps(System.Int32)']/*" />
    public List<CheepDTO> GetCheeps(int page);

    /// <include file="../../docs/ICheepServiceDocs.xml" path="/doc/members/member[@name='M:ICheepService.GetCheepsFromAuthor(System.String)']/*" />
    public List<CheepDTO> GetCheepsFromAuthor(string author); //Do we need this is it not just a helper method for the method below

    /// <include file="../../docs/ICheepServiceDocs.xml" path="/doc/members/member[@name='M:ICheepService.GetCheepsFromAuthor(System.String,System.Int32)']/*" />
    public List<CheepDTO> GetCheepsFromAuthor(string author, int page);

    public CheepDTO GetCheepFromID(int id);

    public void UpdateCheep(int id, string? message, bool? likes); //add dislikes?
    public void PostCheep(Author author, string message);
}