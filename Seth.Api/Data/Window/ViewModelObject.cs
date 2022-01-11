using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Seth.Api.Interfaces.Windows;

namespace Seth.Api.Data.Window
{
    public class ViewModelObject
    {
#pragma warning disable CS8618 // Non-nullable property 'View' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public UserControl View { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'View' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

#pragma warning disable CS8618 // Non-nullable property 'ViewModel' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public IBaseWindow ViewModel { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'ViewModel' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
    }
}
