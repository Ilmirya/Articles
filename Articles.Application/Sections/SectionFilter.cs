namespace Articles.Application.Sections;

public sealed class SectionFilter
{
    public int StartRownum { get; }

    public int RowCount { get; }
    
    public SectionFilter(int startRownum, int rowCount)
    {
        StartRownum = startRownum;
        RowCount = rowCount;
    }
}