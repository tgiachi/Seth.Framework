using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seth.Api.Interfaces.Windows
{
    public interface IBaseWindow : IDisposable
    {
        public string Title { get; set; }
    }
}
