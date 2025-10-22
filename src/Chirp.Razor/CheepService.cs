using Chirp.Razor;

//public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepDTO> GetCheeps();
    public List<CheepDTO> GetCheeps(int page);
    public List<CheepDTO> GetCheepsFromAuthor(string author);
    public List<CheepDTO> GetCheepsFromAuthor(string author, int page);
}

public class CheepService : ICheepService
{
    private readonly ICheepRepository _repository;

    public CheepService(ICheepRepository repository)
    {
        _repository = repository;
    }
    public List<CheepDTO> GetCheeps()
    {
        var messages = _repository.ReadMessages(null, null, null);
        return messages.GetAwaiter().GetResult();
    }

    public List<CheepDTO> GetCheeps(int page)
    {
        var messages = _repository.ReadMessages(null, page, null);
        return messages.GetAwaiter().GetResult();
    }

    public List<CheepDTO> GetCheepsFromAuthor(string author)
    {
        var messages = _repository.ReadMessages(author, null, null);
        return messages.GetAwaiter().GetResult();
    }

    public List<CheepDTO> GetCheepsFromAuthor(string author, int page)
    {
        var messages = _repository.ReadMessages(author, page, null);
        return messages.GetAwaiter().GetResult();
    }

    /// <summary>
    /// Converts a list of Cheeps into a list of corresponding CheepDTOs.
    /// </summary>
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

    /// <summary>
    /// Converts a DateTime object into a formatted string, in the format MM/dd/yy H:mm:ss.
    /// </summary>
    /// <returns>A formatted date/time string, i.e. 12/31/25 9:41:23.</returns>
    public static string DateTimeToDateTimeString(DateTime dateTime)
    {
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
}
