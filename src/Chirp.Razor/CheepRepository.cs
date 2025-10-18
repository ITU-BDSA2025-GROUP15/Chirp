using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using SQLitePCL;

using System.Linq;

public interface ICheepRepository
{
    public Task CreateMessage(CheepDTO newMessage);
    public Task<List<Cheep>> ReadMessages(string? author, int? pages, int? limit);
    public Task UpdateMessage(CheepDTO alteredMessage);
}

public class CheepRepository : ICheepRepository
{
    private readonly int defaultLimit = 32;
    private readonly ChirpDBContext _context;

    public CheepRepository(ChirpDBContext context)
    {
        _context = context;
    }

    public Task CreateMessage(CheepDTO newMessage)
    {
        return Task.Run(() => 0); //does nothing
    }
    public async Task<List<Cheep>> ReadMessages(string? author, int? pages, int? limit)
    {
        //stupid solution
        var query = _context.Cheeps
            .Join(_context.Authors,
                Cheeps => Cheeps.AuthorId,
                Authors => Authors.AuthorId,
                (Cheeps, Authors) => new Cheep
                {
                    AuthorId = Authors.AuthorId,
                    Author = new Author { AuthorId = Authors.AuthorId, Name = Authors.Name },
                    Text = Cheeps.Text,
                    TimeStamp = Cheeps.TimeStamp
                });
        if (author != null)
        {
            query = query.Where(Cheep => Cheep.Author.Name == author);
        }

        query = query.OrderByDescending(Cheep => Cheep.TimeStamp);

        if (pages != null && limit != null)
        {
            int notNullLimit = limit ?? defaultLimit;
            int notNullPage = pages ?? 1;
            query = query.Skip((notNullPage-1) * notNullLimit);
        }
        if (limit != null)
        {
            int notNullLimit = limit ?? defaultLimit;
            query = query.Take(notNullLimit);
        }

        return await query.ToListAsync();
    }
    public Task UpdateMessage(CheepDTO alteredMessage)
    {
        return Task.Run(() => 0); //does nothing
    }

}