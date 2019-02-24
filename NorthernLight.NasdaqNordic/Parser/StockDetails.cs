using Microsoft.CSharp.RuntimeBinder;

namespace NorthernLight.NasdaqNordic.Parser
{
    internal class StockDetails
    {
        private readonly dynamic jsonStockData;
        private readonly string nasdaqStockId;

        internal StockDetails(dynamic jsonStockData, string nasdaqStockId)
        {
            this.jsonStockData = jsonStockData;
            this.nasdaqStockId = nasdaqStockId;
        }

        internal Segment GetSegment()
        {
            try
            {
                string lists = jsonStockData["inst"]["@lists"];
                if (lists.Contains("Large Cap"))
                    return Segment.Large;
                if (lists.Contains("Mid Cap"))
                    return Segment.Mid;
                if (lists.Contains("Small Cap"))
                    return Segment.Small;
                if (lists.Contains("Premier"))
                    return Segment.Premier;
            }
            catch (RuntimeBinderException e)
            {
                throw new ListedStockParserException($"Unable to parse segment information from Stock with Nasdaq stock id {nasdaqStockId}.", e);
            }
            throw new ListedStockParserException($"Unable to parse segment information from Stock with Nasdaq stock id {nasdaqStockId}.");
        }
    }
}
