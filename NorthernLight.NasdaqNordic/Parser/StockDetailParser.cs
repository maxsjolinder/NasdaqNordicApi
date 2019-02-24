using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NorthernLight.NasdaqNordic.HttpClient;
using System;
using System.Threading.Tasks;

namespace NorthernLight.NasdaqNordic.Parser
{
    internal class StockDetailParser : IStockDetailParser
    {
        private const string StockDetailsUrl = "http://www.nasdaqomxnordic.com/webproxy/DataFeedProxy.aspx?SubSystem=Prices&Action=GetInstrument&Source=OMX&Instrument={0}&inst.an=nm,fnm,isin,ts,ltp,ltd,ch,chp,cr,hp,lp,op,tv,not,smv,nos,hlp,sectid,sectname,mkt,lists,notetxt,issuer,pmkt,dft,co,mn,mktc&inst.e=3,13&issuer.e=4&issuer.a=14&DefaultDecimals=false&json=1&app=/shares/microsite-ShareInformation";
        private readonly IHttpClient httpClient;

        internal StockDetailParser(IHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<StockDetails> GetStockDetailsAsync(string nasdaqStockId)
        {
            if (nasdaqStockId.Trim() == string.Empty)
            {
                throw new ArgumentException($"Illegal Nasdaq stock id \"{nasdaqStockId}\".");
            }

            try
            {
                var content = await httpClient.GetStringAsync(string.Format(StockDetailsUrl, nasdaqStockId));
                dynamic jsonStockData = JObject.Parse(content);
                return new StockDetails(jsonStockData, nasdaqStockId);
            }
            catch (JsonReaderException e)
            {
                throw new ListedStockParserException($"Unable to parse stock details json data for Nasdaq stock id {nasdaqStockId}", e);
            }
        }
    }
}
