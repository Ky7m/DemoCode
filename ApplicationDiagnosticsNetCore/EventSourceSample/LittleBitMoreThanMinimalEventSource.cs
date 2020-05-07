using System.Diagnostics.Tracing;

namespace EventSourceSample
{
    [EventSource(Name = "ApplicationDiagnosticsNetCore-LittleBitMoreThanMinimalEventSource")]
    public class LittleBitMoreThanMinimalEventSource : EventSource
    {
        static public LittleBitMoreThanMinimalEventSource Log = new LittleBitMoreThanMinimalEventSource();
        
        /// <summary>
        /// By defining keywords, we can turn on events independently.   Because we defined the 'Request'
        /// and 'Debug' keywords and assigned the 'Request' keywords to the first three events, these 
        /// can be turned on and off by setting this bit when you enable the EventSource.   Similarly
        /// the 'Debug' event can be turned on and off independently.  
        /// </summary>
        public class Keywords   // This is a bitvector
        {
            public const EventKeywords Requests = (EventKeywords)0x0001;
            public const EventKeywords Debug = (EventKeywords)0x0002;
        }
        
        public class Tasks
        {
            public const EventTask Request = (EventTask)0x1;
        }
        
        [Event(1, Keywords = Keywords.Requests, Task = Tasks.Request, Opcode=EventOpcode.Start)]
        public void RequestStart(int requestId, string url) => WriteEvent(1, requestId, url);
        
        [Event(2, Keywords = Keywords.Requests, Level = EventLevel.Verbose)]
        public void RequestPhase(int requestId, string phaseName) => WriteEvent(2, requestId, phaseName);

        [Event(3, Keywords = Keywords.Requests, Task = Tasks.Request, Opcode=EventOpcode.Stop)]
        public void RequestStop(int requestId) => WriteEvent(3, requestId);

        [Event(4, Keywords = Keywords.Debug)]
        public void DebugTrace(string message) => WriteEvent(4, message);
    }
}