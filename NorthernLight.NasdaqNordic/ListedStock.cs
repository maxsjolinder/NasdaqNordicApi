namespace NorthernLight.NasdaqNordic
{
    public class ListedStock : IListedStock
    {
        public string Name { get; set; }

        public string Symbol { get; set; }

        public string Currency { get; set; }

        public string ISIN { get; set; }

        public Segment Segment { get; set; }

        public string Sector { get; set; }

        public string SectorCode { get; set; }

        public string FactSheetUrl { get; set; }

        public string NasdaqInstrumentId { get; set; }
    }
}
