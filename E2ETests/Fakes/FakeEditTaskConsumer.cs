using System.Threading;
using System.Threading.Tasks;
using ImageEdit.Core.Domain;
using MassTransit;

namespace E2ETests.Fakes
{
    public class FakeEditTaskConsumer : IConsumer<EditTask>
    {
        public static object lck = new object();
        public static bool Called { get; set; }

        public Task Consume(ConsumeContext<EditTask> context)
        {
//            lock (lck)
//            {
//                Monitor.Wait(lck);
//                Monitor.Pulse(lck);
//            }

            Called = true;
            return Task.CompletedTask;
        }
    }
}