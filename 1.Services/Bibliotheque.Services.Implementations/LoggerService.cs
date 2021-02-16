using System;
using System.Collections.Generic;
using System.Text;

using Bibliotheque.Services.Contracts;

using log4net.Core;

using Microsoft.Extensions.Logging;

namespace Bibliotheque.Services.Implementations
{
    public class LoggerService : ILoggerService
    {
        private readonly ILogger<LoggerService> _logger;

        public LoggerService(ILogger<LoggerService> logger)
        {
            _logger = logger;
        }

        public void SetDebug(string text)
        {
            _logger.LogInformation(text);
        }

        public void SetError(string error)
        {
            _logger.LogError(error);
        }
    }
}
