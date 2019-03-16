using NorthernLight.NasdaqNordic.Parser;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NorthernLight.NasdaqNordic
{
    public class NasdaqNordicApi : INasdaqNordicApi
    {
        private readonly IListedStockParser listedStockParser;
        private readonly IStockDetailParser stockDetailParser;

        public NasdaqNordicApi()
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
