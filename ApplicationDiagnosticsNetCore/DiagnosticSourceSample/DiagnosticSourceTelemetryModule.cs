using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;

namespace DiagnosticSourceSample
{
    public sealed class HttpCoreDiagnosticSourceListener : IObserver<DiagnosticListener>, IObserver<KeyValuePair<string, object>>
    {
        public void OnCompleted() { }
        public void OnError(Exception error) { }
        
        public void OnNext(DiagnosticListener listener)
        {
            if (listener.Name == "HttpHandlerDiagnosticListener")
            {
                listener.Subscribe(this);
            }
        }
        public void OnNext(KeyValuePair<string, object> item)
        {
            Console.Write($"Event: {item.Key} ");
            if (Activity.Current != null)
            {
                Console.Write($"ActivityName: {Activity.Current?.OperationName} Id: {Activity.Current?.Id} ");
            }
            Console.WriteLine();
            Console.WriteLine(JsonSerializer.Serialize(item.Value));
            Console.WriteLine();
        }
    }
}