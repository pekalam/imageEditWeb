using System;
using System.Collections.Generic;
using System.IO;

namespace ImageEdit.Core.Domain
{
    public abstract class ImgTask
    {
        public Guid TaskId { get; }
        public Guid GroupId { get; }
        public Guid ImgId { get; }
        public string TaskName { get; }
        public Dictionary<string, string> TaskParams { get; }

        protected ImgTask(Guid taskId, Guid groupId, Guid imgId, string taskName, Dictionary<string, string> taskParams)
        {
            TaskId = taskId;
            GroupId = groupId;
            ImgId = imgId;
            TaskName = taskName;
            TaskParams = taskParams;
        }

        public abstract Img Execute(Img image);
    }
}