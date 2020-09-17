using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiagnosticSourceSample
{
    class Program
    {
        private static async Task Main()
        {
            using var subscription = DiagnosticListener.AllListeners.Subscribe(new HttpCoreDiagnosticSourceListener());
            await new HttpClient().GetAsync("https://example.com");
        }
    }
}