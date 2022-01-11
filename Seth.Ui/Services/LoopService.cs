using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Serilog;
using Seth.Api.Attributes;
using Seth.Api.Data.Loop;
using Seth.Api.Interfaces.Services;

namespace Seth.Ui.Services
{
    [SethService(ServiceType.Singleton)]
    public class LoopService : ILoopService
    {
        private readonly IEventBusService _eventBusService;
        private readonly ILogger _logger = Log.ForContext(typeof(LoopService));
        private readonly Dictionary<string, LoopInfoObject> _loopInfoObjects = new();
        private readonly Random _randomGenerator = new();
        private readonly ITaskPoolService _taskPoolService;

        public LoopService(ITaskPoolService taskPoolService, IEventBusService eventBusService)
        {
            _taskPoolService = taskPoolService;
            _eventBusService = eventBusService;
        }

        public LoopInfoObject CreateLoop(string name, Action callback, Action updateCallback, long targetTimer,
            LoopTimerDirection direction, long updateEveryMills = 0, int minTimeUpdate = 60, int maxTimeUpdate = 150)
        {
            var loopObject = new LoopInfoObject
            {
                Name = name,
                MaxUpdateTimer = maxTimeUpdate,
                MinUpdateTimer = minTimeUpdate,
                IsEnabled = true,
                Callback = callback,
                TargetMills = targetTimer,
                Direction = direction,
                UpdateEveryMills = updateEveryMills,
                UpdateCallback = updateCallback
            };

            loopObject.TaskId = _taskPoolService.AddTask(new Task(async () =>
            {
                var lastUpdate = 0.0;
                while (loopObject.IsEnabled)
                {
                    if (loopObject.IsPaused) continue;

                    var delay = _randomGenerator.Next(loopObject.MinUpdateTimer, loopObject.MaxUpdateTimer);
                    await Task.Delay(delay);

                    loopObject.CurrentMills +=
                        GetLoopMills(delay,
                            loopObject.Direction) * loopObject.TimerMultiply;

                    if (loopObject.UpdateEveryMills != 0)
                        if (loopObject.CurrentMills - lastUpdate >= loopObject.UpdateEveryMills)
                        {
                            lastUpdate = loopObject.CurrentMills;
                            loopObject.UpdateCallback?.Invoke();
                        }

                    if (loopObject.Direction == LoopTimerDirection.Backward)
                    {
                        if (loopObject.CurrentMills > loopObject.TargetMills) continue;
                        loopObject.IsEnabled = false;
                        loopObject.Callback.Invoke();
                    }
                    else
                    {
                        if (loopObject.CurrentMills < loopObject.TargetMills) continue;
                        loopObject.IsEnabled = false;
                        loopObject.Callback.Invoke();
                    }
                }
            }));

            return loopObject;
        }

        private static int GetLoopMills(int step, LoopTimerDirection direction)
        {
            if (direction == LoopTimerDirection.Backward)
                return step * -1;

            return step;
        }
    }
}