using System.Reflection;
using Airsoft.Modula.XRay.Core.Utils;
using Autofac;
using AutofacSerilogIntegration;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using ReactiveUI;
using Serilog;
using Seth.Api.Attributes;
using Seth.Api.Interfaces.Services;
using Seth.Api.Manager;
using Seth.Api.Utils;
using Seth.Ui.ViewModels;
using Seth.Ui.Views;
using Splat;
using Splat.Autofac;

namespace Seth.Ui
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private static void RegisterLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.Debug()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public override void OnFrameworkInitializationCompleted()
        {
            RegisterLogger();
            var manager = new SethManager();

            var builder = manager.ScanForServices();
            AutofacDependencyResolver resolver = new(builder);
            Locator.SetLocator(resolver);
            builder.RegisterLogger();
            builder.RegisterSelf();
            var container = builder.Build();
            
            AutofacMethodExt.Instance = container;
          
            Locator.CurrentMutable.InitializeSplat();
            Locator.CurrentMutable.InitializeReactiveUI();

            Locator.CurrentMutable.RegisterConstant(new AvaloniaActivationForViewFetcher(),
                typeof(IActivationForViewFetcher));
            Locator.CurrentMutable.RegisterConstant(new AutoDataTemplateBindingHook(), typeof(IPropertyBindingHook));
            RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
            container.Resolve<IWindowService>().CreateMainWindow(new MainWindow(), new MainWindowViewModel());


            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                
                desktop.MainWindow = container.Resolve<IWindowService>().MainWindow;
            }

            container.Resolve(typeof(IConfigService));

            AssemblyUtils.GetAttribute<SethServiceAttribute>().ForEach(s =>
            {
                var attr = s.GetCustomAttribute<SethServiceAttribute>();
                if (attr.AutoStart)
                {
                    container.Resolve(s);
                }

            });
            container.Resolve<IWindowService>().BuildAndShowWindow<BootWindowViewModel>();


            base.OnFrameworkInitializationCompleted();
        }
    }
}
