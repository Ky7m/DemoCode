using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiagnosticSourceSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var subscription = DiagnosticListener.AllListeners.Subscribe(new HttpCoreDiagnosticSourceListener());
            await new HttpClient().GetAsync("https://example.com");
        }
    }
}