/// <include file="../../docs/ICheepServiceDocs.xml" path="/doc/members/member[@name='T:ICheepService']/*" />
public interface ICheepService 
{
    /// <include file="../../docs/ICheepServiceDocs.xml" path="/doc/members/member[@name='M:ICheepService.GetCheeps']/*" />
    public List<CheepDTO> GetCheeps();

    /// <include file="../../docs/ICheepServiceDocs.xml" path="/doc/members/member[@name='M:ICheepService.GetCheeps(System.Int32)']/*" />
    public List<CheepDTO> GetCheeps(int page);

    /// <include file="../../docs/ICheepServiceDocs.xml" path="/doc/members/member[@name='M:ICheepService.GetCheepsFromAuthor(System.String)']/*" />
    public List<CheepDTO> GetCheepsFromAuthor(string author);

    /// <include file="../../docs/ICheepServiceDocs.xml" path="/doc/members/member[@name='M:ICheepService.GetCheepsFromAuthor(System.String,System.Int32)']/*" />
    public List<CheepDTO> GetCheepsFromAuthor(string author, int page);
    public void PostCheep(Author author, string message);
}