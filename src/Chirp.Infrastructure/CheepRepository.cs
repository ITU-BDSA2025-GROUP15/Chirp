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
    public async Task<List<CheepDTO>> ReadMessages(string? author, int? page, int? limit)
    {
        var query = _context.Cheeps
            .Join(_context.Authors,
                Cheeps => Cheeps.AuthorId,
                Authors => Authors.Id,
                (Cheeps, Authors) => new
                {
                    CheepId = Cheeps.CheepId,
                    Author = Authors.Name,
                    Message = Cheeps.Text,
                    Timestamp = Cheeps.TimeStamp,
                    LikeCounter = Cheeps.LikeCounter
                });
        if (author != null)
        {
            query = query.Where(Cheep => Cheep.Author == author);
        }

        query = query.OrderByDescending(Cheep => Cheep.Timestamp);

        // Use default values for limit/page if any null
        int _limit = limit ?? defaultLimit;
        int _page = page ?? 1;

        try
        {
            query = query.Skip(checked((_page - 1) * _limit));
            query = query.Take(_limit);
        }
        catch (OverflowException)
        {
            return new List<CheepDTO>();
        }

        var cheeps = await query.ToListAsync();
        var cheepdtos = new List<CheepDTO>();

        foreach (var cheep in cheeps)
        {
            cheepdtos.Add(new CheepDTO
            {
                CheepId = cheep.CheepId,
                Author = cheep.Author,
                Message = cheep.Message,
                Timestamp = CheepService.DateTimeToDateTimeString(cheep.Timestamp),
                LikeCounter = cheep.LikeCounter
            });
        }

        return cheepdtos;
    }

    /// <include file="../../docs/CheepRepositoryDocs.xml" path="/doc/members/member[@name='M:CheepRepository.UpdateMessage(CheepDTO)']/*" />
    public async Task UpdateMessage(CheepDTO alteredMessage)
    {
        var cheep = await _context.Cheeps.Where(a => a.CheepId.Equals(alteredMessage.CheepId)).Select(a => a).FirstAsync();
        cheep.LikeCounter = alteredMessage.LikeCounter;
        cheep.Text = alteredMessage.Message;
        await _context.SaveChangesAsync();
        return;
    }

        public async Task<bool> Likes(int authorId, int postId, bool likes)
    {
        try
        {
            if (likes)
            {
                PostOpinions postOpinion = new PostOpinions
                {
                    CheepId = postId,
                    AuthorId = authorId
                };
                var query = _context.PostOpinions.Add(postOpinion);
                await _context.SaveChangesAsync();
            }
            else
            {
                PostOpinions postOpinion = new PostOpinions
                {
                    CheepId = postId,
                    AuthorId = authorId
                };
                var query = _context.PostOpinions.Remove(postOpinion);
                await _context.SaveChangesAsync();
            }
            return true;
        } catch
        {
            return false;
        }
    }

    public async Task<bool> OpinionExist(int auhorId, int cheepId)
    {
        return await _context.PostOpinions
            .AnyAsync(l => l.AuthorId == auhorId && l.CheepId == cheepId);
    }

    ///<include file="../../docs/CheepRepositoryDocs.xml" path="/doc/members/member[@name='M:CheepRepository.FindMessage(System.Int32)']/*" />
    public async Task<CheepDTO> FindMessage(int cheepId)
    {
        var query = _context.Cheeps
            .Join(_context.Authors,
            Cheeps => Cheeps.AuthorId,
            Authors => Authors.Id,
            (Cheeps, Authors) => new
            {
                CheepId = Cheeps.CheepId,
                Author = Authors.Name,
                Message = Cheeps.Text,
                Timestamp = Cheeps.TimeStamp,
                LikeCounter = Cheeps.LikeCounter
            });
        query = query.Where(a => a.CheepId.Equals(cheepId));

        var cheep = await query.FirstAsync();

        Console.WriteLine("This is the id :" + cheep.CheepId);
        Console.WriteLine("This is cheep Author: " + cheep.Author);
        Console.WriteLine("This is cheep message: " + cheep.Message);
        Console.WriteLine("This is cheep Timestamp: " + CheepService.DateTimeToDateTimeString(cheep.Timestamp));
        Console.WriteLine("This is cheep likes: " + cheep.LikeCounter);

        var cheepDTO = new CheepDTO
        {
            CheepId = cheep.CheepId,
            Author = cheep.Author,
            Message = cheep.Message,
            Timestamp = CheepService.DateTimeToDateTimeString(cheep.Timestamp),
            LikeCounter = cheep.LikeCounter
        };

        return cheepDTO;
    }
}