using System;
using System.Runtime.CompilerServices;
using ImageEdit.Core.Domain;

[assembly: InternalsVisibleTo("UnitTests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace ImageEdit.Core.Repositories.Abstr
{
    public interface IImgTaskProgressRepository
    {
        ImgTaskProgress[] GetTaskProgressesByGroupId(Guid groupId);
        void UpdateTaskProgress(ImgTaskProgress imgTaskProgress);
        void AddTaskProgress(ImgTaskProgress imgTaskProgress);
    }
}