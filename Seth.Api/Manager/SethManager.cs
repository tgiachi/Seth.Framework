using System.Reflection;
using Autofac;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Seth.Api.Attributes;
using Seth.Api.Utils;

namespace Seth.Api.Manager
{
    public class SethManager
    {
#pragma warning disable CS8618 // Non-nullable field '_containerBuilder' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
        private ContainerBuilder _containerBuilder;
#pragma warning restore CS8618 // Non-nullable field '_containerBuilder' must contain a non-null value when exiting constructor. Consider declaring the field as nullable.
        private readonly ILogger logger = Log.Logger.ForContext<SethManager>();


        public ContainerBuilder ScanForServices()
        {
            logger.Information("Scanning for services");
            _containerBuilder = new ContainerBuilder();
            var services = AssemblyUtils.GetAttribute<SethServiceAttribute>();
            logger.Information("Found {Service} services", services.Count);

            services.ForEach(srv =>
            {
                var attribute = srv.GetCustomAttribute<SethServiceAttribute>();
                var interfaceType = AssemblyUtils.GetInterfaceOfType(srv);
                logger.Information(
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    $"Registering service {srv.Name} as {attribute.ServiceType} interfaceOf {interfaceType?.Name ?? "NONE"}");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                var registerChain = _containerBuilder.RegisterType(srv);


                registerChain = AssemblyUtils.GetInterfaceOfType(srv) == null
                    ? registerChain.AsSelf()
                    : registerChain.As(AssemblyUtils.GetInterfaceOfType(srv)).As(srv);

                registerChain = attribute.ServiceType == ServiceType.Singleton
                    ? registerChain.SingleInstance()
                    : registerChain.InstancePerRequest();
            });

            logger.Information("Scanning for windows");
            var windows = AssemblyUtils.GetAttribute<ViewModelAttribute>();

            logger.Information("Found {Service} windows", windows.Count);
            windows.ForEach(v =>
            {
                var attribute = v.GetCustomAttribute<ViewModelAttribute>();
                _containerBuilder.RegisterType(v).AsSelf().InstancePerLifetimeScope();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                _containerBuilder.RegisterType(attribute.View).AsSelf().InstancePerLifetimeScope();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                logger.Information(
                    $"Registering window {v.Name} [view: {attribute.View.Name}]");
            });



            return _containerBuilder;
        }
    }
}
