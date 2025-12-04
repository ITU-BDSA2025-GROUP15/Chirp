//public record CheepViewModel(string Author, string Message, string Timestamp);

using System.Threading.Tasks;

/// <include file="../../docs/CheepServiceDocs.xml" path="/doc/members/member[@name='T:CheepService']/*" />
public class CheepService : ICheepService
{
    private readonly ICheepRepository _repository;

    /// <include file="../../docs/CheepServiceDocs.xml" path="/doc/members/member[@name='M:CheepService.#ctor(ICheepRepository)']/*" />
    public CheepService(ICheepRepository repository)
    {
        _repository = repository;
    }
    /// <include file="../../docs/CheepServiceDocs.xml" path="/doc/members/member[@name='M:CheepService.GetCheeps']/*" />
    public List<CheepDTO> GetCheeps()
    {
        var messages = _repository.ReadMessages(null, null, null, null);
        return messages.GetAwaiter().GetResult();
    }

    /// <include file="../../docs/CheepServiceDocs.xml" path="/doc/members/member[@name='M:CheepService.GetCheeps(System.Int32)']/*" />
    public List<CheepDTO> GetCheeps(int page)
    {
        var messages = _repository.ReadMessages(null, page, null, null);
        return messages.GetAwaiter().GetResult();
    }

    public List<CheepDTO> GetCheeps(int page, string sorting)
    {
        var messages = _repository.ReadMessages(null, page, null, sorting);
        return messages.GetAwaiter().GetResult();
    }

    /// <include file="../../docs/CheepServiceDocs.xml" path="/doc/members/member[@name='M:CheepService.GetCheepsFromAuthor(System.String)']/*" />
    public List<CheepDTO> GetCheepsFromAuthor(string author)
    {
        var messages = _repository.ReadMessages(author, null, null, null);
        return messages.GetAwaiter().GetResult();
    }

    /// <include file="../../docs/CheepServiceDocs.xml" path="/doc/members/member[@name='M:CheepService.GetCheepsFromAuthor(System.String,System.Int32)']/*" />
    public List<CheepDTO> GetCheepsFromAuthor(string author, int page)
    {
        var messages = _repository.ReadMessages(author, page, null, null);
        return messages.GetAwaiter().GetResult();
    }

    public List<CheepDTO> GetCheepsFromAuthor(string author, int page, string sorting)
    {
        var messages = _repository.ReadMessages(author, page, null, sorting);
        return messages.GetAwaiter().GetResult();
    }

    public CheepDTO GetCheepFromID(int id)
    {
        return _repository.FindMessage(id).GetAwaiter().GetResult();
    }

    public void UpdateCheep(int id, bool like)
    {
        UpdateCheep(id, null, like);
    }
    public async Task<int> UpdateCheep(int id, string? message, bool hasLiked)
    {
        var cheep = GetCheepFromID(id);
        if (message != null)
        {
            cheep.Message = message;
        }
        if (hasLiked)
        {
            cheep.LikeCounter--;
        } else
        {
            cheep.LikeCounter++;
        }
        await _repository.UpdateMessage(cheep);
        return cheep.LikeCounter;
    }

    public async Task<int> Likes(int authorId, int cheepId)
    {
        //var hasLiked = await HasUserLiked(authorId, cheepId);
        var AmountOfLikes = await _repository.Likes(authorId, cheepId, true);
        //int AmountOfLikes;
        // if(likeStored){
        //     AmountOfLikes = await UpdateCheep(cheepId, null, hasLiked);
        // } else
        // {
        //     AmountOfLikes = 0;
        // }
        return AmountOfLikes;
    }

    public async Task<bool> HasUserLiked(int authorId, int cheepId)
    {
       return await _repository.OpinionExist(authorId, cheepId);
    }

    /// <include file="../../docs/CheepServiceDocs.xml" path="/doc/members/member[@name='M:CheepService.CheepListToCheepDTOList(System.Collections.Generic.List{Cheep})']/*" />
    public static List<CheepDTO> CheepListToCheepDTOList(List<Cheep> cheeps) //Do we even use this functions anymore??? The repository also has this logic
    {
        var modelMessages = new List<CheepDTO>();
        foreach (var cheep in cheeps)
        {
            var modelCheep = new CheepDTO()
            {
                CheepId = 0,
                Author = cheep.Author.Name,
                Message = cheep.Text,
                Timestamp = DateTimeToDateTimeString(cheep.TimeStamp),
                LikeCounter = 0
            };
            modelMessages.Add(modelCheep);
        }

        return modelMessages;
    }

    /// <include file="../../docs/CheepServiceDocs.xml" path="/doc/members/member[@name='M:CheepService.DateTimeToDateTimeString(System.DateTime)']/*" />
    public static string DateTimeToDateTimeString(DateTime dateTime)
    {
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

     public void PostCheep(Author author, string message)
    {
        Cheep cheep = new Cheep {
            AuthorId = author.Id,
            Author = author,
            Text = message,
            TimeStamp = DateTime.Now
        };
        _repository.CreateMessage(cheep);
    }
}
