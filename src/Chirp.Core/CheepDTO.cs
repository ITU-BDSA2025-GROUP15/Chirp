/// <include file="../../docs/CheepDTODocs.xml" path="/doc/members/member[@name='T:CheepDTO']/*" />
public class CheepDTO
{
    public required int CheepId { get; set; }
    /// <include file="../../docs/CheepDTODocs.xml" path="/doc/members/member[@name='P:CheepDTO.Author']/*" />
    public required string Author { get; set; }

    /// <include file="../../docs/CheepDTODocs.xml" path="/doc/members/member[@name='P:CheepDTO.Message']/*" />
    public required string Message { get; set; }

    /// <include file="../../docs/CheepDTODocs.xml" path="/doc/members/member[@name='P:CheepDTO.Timestamp']/*" />
    public required string Timestamp { get; set; }

    public required int LikeCounter { get; set; }

    public bool UserHasLiked { get; set; }
}
