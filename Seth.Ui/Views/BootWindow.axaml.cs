using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Seth.Ui.ViewModels;

namespace Seth.Ui.Views
{
    public partial class BootWindow : UserControl
    {
        public BootWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            DataContextChanged += (sender, args) =>
            {
                if (DataContext is BootWindowViewModel bootWindowViewModel)
                {
                    bootWindowViewModel.Scroller = this.FindControl<ScrollViewer>("Scoller");
                }
            };


        }
    }
}
