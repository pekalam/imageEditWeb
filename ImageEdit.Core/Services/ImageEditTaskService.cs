using System;
using System.IO;
using System.Linq;
using ImageEdit.Core.Domain;
using ImageEdit.Core.Repositories;

namespace ImageEdit.Core.Services
{
    public class ImageEditTaskService
    {
        private readonly IImgRepository _imgRepository;
        private readonly IMessageQueueService _messageQueueService;
        private readonly IEditTaskProgressRepository _editTaskProgressRepository;

        internal ImageEditTaskService(IImgRepository imgRepository, IMessageQueueService messageQueueService,
            IEditTaskProgressRepository editTaskProgressRepository)
        {
            _imgRepository = imgRepository;
            _messageQueueService = messageQueueService;
            _editTaskProgressRepository = editTaskProgressRepository;
        }

        public void CreateEditTask(Stream img, EditTaskAction[] editTaskActions)
        {
            var imageId = _imgRepository.AddImg(img);
            var groupId = Guid.NewGuid();

            var tasks = editTaskActions.Select(action => EditTask.Create(groupId, imageId, action)).ToArray();
            foreach (var task in tasks)
            {
                var progress = EditTaskProgress.FromTask(task);
                _editTaskProgressRepository.AddTaskProgress(progress);
            }

            foreach (var task in tasks)
            {
                _messageQueueService.QueueTaskAsync(task).Wait();
            }
        }
    }
}