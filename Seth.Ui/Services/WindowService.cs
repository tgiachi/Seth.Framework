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


        public WindowService(IContainer container)
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
            window.DataContext = viewModel;
            return new ViewModelObject()
            {
                View = window,
                ViewModel = viewModel
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

        public void BuildAndShowWindow<TViewModel>() where TViewModel : IBaseWindow
        {
            var window = CreateWindow<TViewModel>();
            CurrentWindow = window;
            var mwvm = MainWindow.DataContext as MainWindowViewModel;

            Dispatcher.UIThread.InvokeAsync(() =>
            {
                mwvm.WindowTitle = window.ViewModel.GetType().Name;
                mwvm.WindowContent.Content = window.View;

            });

        }
    }
}
