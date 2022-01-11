using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Avalonia.Controls;
using Avalonia.Threading;
using ReactiveUI;
using Serilog;
using Serilog.Core;
using Seth.Api.Attributes;
using Seth.Api.Data.Window;
using Seth.Api.Interfaces.Services;
using Seth.Api.Interfaces.Windows;
using Seth.Ui.ViewModels;
using Seth.Ui.Views;
using Splat;
using ILogger = Serilog.ILogger;

namespace Seth.Ui.Services
{

    [SethService]
    public class WindowService : IWindowService
    {
        private readonly ILogger _logger = Log.ForContext<IWindowService>();
        private readonly IContainer _container;
        public ViewModelObject CurrentWindow { get; private set; }

        public Window MainWindow { get; private set; }


#pragma warning disable CS8618 // Non-nullable property 'CurrentWindow' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning disable CS8618 // Non-nullable property 'MainWindow' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public WindowService(IContainer container)
#pragma warning restore CS8618 // Non-nullable property 'MainWindow' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
#pragma warning restore CS8618 // Non-nullable property 'CurrentWindow' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        {
            _container = container;
            _logger.Information("Initializing Window service");
        }

        public ViewModelObject CreateWindow<TWindowModel>() where TWindowModel : IBaseWindow
        {
            var viewModelType = typeof(TWindowModel);
            var viewType = GetViewFromAttribute(viewModelType);
            var window = _container.Resolve(viewType) as UserControl;
            var viewModel = _container.Resolve(viewModelType) as IBaseWindow;
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            window.DataContext = viewModel;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            return new ViewModelObject()
            {
                View = window,
#pragma warning disable CS8601 // Possible null reference assignment.
                ViewModel = viewModel
#pragma warning restore CS8601 // Possible null reference assignment.
            };
        }

        private Type GetViewFromAttribute(Type viewModel)
        {
            var attr = viewModel.GetCustomAttribute<ViewModelAttribute>();
            if (attr == null)
                throw new Exception("Invalid windows");

            return attr.View;
        }

        public void CreateMainWindow(Window mainWindow, object mainWindowModel)
        {
            _logger.Information("Creating main window");
            mainWindow.DataContext = mainWindowModel;
            MainWindow = mainWindow;

        }

        private void DisposeCurrentWindow()
        {
            if (CurrentWindow != null)
            {
                CurrentWindow.ViewModel.Dispose();
            }
        }

        public void BuildAndShowWindow<TViewModel>() where TViewModel : IBaseWindow
        {
            DisposeCurrentWindow();
            var window = CreateWindow<TViewModel>();
            CurrentWindow = window;
            var mwvm = MainWindow.DataContext as MainWindowViewModel;

            Dispatcher.UIThread.InvokeAsync(() =>
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                mwvm.WindowTitle = window.ViewModel.GetType().Name;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                mwvm.WindowContent.Content = window.View;

            });

        }
    }
}
