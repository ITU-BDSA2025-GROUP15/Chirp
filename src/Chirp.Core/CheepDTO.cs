/// <include file="../../docs/CheepDTODocs.xml" path="/doc/members/member[@name='T:CheepDTO']/*" />
public class CheepDTO
{
    /// <include file="../../docs/CheepDTODocs.xml" path="/doc/members/member[@name='P:CheepDTO.Author']/*" />
    public required string Author { get; set; }

    /// <include file="../../docs/CheepDTODocs.xml" path="/doc/members/member[@name='P:CheepDTO.Message']/*" />
    public required string Message { get; set; }

    /// <include file="../../docs/CheepDTODocs.xml" path="/doc/members/member[@name='P:CheepDTO.Timestamp']/*" />
    public required string Timestamp { get; set; }

    public required int like_counter { get; set; }
}
