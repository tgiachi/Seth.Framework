using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using DynamicData;
using ReactiveUI.Fody.Helpers;
using Seth.Api.Attributes;
using Seth.Api.Data.Boot;
using Seth.Api.Interfaces.Windows;
using Seth.Ui.Views;

namespace Seth.Ui.ViewModels
{

    [ViewModel(typeof(BootWindow))]
    public class BootWindowViewModel : ViewModelBase, IBaseWindow
    {
        public string Title { get; set; }

        public ScrollViewer Scroller { get; set; }

        public ObservableCollection<BootConsoleEntry> ConsoleOutput { get; set; } = new();
        public void Dispose()
        {

        }

        public BootWindowViewModel()
        {

            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                int a = 0;
                while (true)
                {

                    await Task.Delay(100);
                    AddText($"Test {a}");

                    a++;
                    

                }

            });
        }

        public void AddText(string text)
        {
            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                if (ConsoleOutput.Count >= 150)
                    ConsoleOutput.Clear();

                ConsoleOutput.Add(new BootConsoleEntry()
                {
                    Text = text
                });

                Scroller.ScrollToEnd();
            });

        }


    }
}
