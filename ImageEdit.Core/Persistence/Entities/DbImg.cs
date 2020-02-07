using System;

namespace ImageEdit.Core.Persistence.Entities
{
    public partial class DbImg
    {
        public Guid ImgId { get; set; }
        public byte[] Image { get; set; }
        public string Extension { get; set; }
    }
}
