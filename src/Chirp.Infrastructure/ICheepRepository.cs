public interface ICheepRepository
{
    public Task CreateMessage(CheepDTO newMessage);
    public Task<List<CheepDTO>> ReadMessages(string? author, int? page, int? limit);
    public Task UpdateMessage(CheepDTO alteredMessage);
}