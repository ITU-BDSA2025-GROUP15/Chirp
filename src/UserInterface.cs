static public class UserInterface{
    public static void PrintCheeps(IEnumerable<Cheep> cheeps){
        foreach (var cheep in cheeps)
        {
            var timestamp = DateTimeOffset.FromUnixTimeSeconds(cheep.Timestamp);
            timestamp = TimeZoneInfo.ConvertTime(timestamp, TimeZoneInfo.Local);
            Console.WriteLine(cheep.Author + " @ " + timestamp.ToString("MM/dd/yy HH:mm:ss") + ": " + cheep.Message);
        }
    }
}
    
