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
    public List<CheepDTO> GetCheeps()
    {
        var messages = DBFacade.ReadMessages();
        return CheepListToCheepDTOList(messages);
    }

    public List<CheepDTO> GetCheeps(int page)
    {
        var messages = DBFacade.ReadMessages(page);
        return CheepListToCheepDTOList(messages);
    }

    public List<CheepDTO> GetCheepsFromAuthor(string author)
    {
        var messages = DBFacade.ReadMessages(author);
        return CheepListToCheepDTOList(messages);
    }
    
    public List<CheepDTO> GetCheepsFromAuthor(string author, int page)
    {
        var messages = DBFacade.ReadMessages(author, page);
        return CheepListToCheepDTOList(messages);
    }

    public static List<CheepDTO> CheepListToCheepDTOList(List<Cheep> cheeps)
    {
        var modelMessages = new List<CheepDTO>();

        foreach (var cheep in cheeps)
        {
            var modelCheep = new CheepDTO() {
                Author = cheep.Author.Name,
                Message = cheep.Text,
                Timestamp = CheepService.UnixTimeStampToDateTimeString(((DateTimeOffset)cheep.TimeStamp).ToUnixTimeSeconds())
            };
            modelMessages.Add(modelCheep);
        }

        return modelMessages;
    }

    public static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
}
