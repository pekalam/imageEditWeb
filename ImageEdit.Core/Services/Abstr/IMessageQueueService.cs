using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ImageEdit.Core.Domain;

[assembly:InternalsVisibleTo("UnitTests")]
namespace ImageEdit.Core.Services.Abstr
{
    internal interface IMessageQueueService
    {
        Task QueueTaskAsync(ImgTask imgTask);
        void QueueTask(ImgTask imgTask);
    }
}