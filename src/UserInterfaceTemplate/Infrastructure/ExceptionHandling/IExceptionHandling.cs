using System;

namespace UserInterfaceTemplate.Infrastructure.ExceptionHandling
{
    public interface IExceptionHandler
    {
        void HandleException(Exception ex);
    }
}
