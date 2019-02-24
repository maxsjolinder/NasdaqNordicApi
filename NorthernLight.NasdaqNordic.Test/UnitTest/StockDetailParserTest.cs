using NorthernLight.NasdaqNordic.HttpClient;
using NorthernLight.NasdaqNordic.Parser;
using NorthernLight.NasdaqNordic.Test.Utils;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;


namespace NorthernLight.NasdaqNordic.Test.UnitTest
{
    public class StockDetailParserTest
    {
        private IStockDetailParser stockDetailParser;
        private IHttpClient httpClient;

        public StockDetailParserTest()
        {
            httpClient = Substitute.For<IHttpClient>();
            stockDetailParser = new StockDetailParser(httpClient);

        }

        [Fact]
        public async Task LargeCapStockSegmentTest()
        {
            var scaDetails = EmbeddedResourceUtil.ReadFileContent("sca_b_details.json");
            httpClient.GetStringAsync(Arg.Any<string>()).Returns(scaDetails);
            var stockDetails = await stockDetailParser.GetStockDetailsAsync("dummy stock id");
            Assert.Equal(Segment.Large, stockDetails.GetSegment());
        }

        [Fact]
        public async Task MidCapStockSegmentTest()
        {
            var addnodeDetails = EmbeddedResourceUtil.ReadFileContent("addnode_b_details.json");
            httpClient.GetStringAsync(Arg.Any<string>()).Returns(addnodeDetails);
            var stockDetails = await stockDetailParser.GetStockDetailsAsync("dummy stock id");
            Assert.Equal(Segment.Mid, stockDetails.GetSegment());
        }

        [Fact]
        public async Task SmallCapStockSegmentTest()
        {
            var msabDetails = EmbeddedResourceUtil.ReadFileContent("msab_b_details.json");
            httpClient.GetStringAsync(Arg.Any<string>()).Returns(msabDetails);
            var stockDetails = await stockDetailParser.GetStockDetailsAsync("dummy stock id");
            Assert.Equal(Segment.Small, stockDetails.GetSegment());
        }

        [Fact]
        public async Task EmptyJsonResponseTest()
        {
            httpClient.GetStringAsync(Arg.Any<string>()).Returns("{}");
            var stockDetails = await stockDetailParser.GetStockDetailsAsync("dummy stock id");
            Exception ex = Assert.Throws<ListedStockParserException>(() => stockDetails.GetSegment());
            Assert.Contains("Unable to parse segment information", ex.Message);
            Assert.Contains("dummy stock id", ex.Message);
            Assert.NotNull(ex.InnerException);
        }
        
        [Fact]
        public async Task UnknownStockSegmentTest()
        {
            var jsonWithoutSegment = "{\"inst\": {\"@lists\": \"nonsense\"}}";
            httpClient.GetStringAsync(Arg.Any<string>()).Returns(jsonWithoutSegment);
            var stockDetails = await stockDetailParser.GetStockDetailsAsync("dummy stock id");
            Exception ex = Assert.Throws<ListedStockParserException>(() => stockDetails.GetSegment());
            Assert.Contains("Unable to parse segment information", ex.Message);
            Assert.Contains("dummy stock id", ex.Message);
            Assert.Null(ex.InnerException);
        }

        [Fact]
        public async Task EmptyHttpResponseTest()
        {
            httpClient.GetStringAsync(Arg.Any<string>()).Returns("");
            Exception ex = await Assert.ThrowsAsync<ListedStockParserException>(async () => await stockDetailParser.GetStockDetailsAsync("dummy stock id"));
            Assert.Contains("Unable to parse stock details json", ex.Message);
            Assert.Contains("dummy stock id", ex.Message);
        }
    }
}
