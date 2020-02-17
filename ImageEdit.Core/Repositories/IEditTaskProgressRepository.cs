using System;
using System.Runtime.CompilerServices;
using ImageEdit.Core.Domain;

[assembly: InternalsVisibleTo("UnitTests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace ImageEdit.Core.Repositories
{
    internal interface IEditTaskProgressRepository
    {
        EditTaskProgress[] GetTaskProgressesByGroupId(Guid groupId);
        void UpdateTaskProgress(EditTaskProgress editTask);
        void AddTaskProgress(EditTaskProgress editTask);
    }
}