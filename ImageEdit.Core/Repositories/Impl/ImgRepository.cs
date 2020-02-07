using System;
using System.IO;

namespace ImageEdit.Core.Repositories.Impl
{
    internal class ImgRepository : IImgRepository
    {
        public Guid AddImg(Stream img)
        {
            var tempPath = Path.GetTempPath();
            var imgId = Guid.NewGuid();

            var path = $"{tempPath}/{imgId}";

            using (var fs = File.OpenWrite(path))
            {
                img.CopyTo(fs);
            }

            img.Close();

            return imgId;
        }

        public Stream GetImg(Guid imgId)
        {
            var tempPath = Path.GetTempPath();
            var path = $"{tempPath}/{imgId}";

            if (File.Exists(path))
            {
                return File.OpenRead(path);
            }

            return null;
        }
    }
}