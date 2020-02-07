using System.Reflection;
using Autofac.Core.Activators.Reflection;

namespace ImageEdit.Core.RegisterModules
{
    internal class NonPublicConstructorFinder : DefaultConstructorFinder
    {
        public NonPublicConstructorFinder()
            : base(type => type.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
        }
    }
}