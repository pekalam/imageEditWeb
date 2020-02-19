using System;

namespace ImageEdit.Core.Persistence.Entities
{
    public partial class DbImgTaskResult
    {
        public Guid TaskId { get; set; }
        public Guid GroupId { get; set; }
        public byte[] Image { get; set; }
        public string Extension { get; set; }

        public virtual DbImgTaskProgress DbImgTaskProgress { get; set; }
    }
}
