static public class UserInterface
{
    public static void PrintCheeps(IEnumerable<Cheep> cheeps)
    {
        foreach (var cheep in cheeps)
        {
            var localTime = ToLocalTimeString(cheep.Timestamp);
            Console.WriteLine(cheep.Author + " @ " + localTime + ": " + cheep.Message);
        }
    }

    public static string ToLocalTimeString(long timestamp)
    {
        var t = DateTimeOffset.FromUnixTimeSeconds(timestamp);
            t = TimeZoneInfo.ConvertTime(t, TimeZoneInfo.Local);
        return t.ToString("MM/dd/yy HH:mm:ss");
    }
}
    
