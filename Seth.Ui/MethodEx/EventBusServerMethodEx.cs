using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seth.Api.Data.Events;
using Seth.Api.Interfaces.Services;

namespace Seth.Ui.MethodEx
{
    public static class EventBusServerMethodEx
    {
        public static void SendBootLog(this IEventBusService eventBusService, string text, BootLogType type)
        {
            eventBusService.PublishEvent(new BootLogEvent()
            {
                Text = text,
                EventType = type
            });
        }
    }
}
