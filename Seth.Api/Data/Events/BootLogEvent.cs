using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seth.Api.Data.Events
{
    public enum BootLogType
    {
        Info,
        Error,
        Warning
    }
    public class BootLogEvent
    {
#pragma warning disable CS8618 // Non-nullable property 'Text' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.
        public string Text { get; set; }
#pragma warning restore CS8618 // Non-nullable property 'Text' must contain a non-null value when exiting constructor. Consider declaring the property as nullable.

        public BootLogType  EventType { get; set; }
    }
}
