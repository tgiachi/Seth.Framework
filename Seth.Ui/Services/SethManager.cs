using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Seth.Ui.Services
{
    public class SethManager
    {
        private ContainerBuilder _containerBuilder;


        public ContainerBuilder ScanForServices()
        {
            _containerBuilder = new ContainerBuilder();

            return _containerBuilder;
        }
    }
}
