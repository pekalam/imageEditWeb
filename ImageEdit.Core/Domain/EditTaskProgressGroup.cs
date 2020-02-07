using System;

namespace ImageEdit.Core.Domain
{
    public class EditTaskProgressGroup
    {
        public Guid GroupId { get; }
        public EditTaskProgress[] EditTasks { get; }

        public EditTaskProgressGroup(Guid groupId, EditTaskProgress[] editTasks)
        {
            GroupId = groupId;
            EditTasks = editTasks;
        }
    }
}