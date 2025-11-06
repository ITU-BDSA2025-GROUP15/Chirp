public interface ICheepService 
{
    public List<CheepDTO> GetCheeps();
    public List<CheepDTO> GetCheeps(int page);
    public List<CheepDTO> GetCheepsFromAuthor(string author);
    public List<CheepDTO> GetCheepsFromAuthor(string author, int page);
    public void PostCheep(Author author, string message);
}