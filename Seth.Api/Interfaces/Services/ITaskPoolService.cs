namespace Seth.Api.Interfaces.Services
{
    public interface ITaskPoolService
    {
        string AddTask(Task task);
        void StopTask(string id);
    }
}