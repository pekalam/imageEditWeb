using System;
using ImageEdit.Core.Domain;
using ImageEdit.Core.Repositories;

namespace ImageEdit.Core.Services.Impl
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

            return new EditTaskProgressGroup(groupId, tasks);
        }
    }
}