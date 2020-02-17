using System;

namespace ImageEdit.Core.Persistence.Entities
{
    public partial class DbEditTaskResult
    {
        public long Id { get; set; }
        public Guid TaskId { get; set; }
        public Guid GroupId { get; set; }
        public byte[] Image { get; set; }
        public string Extension { get; set; }

        public virtual DbEditTaskProgress DbEditTaskProgress { get; set; }
    }
}
