using NorthernLight.NasdaqNordic.Parser;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NorthernLight.NasdaqNordic
{
    public class NasdaqNordic : INasdaqNordic
    {
        private readonly IListedStockParser listedStockParser;
        private readonly IStockDetailParser stockDetailParser;

        public NasdaqNordic()
        {
            stockDetailParser = new StockDetailParser(new HttpClient.HttpClient());
            listedStockParser = new ListedStockParser(new HttpClient.HttpClient(), stockDetailParser);
        }

        public Task<IList<IListedStock>> GetListedStockholmStocksAsync()
        {
            return listedStockParser.GetListedStockholmStocksAsync();
        }
    }
}
