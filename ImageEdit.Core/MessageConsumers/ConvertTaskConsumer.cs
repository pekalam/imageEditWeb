using System.Threading.Tasks;
using ImageEdit.Core.Domain;
using ImageEdit.Core.Repositories;
using ImageEdit.Core.Repositories.Abstr;
using ImageEdit.Core.Services;
using MassTransit;

namespace ImageEdit.Core.MessageConsumers
{
    public interface IAppTaskConsumer<T> : IConsumer<T> where T : class
    {
        Task ProcessTask(T task);
    }

    public class ConvertTaskConsumer : IAppTaskConsumer<ConvertImgTask>
    {
        private readonly IImgTaskResultRepository _imgTaskResultRepository;
        private readonly IImgTaskProgressRepository _imgTaskProgress;
        private readonly IImgRepository _imgRepository;

        public ConvertTaskConsumer(IImgTaskResultRepository imgTaskResultRepository, IImgTaskProgressRepository imgTaskProgressRepository, IImgRepository imgRepository)
        {
            _imgTaskResultRepository = imgTaskResultRepository;
            _imgTaskProgress = imgTaskProgressRepository;
            _imgRepository = imgRepository;
        }

        public virtual Task Consume(ConsumeContext<ConvertImgTask> context)
        {
            return ProcessTask(context.Message);
        }

        public Task ProcessTask(ConvertImgTask task)
        {
            var image = _imgRepository.GetImg(task.ImgId);

            var resultImg = task.Execute(image);

            var newProgress = ImgTaskProgress.FromTask(task);
            newProgress.ImgTaskState = ImgTaskState.Completed;

            _imgTaskProgress.UpdateTaskProgress(newProgress);

            var taskResult = ImgTaskResult.FromTask(task, resultImg.ImageStream, resultImg.Extension);
            _imgTaskResultRepository.UpdateTaskResult(taskResult);

            return Task.CompletedTask;
        }
    }
}