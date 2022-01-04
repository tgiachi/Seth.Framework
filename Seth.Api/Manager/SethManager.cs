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
        private ContainerBuilder _containerBuilder;
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
                    $"Registering service {srv.Name} as {attribute.ServiceType} interfaceOf {interfaceType?.Name ?? "NONE"}");
                var registerChain = _containerBuilder.RegisterType(srv);


                registerChain = AssemblyUtils.GetInterfaceOfType(srv) == null
                    ? registerChain.AsSelf()
                    : registerChain.As(AssemblyUtils.GetInterfaceOfType(srv));
                registerChain = attribute.ServiceType == ServiceType.Singleton
                    ? registerChain.SingleInstance()
                    : registerChain.InstancePerRequest();
            });



            return _containerBuilder;
        }
    }
}
