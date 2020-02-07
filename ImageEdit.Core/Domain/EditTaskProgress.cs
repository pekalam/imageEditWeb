using System;
using System.ComponentModel.DataAnnotations;

namespace ImageEdit.Core.Domain
{
    public class EditTaskProgress
    {
        [Key] public Guid TaskId { get; private set; }
        public Guid GroupId { get; private set; }
        public EditTaskState EditTaskState { get; set; }

        public EditTaskProgress(Guid taskId, Guid groupId, EditTaskState editTaskState)
        {
            TaskId = taskId;
            GroupId = groupId;
            EditTaskState = editTaskState;
        }

        internal EditTaskProgress()
        {
        }

        public static EditTaskProgress FromTask(EditTask editTask)
        {
            return new EditTaskProgress(editTask.Id, editTask.GroupId, EditTaskState.Pending);
        }
    }
}