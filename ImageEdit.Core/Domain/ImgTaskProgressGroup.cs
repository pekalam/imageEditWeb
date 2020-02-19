using System;

namespace ImageEdit.Core.Domain
{
    public class ImgTaskProgressGroup
    {
        public Guid GroupId { get; }
        public ImgTaskProgress[] EditTasks { get; }

        public ImgTaskProgressGroup(Guid groupId, ImgTaskProgress[] editTasks)
        {
            GroupId = groupId;
            EditTasks = editTasks;
        }
    }
}