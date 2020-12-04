using HtmlAgilityPack;
using NorthernLight.NasdaqNordic.HttpClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace NorthernLight.NasdaqNordic.Parser
{
    internal class ListedStockParser : IListedStockParser
    {
        private const string ListedSthlmStocksUrl = "http://www.nasdaqomxnordic.com/aktier/listed-companies/stockholm";
        private const int MaxDegreeOfParallelism = 15;

        private readonly IHttpClient httpClient;
        private readonly IStockDetailParser stockDetailParser;
        private Dictionary<string, int> titleToTitleIndexDict;

        internal ListedStockParser(IHttpClient httpClient, IStockDetailParser stockDetailParser)
        {
            this.httpClient = httpClient;
            this.stockDetailParser = stockDetailParser;
        }

        public async Task<IList<IListedStock>> GetListedStockholmStocksAsync()
        {
            var content = await httpClient.GetStringAsync(ListedSthlmStocksUrl);
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);
            return await ParseListedStocksAsync(htmlDoc);
        }

        private async Task<List<IListedStock>> ParseListedStocksAsync(HtmlDocument doc)
        {
            titleToTitleIndexDict = GetTitleToTitleIndexMappings(doc);
            var listedStockTableRows = GetListedCompaniesTableDataRows(doc);
            return await ParseStockAndPopulateStockDetailsAsync(listedStockTableRows);
        }

        private async Task<List<IListedStock>> ParseStockAndPopulateStockDetailsAsync(IEnumerable<HtmlNode> listedStockTableRows)
        {
            // Use a semaphore to limit the number of concurrent network connections
            var tasks = new List<Task<IListedStock>>();
            using (SemaphoreSlim semaphore = new SemaphoreSlim(MaxDegreeOfParallelism))
            {
                foreach (var listedStockNode in listedStockTableRows)
                {
                    var task = Task.Run(async () =>
                    {
                        await semaphore.WaitAsync();
                        try
                        {
                            var stock = ParseStock(listedStockNode);
                            await PopulateStockDetailInformationAsync(stock);
                            return stock as IListedStock;
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    });

                    tasks.Add(task);
                }
                await Task.WhenAll(tasks);
            }

            return tasks.Select(x => x.Result).ToList();
        }

        private static IEnumerable<HtmlNode> GetListedCompaniesTableDataRows(HtmlDocument doc)
        {
            var listedCompaniesTableNode = doc.GetElementbyId("listedCompanies");
            var tableDataRows = listedCompaniesTableNode?.ChildNodes.FirstOrDefault(x => x.Name == "tbody")?.ChildNodes.Where(x => x.Name == "tr");
            if (tableDataRows == null)
            {
                throw new ListedStockParserException("Unable to parse listed stock table data.");
            }
            return tableDataRows;
        }

        private async Task PopulateStockDetailInformationAsync(ListedStock listedStock)
        {
            var stockDetails = await stockDetailParser.GetStockDetailsAsync(listedStock.NasdaqInstrumentId);
            listedStock.Segment = stockDetails.GetSegment();
        }

        private ListedStock ParseStock(HtmlNode htmlNode)
        {
            var columnValues = htmlNode.ChildNodes.Where(x => x.Name == "td").ToList();
            return new ListedStock
            {
                Name = GetFirstChildInnerText("Name", columnValues),
                Symbol = GetFirstChildInnerText("Symbol", columnValues),
                Currency = GetFirstChildInnerText("Currency", columnValues),
                ISIN = GetFirstChildInnerText("ISIN", columnValues),
                Sector = GetFirstChildAttribute("Sector", "title", columnValues),
                SectorCode = GetFirstChildInnerText("Sector Code", columnValues),
                FactSheetUrl = GetFirstChildsHrefAttributeValue("Fact Sheet", columnValues),
                NasdaqInstrumentId = ParserUtil.ParseNasdaqInstrumentIdFromUrlString(GetFirstChildsHrefAttributeValue("Name", columnValues))
            };
        }

        private HtmlNode GetColumnNode(string columnTitle, List<HtmlNode> columnValues)
        {
            var index = GetDataColumnIndex(columnTitle);
            return columnValues[index];
        }

        private string GetFirstChildInnerText(string columnTitle, List<HtmlNode> columnValues)
        {
            var node = GetColumnNode(columnTitle, columnValues);
            var text = node.ChildNodes.FirstOrDefault()?.InnerText ?? "";
            return HtmlDecode(text);
        }

        private string GetFirstChildAttribute(string columnTitle, string attribute, List<HtmlNode> columnValues)
        {
            var node = GetColumnNode(columnTitle, columnValues);
            var text = node.Attributes.FirstOrDefault(x => x.Name == attribute)?.Value ?? "";
            return HtmlDecode(text);
        }

        private string GetFirstChildsHrefAttributeValue(string columnTitle, List<HtmlNode> columnValues)
        {
            var node = GetColumnNode(columnTitle, columnValues);
            var text = node.ChildNodes.FirstOrDefault()?.Attributes.FirstOrDefault(x => x.Name == "href")?.Value ?? "";
            return HtmlDecode(text);
        }

        private int GetDataColumnIndex(string columnTitle)
        {
            if (!titleToTitleIndexDict.TryGetValue(columnTitle, out int index))
            {
                throw new ListedStockParserException($"Unable to parse listed stock table data column {columnTitle}.");
            }
            return index;
        }

        private Dictionary<string, int> GetTitleToTitleIndexMappings(HtmlDocument doc)
        {
            var listedCompaniesTable = doc.GetElementbyId("listedCompanies");
            var headNode = listedCompaniesTable?.ChildNodes.FirstOrDefault(x => x.Name == "thead");
            var headElements = headNode?.ChildNodes.FirstOrDefault(x => x.Name == "tr")?.ChildNodes.Where(x => x.Name == "th");
            var headerTitleValues = headElements?.Select(x =>
            {
                var attr = x.Attributes.Where(a => a.Name == "title").FirstOrDefault();
                return attr.Value;
            }).ToList();

            if (headerTitleValues == null)
            {
                throw new ListedStockParserException("Unable to parse listed stock titles.");
            }

            Dictionary<string, int> titleToIndexDict = new Dictionary<string, int>();
            for (int i = 0; i < headerTitleValues.Count; i++)
            {
                titleToIndexDict.Add(headerTitleValues[i], i);
            }

            return titleToIndexDict;
        }

        private string HtmlDecode(string str)
        {
            return HttpUtility.HtmlDecode(str);
        }
    }
}