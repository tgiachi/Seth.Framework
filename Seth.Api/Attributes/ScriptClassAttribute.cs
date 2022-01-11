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
#pragma warning disable CS8618 // Non-nullable property 'ObjectName' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string ObjectName { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'ObjectName' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

    }
}
