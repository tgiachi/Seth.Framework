using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seth.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class ViewModelAttribute : Attribute
    {
        public Type View { get; set; }
        public ViewModelAttribute(Type view)
        {
            View = view;

        }
    }
}
