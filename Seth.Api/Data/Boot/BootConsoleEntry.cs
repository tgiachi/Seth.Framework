using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media;

namespace Seth.Api.Data.Boot
{
    public class BootConsoleEntry
    {
#pragma warning disable CS8618 // Non-nullable property 'Text' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string Text { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'Text' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

        public Brush Foreground { get; set; } = new SolidColorBrush(Colors.White);

        public Brush Background { get; set; } = new SolidColorBrush(Colors.Black);
    }
}
