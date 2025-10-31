using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using SQLitePCL;

using System.Linq;


public class CheepRepository : ICheepRepository
{
    private readonly int defaultLimit = 32;
    private readonly ChirpDBContext _context;

    public CheepRepository(ChirpDBContext context)
    {
        _context = context;
    }

    public async Task CreateMessage(Cheep newMessage)
    {   
        if (String.IsNullOrWhiteSpace(newMessage.Text)||newMessage.Text.Length > 160)
        {
            throw new ArgumentException("Max length exceeded!?!, or message is empty!?!");
        }
        var query = _context.Cheeps.Add(newMessage);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Reads messages from database.
    /// </summary>
    /// <param name="author">Author of messages. If null, all authors are returned.</param>
    /// <param name="page">Page number to read. If null, defaults to 1.</param>
    /// <param name="limit">Messages per page. If null, defaults to 32.</param>
    /// <returns>List of messages converted to CheepDTO.</returns>
    public async Task<List<CheepDTO>> ReadMessages(string? author, int? page, int? limit)
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

        // Use default values for limit/page if any null
        int _limit = limit ?? defaultLimit;
        int _page = page ?? 1;

        query = query.Skip((_page - 1) * _limit);
        query = query.Take(_limit);
    
        var cheeps = await query.ToListAsync();
        var cheepdtos = new List<CheepDTO>();

        foreach (var cheep in cheeps)
        {
            cheepdtos.Add(new CheepDTO
            {
                Author = cheep.Author,
                Message = cheep.Message,
                Timestamp = CheepService.DateTimeToDateTimeString(cheep.Timestamp)
            });
        }

        return cheepdtos;
    }
    public Task UpdateMessage(CheepDTO alteredMessage)
    {
        return Task.Run(() => 0); //does nothing
    }
    /// <summary>
    /// For testing purposes
    /// </summary>
    /// <param name="cheepId"></param>
    /// <returns></returns>
    public async Task<Cheep> FindMessage(int cheepId)
    {
        return await Task.Run(() =>_context.Cheeps.Where(a => a.CheepId.Equals(cheepId)).First());
    }
}