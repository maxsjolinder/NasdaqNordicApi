using System;

namespace NorthernLight.NasdaqNordic
{
    internal class ListedStock : IListedStock, IEquatable<ListedStock>
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

        public bool Equals(ListedStock other)
        {
            if (other == null)
                return false;

            return
                 this.ISIN == other.ISIN &&
                 this.Name == other.Name &&
                 this.Symbol == other.Symbol &&
                 this.Currency == other.Currency &&
                 this.Segment == other.Segment &&
                 this.Sector == other.Sector &&
                 this.SectorCode == other.SectorCode &&
                 this.FactSheetUrl == other.FactSheetUrl &&
                 this.NasdaqInstrumentId == other.NasdaqInstrumentId;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
                return false;

            ListedStock stock = obj as ListedStock;
            if (stock == null)
                return false;
            else
                return Equals(stock);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + ISIN.GetHashCode();
                hash = hash * 23 + Name.GetHashCode();
                hash = hash * 23 + Symbol.GetHashCode();
                hash = hash * 23 + Currency?.GetHashCode() ?? 0;
                hash = hash * 23 + Segment.GetHashCode();
                hash = hash * 23 + Sector?.GetHashCode() ?? 0;
                hash = hash * 23 + SectorCode?.GetHashCode() ?? 0;
                hash = hash * 23 + FactSheetUrl?.GetHashCode() ?? 0;
                hash = hash * 23 + NasdaqInstrumentId.GetHashCode();

                return hash;
            }
        }
    }
}
