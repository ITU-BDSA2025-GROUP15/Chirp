using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using SQLitePCL;

using System.Linq;

public interface ICheepRepository
{
    public Task CreateMessage(CheepDTO newMessage);
    public Task<List<CheepDTO>> ReadMessages(string? author, int? pages, int? limit);
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
    public async Task<List<CheepDTO>> ReadMessages(string? author, int? pages, int? limit) //maybe DBFacade should handle null values
    {
        var query = _context.Cheeps
            .Join(_context.Authors,
                Cheeps => Cheeps.AuthorId,
                Authors => Authors.AuthorId,
                (Cheeps, Authors) => new
                {
                    Author = Authors.Name,
                    Message = Cheeps.Text,
                    Timestamp = Cheeps.TimeStamp
                });
        if (author != null)
        {
            query = query.Where(Cheep => Cheep.Author == author);
        }

        query = query.OrderByDescending(Cheep => Cheep.Timestamp);

        if (pages != null && limit != null)
        {
            int notNullLimit = limit ?? defaultLimit;
            int notNullPage = pages ?? 1;
            query = query.Skip((notNullPage - 1) * notNullLimit);
        }
        if (limit != null)
        {
            int notNullLimit = limit ?? defaultLimit;
            query = query.Take(notNullLimit);
        }

        var cheeps = await query.ToListAsync();
        var cheepdtos = new List<CheepDTO>();

        foreach (var cheep in cheeps)
        {
            cheepdtos.Add(new CheepDTO
            {
                Author = cheep.Author,
                Message = cheep.Message,
                Timestamp = cheep.Timestamp.ToString("MM/dd/yy H:mm:ss")
            });
        }

        return cheepdtos;
    }
    public Task UpdateMessage(CheepDTO alteredMessage)
    {
        return Task.Run(() => 0); //does nothing
    }

}