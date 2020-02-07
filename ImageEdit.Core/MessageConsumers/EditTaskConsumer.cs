using System.Threading.Tasks;
using ImageEdit.Core.Domain;
using MassTransit;

namespace ImageEdit.Core.MessageConsumers
{
    public class EditTaskConsumer : IConsumer<EditTask>
    {
        public Task Consume(ConsumeContext<EditTask> context)
        {
            return Task.CompletedTask;
        }
    }
}