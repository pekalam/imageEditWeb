using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ImageEdit.Core.Domain;
using ImageMagick;
using Microsoft.Extensions.Primitives;

namespace ImageEdit.Core.Services.Impl
{


    public class ConvertTaskExecutor
    {
        private static MagickFormat ChooseTargetFormat(string format)
        {
            switch (format)
            {
                case "png":
                    return MagickFormat.Png;
                case "jpg":
                    return MagickFormat.Jpg;
            }

            throw new ArgumentException();
        }

        public static Stream Convert(Stream image, EditTaskAction action)
        {
            using (MagickImage mImage = new MagickImage(image))
            {
                var resultImg = new MemoryStream();
                mImage.Format = ChooseTargetFormat(action.Params["to"]);
                mImage.Write(resultImg);
                return resultImg;
            }
        }
    }
}
