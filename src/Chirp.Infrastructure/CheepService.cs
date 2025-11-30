//public record CheepViewModel(string Author, string Message, string Timestamp);

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
        var messages = _repository.ReadMessages([], null, null);
        return messages.GetAwaiter().GetResult();
    }

    /// <include file="../../docs/CheepServiceDocs.xml" path="/doc/members/member[@name='M:CheepService.GetCheeps(System.Int32)']/*" />
    public List<CheepDTO> GetCheeps(int page)
    {
        var messages = _repository.ReadMessages([], page, null);
        return messages.GetAwaiter().GetResult();
    }

    /// <include file="../../docs/CheepServiceDocs.xml" path="/doc/members/member[@name='M:CheepService.GetCheepsFromAuthor(System.String)']/*" />
    public List<CheepDTO> GetCheepsFromAuthor(string author)
    {
        var messages = _repository.ReadMessages(author, null, null);
        return messages.GetAwaiter().GetResult();
    }

    /// <include file="../../docs/CheepServiceDocs.xml" path="/doc/members/member[@name='M:CheepService.GetCheepsFromAuthor(System.String,System.Int32)']/*" />
    public List<CheepDTO> GetCheepsFromAuthor(string author, int page)
    {
        var messages = _repository.ReadMessages(author, page, null);
        return messages.GetAwaiter().GetResult();
    }

    public List<CheepDTO> GetCheepsFromAuthors(string[] author, int page)
    {
        var messages = _repository.ReadMessages(author, page, null);
        return messages.GetAwaiter().GetResult();
    }

    /// <include file="../../docs/CheepServiceDocs.xml" path="/doc/members/member[@name='M:CheepService.CheepListToCheepDTOList(System.Collections.Generic.List{Cheep})']/*" />
    public static List<CheepDTO> CheepListToCheepDTOList(List<Cheep> cheeps)
    {
        var modelMessages = new List<CheepDTO>();

        foreach (var cheep in cheeps)
        {
            var modelCheep = new CheepDTO()
            {
                Author = cheep.Author.Name,
                Message = cheep.Text,
                Timestamp = DateTimeToDateTimeString(cheep.TimeStamp)
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
