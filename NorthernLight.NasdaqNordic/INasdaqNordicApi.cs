using System.Collections.Generic;
using System.Threading.Tasks;

namespace NorthernLight.NasdaqNordic
{
    public interface INasdaqNordicApi
    {
        Task<IList<IListedStock>> GetListedStockholmStocksAsync();
    }
}
