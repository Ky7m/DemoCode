using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace UnderstandingAsync
{
    class Program
    {
        static async Task Main()
        {
            var stringData = await GetDataFromRemoteService();
            await Console.Out.WriteLineAsync(stringData);
        }

        private static async Task<string> GetDataFromRemoteService()
        {
            const string url = "https://www.ifesenko.com/humans.txt";

            using (var httpClient = new HttpClient())
            using (var response = await httpClient.GetAsync(url))
            using (var content = response.Content)
            {
                var data = await content.ReadAsStringAsync();
                return data ?? string.Empty;
            }
        }
    }
}