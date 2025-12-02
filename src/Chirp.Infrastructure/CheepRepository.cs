using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using SQLitePCL;

using System.Linq;

/// <include file="../../docs/CheepRepositoryDocs.xml" path="/doc/members/member[@name='T:CheepRepository']/*" />
public class CheepRepository : ICheepRepository
{
    private readonly int defaultLimit = 32;
    private readonly ChirpDBContext _context;


    /// <include file="../../docs/CheepRepositoryDocs.xml" path="/doc/members/member[@name='M:CheepRepository.#ctor(ChirpDBContext)']/*" />
    public CheepRepository(ChirpDBContext context)
    {
        _context = context;
    }
    /// <include file="../../docs/CheepRepositoryDocs.xml" path="/doc/members/member[@name='M:CheepRepository.CreateMessage(Cheep)']/*" />
    public async Task CreateMessage(Cheep newMessage)
    {
        if (String.IsNullOrWhiteSpace(newMessage.Text))
        {
            throw new ArgumentException("Message is empty!?!");
        }
        /*max length for cheeps*/
        const int maxLength = 160;
        if (newMessage.Text.Length > maxLength)
        {
            throw new ArgumentException("Max length exceeded!?!");
        }
        var query = _context.Cheeps.Add(newMessage);
        await _context.SaveChangesAsync();
    }

    /// <include file="../../docs/CheepRepositoryDocs.xml" path="/doc/members/member[@name='M:CheepRepository.ReadMessages(System.String,System.Nullable{System.Int32},System.Nullable{System.Int32})']/*" />
    public async Task<List<CheepDTO>> ReadMessages(IEnumerable<string>? authors, int? page, int? limit)

    {
        if (authors == null)
        {
            return await ReadMessages([], page, limit);
        }
        var query = _context.Cheeps
            .Join(_context.Authors,
                Cheeps => Cheeps.AuthorId,
                Authors => Authors.Id,
                (Cheeps, Authors) => new
                {
                    Author = Authors.Name,
                    Message = Cheeps.Text,
                    Timestamp = Cheeps.TimeStamp
                });
        if (authors != null)
        {
            if (authors.Count() > 0)
            {
                query = query.Where(Cheep => authors.Contains(Cheep.Author));
            }
        }

        query = query.OrderByDescending(Cheep => Cheep.Timestamp);

        // Use default values for limit/page if any null
        int _limit = limit ?? defaultLimit;
        int _page = page ?? 1;

        try
        {
            query = query.Skip(checked((_page - 1) * _limit));
            query = query.Take(_limit);
        } catch (OverflowException) {
            return new List<CheepDTO>();
        }

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

    /// <include file="../../docs/CheepRepositoryDocs.xml" path="/doc/members/member[@name='M:CheepRepository.UpdateMessage(CheepDTO)']/*" />
    public Task UpdateMessage(CheepDTO alteredMessage)
    {
        return Task.Run(() => 0); //does nothing
    }
    
    ///<include file="../../docs/CheepRepositoryDocs.xml" path="/doc/members/member[@name='M:CheepRepository.FindMessage(System.Int32)']/*" />
    public async Task<Cheep> FindMessage(int cheepId)
    {
        return await Task.Run(() =>_context.Cheeps.Where(a => a.CheepId.Equals(cheepId)).First());
    }
}