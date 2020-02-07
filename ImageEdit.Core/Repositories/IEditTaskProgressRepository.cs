using System;
using ImageEdit.Core.Domain;

namespace ImageEdit.Core.Repositories
{
    internal interface IEditTaskProgressRepository
    {
        EditTaskProgress[] GetTaskProgressesByGroupId(Guid groupId);
        void UpdateTaskProgress(EditTaskProgress editTask);
        void AddTaskProgress(EditTaskProgress editTask);
    }
}