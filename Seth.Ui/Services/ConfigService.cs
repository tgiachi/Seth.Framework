using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Config.Net;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Seth.Api.Attributes;
using Seth.Api.Config;
using Seth.Api.Interfaces.Services;
using Seth.Api.Utils;

namespace Seth.Ui.Services
{

    [SethService(ServiceType.Singleton)]
    public class ConfigService : IConfigService
    {
        public ISethConfig Config { get; set; }
        private readonly ILogger _logger = Log.Logger;
        public ConfigService()
        {
            var configFile = Path.Join(EnvVariables.SethConfigPathEnv, "seth.json");
            _logger.Information("Loading config file {Config}", configFile);
            Config = new ConfigurationBuilder<ISethConfig>().UseJsonFile(configFile).Build();

        }


    }
}
