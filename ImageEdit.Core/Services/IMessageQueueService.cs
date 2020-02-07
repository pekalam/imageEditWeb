using System.Threading.Tasks;
using ImageEdit.Core.Domain;

namespace ImageEdit.Core.Services
{
    internal interface IMessageQueueService
    {
        Task QueueTaskAsync(EditTask editTask);
        void QueueTask(EditTask editTask);
    }
}