using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ImageEdit.Core.Domain
{
    public class EditTaskResult
    {
        public Guid TaskId { get; private set; }
        public Guid GroupId { get; private set; }
        public Stream ImgStream { get; private set; }
        public string Extension { get; private set; }

        public EditTaskResult(Guid taskId, Guid groupId, Stream imgStream, string extension)
        {
            TaskId = taskId;
            GroupId = groupId;
            ImgStream = imgStream;
            Extension = extension;
        }

        internal EditTaskResult()
        {
            
        }
    }
}
