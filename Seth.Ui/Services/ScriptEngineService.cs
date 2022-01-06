using System;
using System.Collections.Generic;
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
    [SethService(ServiceType.Singleton)]
    public class ScriptEngineService : IScriptEngineService
    {
        private readonly Engine _engine;
        private readonly ILogger _logger = Log.Logger;

        public ScriptEngineService()
        {
            _engine = new Engine();

        }

    }
}
