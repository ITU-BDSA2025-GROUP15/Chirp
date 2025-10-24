public interface ICheepRepository
{
    public Task CreateMessage(Cheep newMessage);
    public Task<List<CheepDTO>> ReadMessages(string? author, int? page, int? limit);
    public Task UpdateMessage(CheepDTO alteredMessage);
    public Task<Cheep> FindMessage(int cheepId);
}