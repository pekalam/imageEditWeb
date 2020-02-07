using System;

namespace ImageEdit.Core.Persistence.Entities
{
    public partial class DbEditTaskProgress
    {
        public Guid TaskId { get; set; }
        public Guid GroupId { get; set; }
        public int EditTaskState { get; set; }
    }
}
