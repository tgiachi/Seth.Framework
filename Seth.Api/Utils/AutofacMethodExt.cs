using Autofac;

namespace Airsoft.Modula.XRay.Core.Utils
{
    public static class AutofacMethodExt
    {
#pragma warning disable CS8618 // Non-nullable property 'Instance' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public static IContainer Instance { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'Instance' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

        public static void RegisterSelf(this ContainerBuilder builder)
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            IContainer container = null;
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8714 // The type 'Autofac.IContainer?' cannot be used as type parameter 'T' in the generic type or method 'RegistrationExtensions.Register<T>(ContainerBuilder, Func<IComponentContext, T>)'. Nullability of type argument 'Autofac.IContainer?' doesn't match 'notnull' constraint.
            builder.Register(c => container).AsSelf().SingleInstance();
#pragma warning restore CS8714 // The type 'Autofac.IContainer?' cannot be used as type parameter 'T' in the generic type or method 'RegistrationExtensions.Register<T>(ContainerBuilder, Func<IComponentContext, T>)'. Nullability of type argument 'Autofac.IContainer?' doesn't match 'notnull' constraint.
            builder.RegisterBuildCallback(c => container = (IContainer)c);
        }
    }
}