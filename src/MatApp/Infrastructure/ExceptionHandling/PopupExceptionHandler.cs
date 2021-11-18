using MatApp.Infrastructure.Dialogs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatApp.Infrastructure.ExceptionHandling
{
    public class PopupExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<PopupExceptionHandler> _log;
        private readonly IDialogService _dialogService;

        public PopupExceptionHandler(ILogger<PopupExceptionHandler> logger, IDialogService dialogService)
        {
            _log = logger;
            _dialogService = dialogService;
        }

        public void HandleException(Exception ex)
        {
            if (ex.IsFatal())
                throw ex; //application crash
            _log.LogError(ex, ex.Message);

            //Show error to ui
            StringBuilder sb = new StringBuilder();
            string exType = ex.GetType().Name;
            while (ex != null)
            {
                sb.AppendLine(ex.Message);
                sb.AppendLine("---------");
                sb.AppendLine("Please find more details in the application logs.");
                ex = ex.InnerException;
            }
            _dialogService.ShowErrorPopup(exType, sb.ToString());
        }
    }
}
