using System.Collections.Generic;
using System.Threading.Tasks;

namespace NorthernLight.NasdaqNordic.Parser
{
    internal interface IListedStockParser
    {
        Task<IList<IListedStock>> GetListedStockholmStocksAsync();
    }
}
