using System.Collections.Generic;
using System.Threading.Tasks;

namespace NorthernLight.NasdaqNordic
{
    public interface INasdaqNordic
    {
        Task<IList<IListedStock>> GetListedStockholmStocksAsync();
    }
}
