using System;
using ImageEdit.Core.Domain;

namespace ImageEdit.Core.Repositories.Abstr
{
    public interface IImgTaskResultRepository
    {
        ImgTaskResult GeTaskResult(Guid groupId, Guid taskId);
        void AddTaskResult(ImgTaskResult result);
        void UpdateTaskResult(ImgTaskResult result);
    }
}
