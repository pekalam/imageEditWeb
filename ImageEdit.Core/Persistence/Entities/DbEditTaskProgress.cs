using System;
using System.Collections.Generic;

namespace ImageEdit.Core.Persistence.Entities
{
    public partial class DbEditTaskProgress
    {
        public DbEditTaskProgress()
        {
            DbEditTaskResult = new HashSet<DbEditTaskResult>();
        }

        public Guid TaskId { get; set; }
        public Guid GroupId { get; set; }
        public int EditTaskState { get; set; }

        public virtual ICollection<DbEditTaskResult> DbEditTaskResult { get; set; }
    }
}
