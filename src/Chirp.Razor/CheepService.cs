using Chirp.Razor;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps();
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    // These would normally be loaded from a database for example
    private static readonly List<CheepViewModel> _cheeps = new()
        {
            new CheepViewModel("Helge", "Hello, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Adrian", "Hej, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
        };

    public List<CheepViewModel> GetCheeps()
    {
        var messages = DBFacade.ReadMessages();
        return CheepListToCheepViewModelList(messages);
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        var messages = DBFacade.ReadMessages(author);
        return CheepListToCheepViewModelList(messages);
    }

    private static List<CheepViewModel> CheepListToCheepViewModelList(List<Cheep> cheeps)
    {
        var modelMessages = new List<CheepViewModel>();
    
        foreach (var cheep in cheeps)
        {
            var modelCheep = new CheepViewModel(
                cheep.author,
                cheep.message,
                UnixTimeStampToDateTimeString(cheep.timestamp)
            );
            modelMessages.Add(modelCheep);
        }

        return modelMessages;
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
}
