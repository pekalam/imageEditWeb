using System;
using System.IO;
using System.Runtime.CompilerServices;
using ImageEdit.Core.Domain;
using ImageEdit.Core.Repositories;

[assembly: InternalsVisibleTo("UnitTests")]
namespace ImageEdit.Core.Services
{
    public class EditTaskProgressService
    {
        private readonly IEditTaskProgressRepository _editTaskProgressRepository;

        internal EditTaskProgressService(IEditTaskProgressRepository editTaskProgressRepository)
        {
            _editTaskProgressRepository = editTaskProgressRepository;
        }

        public EditTaskProgressGroup GetTaskProgressGroup(Guid groupId)
        {
            var tasks = _editTaskProgressRepository.GetTaskProgressesByGroupId(groupId);

            return tasks == null ? null : new EditTaskProgressGroup(groupId, tasks);
        }
    }

    public class TaskResultService
    {
        private readonly IEditTaskResultRepository _editTaskResultRepository;

        public TaskResultService(IEditTaskResultRepository editTaskResultRepository)
        {
            _editTaskResultRepository = editTaskResultRepository;
        }

        public EditTaskResult GetTaskResult(Guid groupId, Guid taskId)
        {
            var result = _editTaskResultRepository.GeTaskResult(groupId, taskId);
            return result;
        }
    }
}