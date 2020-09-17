using System;

namespace EventSourceSample
{
    class Program
    {
        static void Main()
        {
            // The model for EventSources is that any listeners that subscribe to an EventSource get the
            // messages.   This could be ETW, or the Windows EventLog or other EventListeners.   To demo
            // EventListeners, we  create an EventListener that also sends the logging messages to the console
            // and have ALL the demos send data there (as well as perhaps other places). 
            using (var eventListener = new ConsoleEventListener())
            {
                Console.WriteLine("******************** EventSource Demo ********************");
                Console.WriteLine("Sending a variety of messages, including 'Start', an 'Stop' Messages.");

                // Simulate some requests.  
                DoRequest("/home/index.aspx", 0);
                DoRequest("/home/catalog/100", 1);
                DoRequest("/home/catalog/121", 2);
                DoRequest("/home/catalog/144", 3);

                Console.WriteLine("Done generating events.");
                Console.WriteLine();
            }
        }
        
        private static void DoRequest(string request, int requestId)
        {
            LittleBitMoreThanMinimalEventSource.Log.RequestStart(requestId, request);

            foreach (var phase in new[] { "initialize", "query_db", "query_webservice", "process_results", "send_results" })
            {
                LittleBitMoreThanMinimalEventSource.Log.RequestPhase(requestId, phase);
                // simulate error on request for "/home/catalog/121"
                if (request == "/home/catalog/121" && phase == "query_db")
                {
                    LittleBitMoreThanMinimalEventSource.Log.DebugTrace("Error on page: " + request);
                    break;
                }
            }

            LittleBitMoreThanMinimalEventSource.Log.RequestStop(requestId);
        }
    }
}