using System;
using System.Collections.Generic;
using System.Text;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using ReactiveUI.Fody.Helpers;

namespace Seth.Ui.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ContentPresenter WindowContent { get; set; }

        [Reactive]
        public string WindowTitle { get; set; }
        [Reactive]
        public string DebugText { get; set; }

    }
}
