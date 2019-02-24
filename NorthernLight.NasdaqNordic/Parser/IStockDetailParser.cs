using System.Threading.Tasks;

namespace NorthernLight.NasdaqNordic.Parser
{
    internal interface IStockDetailParser
    {
        Task<StockDetails> GetStockDetailsAsync(string nasdaqStockId);
    }
}
