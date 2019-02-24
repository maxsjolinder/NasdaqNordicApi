using System.Threading.Tasks;

namespace NorthernLight.NasdaqNordic.HttpClient
{
    internal class HttpClient : IHttpClient
    {
        private static readonly System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();

        public Task<string> GetStringAsync(string url)
        {
            return httpClient.GetStringAsync(url);
        }
    }
}
