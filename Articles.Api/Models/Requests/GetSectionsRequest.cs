namespace Articles.Api.Models.Requests;

public class GetSectionsRequest
{
    public int StartRownum { get; set; } = 0;
    
    public int RowCount { get; set; } = 10;
}