using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.Text;
using Microsoft.Diagnostics.NETCore.Client;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Parsers.Clr;

namespace MicrosoftDiagnosticsClientSample
{
    internal static class Program
    {
        // from https://docs.microsoft.com/en-us/dotnet/framework/performance/garbage-collection-etw-events
        private const int GCStart = 1;

        private static void Main()
        {
            Console.Write("Enter process id: ");
            var processId = int.Parse(Console.ReadLine() ?? string.Empty);
            PrintRuntimeGCEvents(processId);
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        private static void PrintRuntimeGCEvents(int processId)
        {
            var providers = new List<EventPipeProvider>
            {
                new EventPipeProvider("Microsoft-Windows-DotNETRuntime", EventLevel.Informational,
                    (long) ClrTraceEventParser.Keywords.GC)
            };

            var client = new DiagnosticsClient(processId);
            using var session = client.StartEventPipeSession(providers, false);
            var source = new EventPipeEventSource(session.EventStream);

            source.Clr.All += traceEvent =>
            {
                // Console.WriteLine(traceEvent.EventName);

                // A garbage collection has started.
                if (traceEvent.ID == (TraceEventID) GCStart)
                {
                    var timestamp = traceEvent.TimeStamp.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture);
                    var sb = new StringBuilder($"{timestamp}: GC was triggered for Gen#");
                    
                    var generation = (int) traceEvent.PayloadByName("Depth");
                    var gcType = (GCType) traceEvent.PayloadByName("Type");
                    var gcReason = (GCReason) traceEvent.PayloadByName("Reason");

                    sb.Append($"{generation}, GCType: {gcType}, Reason: {gcReason}");

                    Console.WriteLine(sb.ToString());
                    Console.WriteLine();
                }
            };

            try
            {
                source.Process();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error encountered while processing events");
                Console.WriteLine(e.ToString());
            }
        }
    }
}