using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ImageEdit.Core.Domain;

namespace ImageEdit.Core.Services
{
    public interface IEditTaskExecutorService
    {
        Stream ExecuteTask(EditTask task);
    }
}
