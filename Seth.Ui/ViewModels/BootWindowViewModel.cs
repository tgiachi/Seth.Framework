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
using Redbus;
using Seth.Api.Attributes;
using Seth.Api.Data.Boot;
using Seth.Api.Data.Events;
using Seth.Api.Interfaces.Services;
using Seth.Api.Interfaces.Windows;
using Seth.Ui.Views;

namespace Seth.Ui.ViewModels
{

    [ViewModel(typeof(BootWindow))]
    public class BootWindowViewModel : ViewModelBase, IBaseWindow
    {
        private readonly IEventBusService _eventBusService;

        private SubscriptionToken _bootLogSubscriptionToken;
        [Reactive]
        public string Title { get; set; }

        public int ProgressBarValue { get; set; }

        public ScrollViewer Scroller { get; set; }

        public ObservableCollection<BootConsoleEntry> ConsoleOutput { get; set; } = new();


        public BootWindowViewModel(IEventBusService eventBusService)
        {
            _eventBusService = eventBusService;
            _bootLogSubscriptionToken = _eventBusService.SubscribeEvent<BootLogEvent>(OnBootLogEvent);

        }

        private void OnBootLogEvent(BootLogEvent obj)
        {
            switch (obj.EventType)
            {
                case BootLogType.Info:
                    AddText(obj.Text, Colors.White, Colors.Black);
                    break;
                case BootLogType.Error:
                    AddText(obj.Text, Colors.Red, Colors.Yellow);
                    break;
                case BootLogType.Warning:
                    AddText(obj.Text, Colors.Yellow, Colors.Black);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void AddText(string text, Color foreground, Color background)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
           {
               if (ConsoleOutput.Count >= 150)
                   ConsoleOutput.Clear();

               ConsoleOutput.Add(new BootConsoleEntry()
               {
                   Text = text,
                   Background = new SolidColorBrush(background),
                   Foreground = new SolidColorBrush(foreground)
               });

               Scroller.ScrollToEnd();
           });

        }

        public void SetProgressBarValue(int value)
        {
            ProgressBarValue = value;
        }

        public void Dispose()
        {
            _eventBusService.UnsubscribeEvent(_bootLogSubscriptionToken);
        }

    }
}
