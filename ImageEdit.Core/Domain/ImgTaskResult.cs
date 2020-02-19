using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ImageEdit.Core.Domain
{
    public class ImgTaskResult
    {
        public Guid TaskId { get; private set; }
        public Guid GroupId { get; private set; }
        public Stream ImgStream { get; private set; }
        public string Extension { get; private set; }

        public ImgTaskResult(Guid taskId, Guid groupId, Stream imgStream, string extension)
        {
            if (imgStream == null && extension != null)
            {
                throw new ArgumentException();
            }

            TaskId = taskId;
            GroupId = groupId;
            ImgStream = imgStream;
            Extension = extension;
        }

        internal ImgTaskResult()
        {
        }

        public static ImgTaskResult FromTask(ImgTask task, Stream imgStream, string extension) =>
            new ImgTaskResult(task.TaskId, task.GroupId, imgStream, extension);
    }
}