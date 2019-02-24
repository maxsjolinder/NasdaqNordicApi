using System;

namespace NorthernLight.NasdaqNordic.Parser
{
    public class ListedStockParserException : Exception
    {
        public ListedStockParserException(string message) : base(message)
        {
        }

        public ListedStockParserException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
