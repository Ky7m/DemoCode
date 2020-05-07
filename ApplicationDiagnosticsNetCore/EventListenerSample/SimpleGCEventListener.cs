using System;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.Text;

namespace EventListenerSample
{
    internal sealed class SimpleGCEventListener : EventListener
    {
        // from https://docs.microsoft.com/en-us/dotnet/framework/performance/clr-etw-keywords-and-levels
        private const int GCKeyword = 0x0000001;

        // Called whenever an EventSource is created.
        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            Console.WriteLine($"{eventSource.Guid} | {eventSource.Name}");
            // Watch for the .NET runtime EventSource 
            if (eventSource.Name.Equals("Microsoft-Windows-DotNETRuntime"))
            {
                // and collect information pertaining to garbage collection.
                EnableEvents(eventSource, EventLevel.Informational, (EventKeywords) GCKeyword);
            }
        }

        // Called whenever an event is written.
        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            // A garbage collection has started.
            if (eventData.EventName == "GCStart_V1" || eventData.EventName == "GCStart_V2")
            {
                var timestamp = eventData.TimeStamp.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture);
                var sb = new StringBuilder($"{timestamp}: GC was triggered for Gen#");

                for (var i = 0; i < eventData.Payload.Count; i++)
                {
                    var payloadValue = eventData.Payload[i];
                    if (payloadValue is null)
                    {
                        continue;
                    }

                    if (eventData.PayloadNames[i] == "Depth") // the generation that is being collected
                    {
                        sb.Append($"{payloadValue}. Reason: ");
                    }

                    if (eventData.PayloadNames[i] == "Reason") // why the garbage collection was triggered
                    {
                        switch (Convert.ToUInt32(payloadValue))
                        {
                            case 0x0:
                                sb.Append("Small object heap allocation.");
                                break;
                            case 0x1:
                                sb.Append("Induced.");
                                break;
                            case 0x2:
                                sb.Append("Low memory.");
                                break;
                            case 0x3:
                                sb.Append("Empty.");
                                break;
                            case 0x4:
                                sb.Append("Large object heap allocation.");
                                break;
                            case 0x5:
                                sb.Append("Out of space (for small object heap).");
                                break;
                            case 0x6:
                                sb.Append("Out of space (for large object heap).");
                                break;
                            case 0x7:
                                sb.Append("Induced but not forced as blocking.");
                                break;
                        }
                    }
                }

                Console.WriteLine(sb.ToString());
                Console.WriteLine();
            }
        }
    }
}