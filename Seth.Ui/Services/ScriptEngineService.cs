using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint;
using Jint.Native;
using Serilog;
using Serilog.Core;
using Seth.Api.Attributes;
using Seth.Api.Data.Events;
using Seth.Api.Interfaces.Services;
using Seth.Ui.MethodEx;

namespace Seth.Ui.Services
{
    [SethService(AutoStart = true)]
    public class ScriptEngineService : IScriptEngineService
    {
        private Engine _engine;
        private readonly ILogger _logger;
        private readonly IConfigService _configService;
        private readonly IEventBusService _eventBusService;
        public string ScriptsDirectory { get; set; }


        public ScriptEngineService(ILogger logger, IConfigService configService, IEventBusService eventBusService)

        {
            _logger = logger;
            _configService = configService;
            _eventBusService = eventBusService;
            ScriptsDirectory = Path.Join(_configService.RootDirectory, "scripts");
            logger.Information("Scripts directory: {Dir}", ScriptsDirectory);
            if (!Directory.Exists(ScriptsDirectory))
            {
                Directory.CreateDirectory(ScriptsDirectory);
            }

            InitializeEngine();
            BuildScriptClasses();
            ScanScripts();
        }

        private void BuildScriptClasses()
        {
            _engine = _engine.SetValue("include", new Func<string, JsValue>(IncludeFile));
            _engine = _engine.SetValue("log", new Action<string>(ConsoleLog));

        }

        private JsValue IncludeFile(string fileName)
        {
            var res = _engine.Evaluate(File.ReadAllText(Path.Join(ScriptsDirectory, fileName)));
            return res;
        }

        private void ConsoleLog(string text)
        {
            _logger.Information($"[ScriptEngine] {text}");
        }

        private void InitializeEngine()
        {
            _engine = new Engine();
        }

        private void ScanScripts()
        {
            var scripts = Directory.GetFiles(ScriptsDirectory, "*.js", SearchOption.AllDirectories);
            _logger.Information("Found {Scripts} scripts to load", scripts.Length);
            _eventBusService.SendBootLog($"Found {scripts.Length} scripts to load", BootLogType.Info);
            scripts.ToList().ForEach(LoadScriptFile);
        }

        public void LoadScriptFile(string script)
        {
            try
            {
                _eventBusService.SendBootLog($"Executing script {Path.GetFileName(script)}", BootLogType.Info);
                _engine = _engine.Execute(File.ReadAllText(script));
                _eventBusService.SendBootLog($"Script: {Path.GetFileName(script)}... OK", BootLogType.Info);
            }
            catch (Exception ex)
            {
                _logger.Error("Error during load script: {ex} => {stack}", ex.Message, ex.StackTrace);
                _eventBusService.SendBootLog($"Executing scripts {Path.GetFileName(script)}: {ex.Message} => {ex.StackTrace}", BootLogType.Error);
            }
        }


    }
}
