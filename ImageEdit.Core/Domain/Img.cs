using System;
using System.IO;

namespace ImageEdit.Core.Domain
{
    public class Img
    {
        public Guid ImgId { get; private set; }
        public Stream ImageStream { get; private set; }
        public string Extension { get; private set; }

        public Img(Guid imgId, Stream imageStream, string extension)
        {
            ImgId = imgId;
            ImageStream = imageStream;
            Extension = extension;
        }

        public Img()
        {

        }
    }
}