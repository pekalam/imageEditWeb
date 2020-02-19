using System;

namespace ImageEdit.Core.Persistence.Entities
{
    public partial class DbImgTaskProgress
    {
        public Guid TaskId { get; set; }
        public Guid GroupId { get; set; }
        public int ImgTaskState { get; set; }

        public virtual DbImgTaskResult DbImgTaskResult { get; set; }
    }
}
