using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ImageEdit.Core.Domain
{
    internal delegate ImgTask ImgTaskFactoryFunc(Guid groupId, Guid imgId,
        Dictionary<string, string> taskParams);

    public static class ImgTaskFactoryRegistry
    {
        private static readonly Dictionary<string, ImgTaskFactoryFunc> _taskNameToFactory =
            new Dictionary<string, ImgTaskFactoryFunc>();

        internal static void Init(Assembly assembly)
        {
            var imgTasksTypes = assembly
                .GetTypes()
                .Where(type => type.IsSubclassOf(typeof(ImgTask)));
            foreach (var imgTask in imgTasksTypes)
            {
                System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(imgTask.TypeHandle);
            }
        }

        internal static void Register(string taskName, ImgTaskFactoryFunc factoryFunc)
        {
            _taskNameToFactory.Add(taskName, factoryFunc);
        }

        public static ImgTask GetImgTask(Guid groupId, Guid imgId, string taskName,
            Dictionary<string, string> taskParams)
        {
            _taskNameToFactory.TryGetValue(taskName, out var factoryFunc);
            if (factoryFunc == null)
            {
                throw new ArgumentException($"Invalid task name: {taskName}");
            }

            var imgTask = factoryFunc(groupId, imgId, taskParams);
            return imgTask;
        }
    }
}