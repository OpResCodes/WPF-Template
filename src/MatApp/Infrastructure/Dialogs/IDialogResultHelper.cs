using System;

namespace MatApp.Infrastructure.Dialogs
{
    public interface IDialogResultHelper
    {
        void CloseDialog(bool withResult = false);
        event EventHandler<RequestCloseEventArgs> RequestCloseDialog;
    }
}
