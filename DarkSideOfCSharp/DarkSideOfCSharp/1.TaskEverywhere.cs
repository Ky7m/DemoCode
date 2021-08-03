using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.dotMemoryUnit;
using WorkflowCore.Interface;
using Xunit;
using Xunit.Abstractions;

namespace DarkSideOfCSharp
{
    public sealed class TaskEverywhere
    {
        public TaskEverywhere(ITestOutputHelper output)
        {
            DotMemoryUnitTestOutput.SetOutputMethod(output.WriteLine);
        }

        [Fact]
        [AssertTraffic(AllocatedObjectsCount = 1, Types = new[] {typeof(Task<string?>)})]
        public async Task TaskFromResultMemoryTraffic()
        {
            var queueProvider = new InMemoryQueueProvider();
            var i = 999;

            while (i --> 0)
            {
                await queueProvider.DequeueWork(QueueType.Workflow, CancellationToken.None);
                await queueProvider.DequeueWork(QueueType.Event, CancellationToken.None);
                await queueProvider.DequeueWork(QueueType.Index, CancellationToken.None);
            }
        }
    }
    
    
    public sealed class InMemoryQueueProvider : IQueueProvider
    {
        private readonly ConcurrentQueue<string> _workflows = new();
        private readonly ConcurrentQueue<string> _events = new();
        private readonly ConcurrentQueue<string> _indices = new();
        private readonly Task<string?> _nullValue = Task.FromResult<string?>(null);

        public bool IsDequeueBlocking => true;

        public Task QueueWork(string id, QueueType queue)
        {
            switch (queue)
            {
                case QueueType.Workflow:
                    _workflows.Enqueue(id);
                    break;
                case QueueType.Event:
                    _events.Enqueue(id);
                    break;
                case QueueType.Index:
                    _indices.Enqueue(id);
                    break;
            }

            return Task.CompletedTask;
        }

        public Task<string?> DequeueWork(QueueType queue, CancellationToken cancellationToken)
        {
            string? id = null;
            switch (queue)
            {
                case QueueType.Workflow:
                    _workflows.TryDequeue(out id);
                    break;
                case QueueType.Event:
                    _events.TryDequeue(out id);
                    break;
                case QueueType.Index:
                    _indices.TryDequeue(out id);
                    break;
            }

            if (id is null)
            {
                // uncomment to fix memory traffic
                // return _nullValue;
            }

            return Task.FromResult<string?>(id);
        }

        public Task Start()
        {
            return Task.CompletedTask;
        }

        public Task Stop()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
    
}