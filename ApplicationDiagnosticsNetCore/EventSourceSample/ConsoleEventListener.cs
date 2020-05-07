using System;
using System.Diagnostics.Tracing;
using System.Linq;

namespace EventSourceSample
{
    public class ConsoleEventListener : EventListener
    {
        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            EnableEvents(eventSource, EventLevel.LogAlways, EventKeywords.All);
        }
        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            // report all event information
            Console.Write("  Event {0} ", eventData.EventName);
            
            // Events can have formatting strings 'the Message property on the 'Event' attribute.  
            // If the event has a formatted message, print that, otherwise print out argument values.  
            if (eventData.Message != null)
                Console.WriteLine(eventData.Message, eventData.Payload.ToArray());
            else
            {
                var sargs = eventData.Payload?.Select(o => o.ToString()).ToArray();
                Console.WriteLine("({0}).", sargs != null ? string.Join(", ", sargs) : "");
            }
        }
    }
}