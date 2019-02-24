using System.Threading.Tasks;

namespace NorthernLight.NasdaqNordic.HttpClient
{
    internal interface IHttpClient
    {
        Task<string> GetStringAsync(string url);
    }
}
