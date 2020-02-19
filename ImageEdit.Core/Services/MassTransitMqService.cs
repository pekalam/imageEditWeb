using System.Threading.Tasks;
using ImageEdit.Core.Domain;
using ImageEdit.Core.Services.Abstr;
using MassTransit;

namespace ImageEdit.Core.Services
{
    internal class MassTransitMqService : IMessageQueueService
    {
        private readonly IBusControl _bc;

        public MassTransitMqService(IBusControl bc)
        {
            _bc = bc;
        }

        public async Task QueueTaskAsync(ImgTask imgTask)
        {
            await _bc.Publish((object)imgTask);
        }

        public void QueueTask(ImgTask imgTask)
        {
            _bc.Publish((object)imgTask).Wait();
        }
    }
}