using System.Threading.Tasks;
using ImageEdit.Core.Domain;
using MassTransit;

namespace ImageEdit.Core.Services.Impl
{
    internal class MassTransitMqService : IMessageQueueService
    {
        private readonly IBusControl _bc;

        public MassTransitMqService(IBusControl bc)
        {
            _bc = bc;
        }

        public async Task QueueTaskAsync(EditTask editTask)
        {
            await _bc.Publish<EditTask>(editTask);
        }

        public void QueueTask(EditTask editTask)
        {
            _bc.Publish<EditTask>(editTask).Wait();
        }
    }
}