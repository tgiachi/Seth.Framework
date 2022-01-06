using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seth.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ScriptClassAttribute : Attribute
    {
        public string ObjectName { get; set; }

    }
}
