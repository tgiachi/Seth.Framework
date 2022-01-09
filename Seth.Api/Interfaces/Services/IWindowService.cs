using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Seth.Api.Base;
using Seth.Api.Data.Window;
using Seth.Api.Interfaces.Windows;

namespace Seth.Api.Interfaces.Services
{
    public interface IWindowService : ISethService
    {
        
        ViewModelObject CurrentWindow { get; }
        ViewModelObject CreateWindow<TWindowModel>() where TWindowModel : IBaseWindow;

        Window MainWindow { get;  }

        void CreateMainWindow(Window mainWindow, object mainWindowModel);

        void BuildAndShowWindow<TViewModel>() where TViewModel : IBaseWindow;
    }
}
