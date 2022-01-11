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
#pragma warning disable CS8618 // Non-nullable property 'WindowContent' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public ContentPresenter WindowContent { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'WindowContent' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

        [Reactive]
#pragma warning disable CS8618 // Non-nullable property 'WindowTitle' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string WindowTitle { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'WindowTitle' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        [Reactive]
#pragma warning disable CS8618 // Non-nullable property 'DebugText' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string DebugText { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'DebugText' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

    }
}
