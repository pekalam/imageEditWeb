using System.Threading.Tasks;
using ImageEdit.Core.Domain;
using ImageEdit.Core.Repositories;
using ImageEdit.Core.Services;
using MassTransit;

namespace ImageEdit.Core.MessageConsumers
{
    public interface IAppTaskConsumer<T> : IConsumer<T> where T : class
    {
        Task ProccessTask(T task);
    }


    public class EditTaskConsumer : IAppTaskConsumer<EditTask>
    {
        private readonly IEditTaskResultRepository _editTaskResultRepository;
        private readonly IImgRepository _imgRepository;
        private readonly IEditTaskExecutorService _editTaskExecutorService;

        public EditTaskConsumer(IEditTaskResultRepository editTaskResultRepository, IImgRepository imgRepository, IEditTaskExecutorService editTaskExecutorService)
        {
            _editTaskResultRepository = editTaskResultRepository;
            _imgRepository = imgRepository;
            _editTaskExecutorService = editTaskExecutorService;
        }

        public virtual Task Consume(ConsumeContext<EditTask> context)
        {
            return ProccessTask(context.Message);
        }

        public Task ProccessTask(EditTask task)
        {
            var image = _imgRepository.GetImg(task.ImgId);

            var resultImg = _editTaskExecutorService.ExecuteTask(task);

            var taskResult = new EditTaskResult(task.Id, task.GroupId, resultImg, "png");

            _editTaskResultRepository.AddTaskResult(taskResult);
            return Task.CompletedTask;
        }
    }
}