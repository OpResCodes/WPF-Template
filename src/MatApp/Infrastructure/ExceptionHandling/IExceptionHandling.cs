using System;

namespace MatApp.Infrastructure.ExceptionHandling
{
    public interface IExceptionHandler
    {
        void HandleException(Exception ex);
    }
}
