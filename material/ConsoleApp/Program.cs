using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://ip172-18-0-6-c5kclavnjsv000civrng-5001.direct.labs.play-with-docker.com");

            var json = await httpClient.GetStringAsync("/api/v1/products");

            Console.WriteLine(json);
            Console.WriteLine("========> Hello World!");
        }
    }
}
