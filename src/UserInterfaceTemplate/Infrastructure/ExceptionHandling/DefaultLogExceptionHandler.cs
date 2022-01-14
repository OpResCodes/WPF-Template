using Microsoft.Extensions.Logging;
using System;

namespace UserInterfaceTemplate.Infrastructure.ExceptionHandling
{
    public class DefaultLogExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<DefaultLogExceptionHandler> _log;
        public DefaultLogExceptionHandler(ILogger<DefaultLogExceptionHandler> logger)
        {
            _log = logger;
        }

        public void HandleException(Exception ex)
        {
            if (ex.IsFatal())
                throw ex; //application crash
            _log.LogError(ex, ex.Message);
        }
    }
}
