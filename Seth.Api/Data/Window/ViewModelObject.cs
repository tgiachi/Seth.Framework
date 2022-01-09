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
        public UserControl View { get; set; }

        public IBaseWindow ViewModel { get; set; }
    }
}
