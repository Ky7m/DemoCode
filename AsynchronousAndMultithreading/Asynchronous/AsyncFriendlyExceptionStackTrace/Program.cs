using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AsyncFriendlyStackTrace;

namespace AsyncFriendlyExceptionStackTrace
{
    class Program
    {
        static async Task Main()
        {
            var sb = new StringBuilder();
            try
            {
                await new SimpleAsyncMethodChain().Run();
                //await new AsyncInvocationsWithWait().Run();
            }
            catch (Exception e)
            {
                sb.AppendLine("Usual");
                sb.AppendLine("---");
                sb.AppendLine("```");
                sb.AppendLine(e.ToString());
                sb.AppendLine("```");
                sb.AppendLine();

                sb.AppendLine("AsyncFriendlyStackTrace");
                sb.AppendLine("---");
                sb.AppendLine("```");
                sb.AppendLine(e.ToAsyncString());
                sb.AppendLine("```");
                sb.AppendLine();

                sb.AppendLine("Ben.Demystifier");
                sb.AppendLine("---");
                sb.AppendLine("```");
                sb.AppendLine(e.ToStringDemystified());
                sb.AppendLine("```");
                sb.AppendLine();
            }
            finally
            {
                await File.WriteAllTextAsync("result.txt", sb.ToString());
            }
        }
    }
}