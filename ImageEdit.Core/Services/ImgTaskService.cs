using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ImageEdit.Core.Domain;
using ImageEdit.Core.Repositories;
using ImageEdit.Core.Repositories.Abstr;
using ImageEdit.Core.Services.Abstr;

namespace ImageEdit.Core.Services
{
    public class ImgTaskCreationParams
    {
        public string TaskName { get; set; }
        public Dictionary<string, string> TaskParams { get; set; }
    }

    public class ImgTaskService
    {
        private readonly IImgRepository _imgRepository;
        private readonly IMessageQueueService _messageQueueService;
        private readonly IImgTaskProgressRepository _imgTaskProgressRepository;
        private readonly IImgTaskResultRepository _imgTaskResultRepository;

        internal ImgTaskService(IImgRepository imgRepository, IMessageQueueService messageQueueService, IImgTaskProgressRepository imgTaskProgressRepository, IImgTaskResultRepository imgTaskResultRepository)
        {
            _imgRepository = imgRepository;
            _messageQueueService = messageQueueService;
            _imgTaskProgressRepository = imgTaskProgressRepository;
            _imgTaskResultRepository = imgTaskResultRepository;
        }

        public Guid CreateEditTask(Stream imgStream, string imgFormat, ImgTaskCreationParams[] taskCreationParams)
        {
            var image = new Img(Guid.NewGuid(), imgStream, imgFormat);
            _imgRepository.AddImg(image);
            var groupId = Guid.NewGuid();

            var tasks = taskCreationParams.Select(taskCreationParam =>
            {
                var task = ImgTaskFactoryRegistry.GetImgTask(groupId, image.ImgId, taskCreationParam.TaskName,
                    taskCreationParam.TaskParams);
                return task;
            }).ToArray();

            foreach (var task in tasks)
            {
                var imgTaskResult = ImgTaskResult.FromTask(task, null, null);
                _imgTaskResultRepository.AddTaskResult(imgTaskResult);
            }

            foreach (var task in tasks)
            {
                var progress = ImgTaskProgress.FromTask(task);
                _imgTaskProgressRepository.AddTaskProgress(progress);
            }

            foreach (var task in tasks)
            {
                _messageQueueService.QueueTaskAsync(task).Wait();
            }

            return groupId;
        }
    }
}