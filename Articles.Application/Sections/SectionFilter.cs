namespace Articles.Application.Sections;

public sealed class SectionFilter(int startRownum, int rowCount)
{
    public int StartRownum { get; } = startRownum;

    public int RowCount { get; } = rowCount;
}