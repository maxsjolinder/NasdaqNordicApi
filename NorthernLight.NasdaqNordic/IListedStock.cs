namespace NorthernLight.NasdaqNordic
{
    public enum Segment
    {
        Large,
        Mid,
        Small,
        Premier
    }

    public interface IListedStock
    {
        string Name { get; }
        string Symbol { get; }
        string Currency { get; }
        string ISIN { get; }
        Segment Segment { get; }
        string Sector { get; }
        string SectorCode { get; }
        string FactSheetUrl { get; }
    }
}
