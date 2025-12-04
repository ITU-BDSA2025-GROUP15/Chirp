/// <include file="../../docs/ICheepRepositoryDocs.xml" path="/doc/members/member[@name='T:ICheepRepository']/*" />
public interface ICheepRepository
{
    /// <include file="../../docs/ICheepRepositoryDocs.xml" path="/doc/members/member[@name='M:ICheepRepository.CreateMessage(Cheep)']/*" />
    public Task CreateMessage(Cheep newMessage);

    /// <include file="../../docs/ICheepRepositoryDocs.xml" path="/doc/members/member[@name='M:ICheepRepository.ReadMessages(System.String,System.Nullable{System.Int32},System.Nullable{System.Int32})']/*" />
    public Task<List<CheepDTO>> ReadMessages(IEnumerable<string>? authors, int? page, int? limit);

    /// <include file="../../docs/ICheepRepositoryDocs.xml" path="/doc/members/member[@name='M:ICheepRepository.UpdateMessage(CheepDTO)']/*" />
    public Task UpdateMessage(CheepDTO alteredMessage);

    /// <include file="../../docs/ICheepRepositoryDocs.xml" path="/doc/members/member[@name='M:ICheepRepository.FindMessage(System.Int32)']/*" />
    public Task<Cheep> FindMessage(int cheepId);
}