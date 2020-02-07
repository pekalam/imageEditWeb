using Autofac;
using Autofac.Builder;

namespace ImageEdit.Core.RegisterModules
{
    internal static class RegistrationBuilderExtensions
    {
        public static IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle>
            WithNonPublicCtors<TLimit, TReflectionActivatorData, TStyle>(
                this IRegistrationBuilder<TLimit, TReflectionActivatorData, TStyle> registration)
            where TReflectionActivatorData : ReflectionActivatorData
        {
            return registration.FindConstructorsWith(new NonPublicConstructorFinder());
        }
    }
}