using System;
using System.Collections.Generic;
using System.Text;
using ImageEdit.Core.Domain;

namespace ImageEdit.Core.Repositories
{
    public interface IEditTaskResultRepository
    {
        EditTaskResult GeTaskResult(Guid groupId, Guid taskId);
        void AddTaskResult(EditTaskResult result);
    }
}
