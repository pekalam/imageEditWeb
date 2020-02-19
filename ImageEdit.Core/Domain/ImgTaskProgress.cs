using System;
using System.ComponentModel.DataAnnotations;

namespace ImageEdit.Core.Domain
{
    public class ImgTaskProgress
    {
        public Guid TaskId { get; private set; }
        public Guid GroupId { get; private set; }
        public ImgTaskState ImgTaskState { get; set; }

        public ImgTaskProgress(Guid taskId, Guid groupId, ImgTaskState imgTaskState)
        {
            TaskId = taskId;
            GroupId = groupId;
            ImgTaskState = imgTaskState;
        }

        internal ImgTaskProgress()
        {
        }

        public static ImgTaskProgress FromTask(ImgTask editTask)
        {
            return new ImgTaskProgress(editTask.TaskId, editTask.GroupId, ImgTaskState.Pending);
        }
    }
}