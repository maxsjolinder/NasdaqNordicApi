﻿using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NorthernLight.NasdaqNordic.HttpClient
{
    internal class HttpClient : IHttpClient
    {
        private static readonly System.Net.Http.HttpClient httpClient = CreateHttpClient();

        public Task<string> GetStringAsync(string url)
        {
            return httpClient.GetStringAsync(url);
        }

        private static System.Net.Http.HttpClient CreateHttpClient()
        {
            var httpClientHandler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var client = new System.Net.Http.HttpClient(httpClientHandler);
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.183 Safari/537.36");
            client.DefaultRequestHeaders.Add("Host", "www.nasdaqomxnordic.com");
            client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
            client.DefaultRequestHeaders.Add("Accept-Language", "en-US");

            return client;
        }
    }
}
