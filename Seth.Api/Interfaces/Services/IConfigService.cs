using Seth.Api.Base;
using Seth.Api.Config;

namespace Seth.Api.Interfaces.Services
{
    public interface IConfigService : ISethService
    {
        ISethConfig Config { get; set; }

        string RootDirectory { get; set; }
    }
}
