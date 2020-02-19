using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ImageMagick;
using Newtonsoft.Json;

namespace ImageEdit.Core.Domain
{
    public class ConvertImgTask : ImgTask
    {
        public const string Name = "convert";
        private static readonly string[] AllowedExtensions = new[] {"png", "jpg", "jpeg", "pdf"};

        static ConvertImgTask()
        {
            ImgTaskFactoryRegistry.Register(Name, (groupId, imgId, @params) => new ConvertImgTask(Guid.NewGuid(), groupId, imgId, @params));
        }

        private static void ValidateTaskParams(Dictionary<string, string> taskParams)
        {
            if (!taskParams.ContainsKey("to"))
            {
                throw new ArgumentException();
            }

            if (!AllowedExtensions.Contains(taskParams["to"]))
            {
                throw new ArgumentException("Extension not allowed");
            }
        }

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

        [JsonConstructor]
        internal ConvertImgTask(Guid taskId, Guid groupId, Guid imgId, Dictionary<string, string> taskParams) : base(taskId, groupId, imgId, Name, taskParams)
        {
            ValidateTaskParams(taskParams);
        }

        public override Img Execute(Img image)
        {
            using (MagickImage mImage = new MagickImage(image.ImageStream))
            {
                var resultImg = new MemoryStream();
                mImage.Format = ChooseTargetFormat(TaskParams["to"]);
                mImage.Write(resultImg);
                resultImg.Seek(0, SeekOrigin.Begin);
                return new Img(image.ImgId, resultImg, TaskParams["to"]);
            }
        }
    }
}
