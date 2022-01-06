using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint;
using Serilog;
using Serilog.Core;
using Seth.Api.Attributes;
using Seth.Api.Interfaces.Services;

namespace Seth.Ui.Services
{
    [SethService(AutoStart = true)]
    public class ScriptEngineService : IScriptEngineService
    {
        private Engine _engine;
        private readonly ILogger _logger;
        private readonly IConfigService _configService;
        public string ScriptsDirectory { get; set; }

        public ScriptEngineService(ILogger logger, IConfigService configService)
        {
            _logger = logger;
            _configService = configService;
            ScriptsDirectory = Path.Join(_configService.RootDirectory, "scripts");
            logger.Information("Scripts directory: {Dir}", ScriptsDirectory);
            if (!Directory.Exists(ScriptsDirectory))
            {
                Directory.CreateDirectory(ScriptsDirectory);
            }

            InitializeEngine();
            ScanScripts();
        }

        private void InitializeEngine()
        {
            _engine = new Engine();
        }

        private void ScanScripts()
        {
            var scripts = Directory.GetFiles(ScriptsDirectory, "*.js", SearchOption.AllDirectories);
            _logger.Information("Found {Scripts} scripts to load", scripts.Length);
            scripts.ToList().ForEach(LoadScript);
        }

        public void LoadScript(string script)
        {
            try
            {
                _engine = _engine.Execute(script);
            }
            catch (Exception ex)
            {
                _logger.Error("Error during load script: {ex}", ex.Message);
            }
        }


    }
}
