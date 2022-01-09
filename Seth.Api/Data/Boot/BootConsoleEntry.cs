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
        public string Text { get; set; }

        public Brush Foreground { get; set; } = new SolidColorBrush(Colors.White);

        public Brush Background { get; set; } = new SolidColorBrush(Colors.Black);
    }
}
