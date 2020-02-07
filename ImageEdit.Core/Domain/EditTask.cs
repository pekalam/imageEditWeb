using System;

namespace ImageEdit.Core.Domain
{
    public class EditTask
    {
        public Guid Id { get; }
        public Guid GroupId { get; }
        public Guid ImgId { get; }
        public EditTaskAction EditTaskAction { get; }

        public EditTask(Guid id, Guid groupId, Guid imgId, EditTaskAction editTaskAction)
        {
            Id = id;
            GroupId = groupId;
            ImgId = imgId;
            EditTaskAction = editTaskAction;
        }

        public static EditTask Create(Guid groupId, Guid imgId, EditTaskAction editTaskAction)
        {
            return new EditTask(Guid.NewGuid(), groupId, imgId, editTaskAction);
        }
    }
}