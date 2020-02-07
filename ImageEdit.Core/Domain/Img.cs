using System;

namespace ImageEdit.Core.Domain
{
    public class Img
    {
        public Guid ImgId { get; set; }
        public byte[] Image { get; set; }
        public string Extension { get; set; }
    }
}