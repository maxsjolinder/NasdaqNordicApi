namespace NorthernLight.NasdaqNordic.Parser
{
    internal static class ParserUtil
    {
        // TODO add test
        internal static string ParseNasdaqInstrumentIdFromUrlString(string url)
        {
            // "/shares/microsite?Instrument=SSE36273&symbol=AAK&name=AAK"
            var instrument = "Instrument=";
            var startindex = url.IndexOf(instrument) + instrument.Length;
            var stopIndex = url.IndexOf("&");

            return url.Substring(startindex, (stopIndex - startindex));
        }
    }
}
