using System;
using System.Linq;
using System.Threading.Tasks;

namespace NorthernLight.NasdaqNordic.ConsoleApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            INasdaqNordicApi nasdaqApi = new NasdaqNordicApi();
            var listedStocks = await nasdaqApi.GetListedStockholmStocksAsync();
            
            watch.Stop();
            var elapsedSecs = watch.Elapsed.TotalSeconds;
            Console.WriteLine($"Operation took {elapsedSecs} seconds!");

            Console.WriteLine("Listed small cap stocks:");
            foreach (var listedStock in listedStocks.Where(x => x.Segment == Segment.Small))
            {
                Console.WriteLine(listedStock.Name);
            }                        
            Console.ReadLine();
        }
    }
}
