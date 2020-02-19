using System.Threading;
using System.Threading.Tasks;
using ImageEdit.Core.Domain;
using MassTransit;

namespace E2ETests.Fakes
{
    public class FakeEditTaskConsumer : IConsumer<ConvertImgTask>
    {
        public static object lck = new object();
        public static bool Called { get; set; }

        public Task Consume(ConsumeContext<ConvertImgTask> context)
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