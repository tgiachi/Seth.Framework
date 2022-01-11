using Seth.Api.Base;
using Seth.Api.Data.Loop;

namespace Seth.Api.Interfaces.Services
{
    public interface ILoopService : ISethService
    {
        LoopInfoObject CreateLoop(string name, Action callback, Action updateCallback, long targetTimer,
            LoopTimerDirection direction, long updateEveryMills = 0, int minTimeUpdate = 10, int maxTimeUpdate = 60);
    }
}