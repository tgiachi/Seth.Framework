using Autofac;

namespace Airsoft.Modula.XRay.Core.Utils
{
    public static class AutofacMethodExt
    {
        public static IContainer Instance { get; set; }

        public static void RegisterSelf(this ContainerBuilder builder)
        {
            IContainer container = null;
            builder.Register(c => container).AsSelf().SingleInstance();
            builder.RegisterBuildCallback(c => container = (IContainer)c);
        }
    }
}