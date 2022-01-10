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
        public string Text { get; set; }

        public BootLogType  EventType { get; set; }
    }
}
