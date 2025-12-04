/// <include file="../../docs/ICheepRepositoryDocs.xml" path="/doc/members/member[@name='T:ICheepRepository']/*" />
public interface ICheepRepository
{
    /// <include file="../../docs/ICheepRepositoryDocs.xml" path="/doc/members/member[@name='M:ICheepRepository.CreateMessage(Cheep)']/*" />
    public Task CreateMessage(Cheep newMessage);

    /// <include file="../../docs/ICheepRepositoryDocs.xml" path="/doc/members/member[@name='M:ICheepRepository.ReadMessages(System.String,System.Nullable{System.Int32},System.Nullable{System.Int32})']/*" />
    public Task<List<CheepDTO>> ReadMessages(string? author, int? page, int? limit, string? sorting);

    /// <include file="../../docs/ICheepRepositoryDocs.xml" path="/doc/members/member[@name='M:ICheepRepository.UpdateMessage(CheepDTO)']/*" />
    public Task UpdateMessage(CheepDTO alteredMessage);

    public Task<int> Likes(int authorId, int cheepId, bool likes);

    public Task<bool> OpinionExist(int authorId, int cheepId);

    /// <include file="../../docs/ICheepRepositoryDocs.xml" path="/doc/members/member[@name='M:ICheepRepository.FindMessage(System.Int32)']/*" />
    public Task<CheepDTO> FindMessage(int cheepId);
}