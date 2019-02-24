﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace NorthernLight.NasdaqNordic.Parser
{
    public interface IListedStockParser
    {
        Task<IList<IListedStock>> GetListedStockholmStocksAsync();
    }
}
