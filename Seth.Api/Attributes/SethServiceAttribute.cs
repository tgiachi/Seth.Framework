using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seth.Api.Attributes
{
    public enum ServiceType
    {
        Singleton,
        PerRequest
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class SethServiceAttribute : Attribute
    {
        public ServiceType ServiceType { get; set; }

        public SethServiceAttribute(ServiceType serviceType = ServiceType.Singleton)
        {
            ServiceType = serviceType;
        }

    }
}
