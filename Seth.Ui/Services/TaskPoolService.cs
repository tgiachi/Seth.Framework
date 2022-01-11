using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Seth.Api.Attributes;
using Seth.Api.Interfaces.Services;

namespace Seth.Ui.Services
{
    [SethService(ServiceType.Singleton)]
    public class TaskPoolService : ITaskPoolService
    {
        private const int MaxTask = 40;
        private readonly Dictionary<string, Task> _tasksPools = new(MaxTask);

        public string AddTask(Task task)
        {
            var guid = Guid.NewGuid().ToString().Replace("-", "");
            _tasksPools.Add(guid, task);
            task.Start();
            return guid;
        }

        public void StopTask(string id)
        {
            if (_tasksPools[id] == null) return;

            _tasksPools[id].Dispose();
            _tasksPools.Remove(id);
        }
    }
}